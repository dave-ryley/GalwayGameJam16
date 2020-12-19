using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EventManager))]
public class GameLogic : MonoBehaviour
{
    public enum GameStage { CharacterCreation, CallOver, Tutorial, MainGame }

    public GameStage curGameStage = GameStage.CharacterCreation;
    private static GameLogic instance;
    private int _fullScreenPanelCountOpen = 0;
    private List<NPC> _npcs;
    private int _curNpcIndex = 0;
    [SerializeField] private ParcelManager parcelManager;
    [SerializeField] private Ability[] abilities;

    [SerializeField] private CinemachineVirtualCamera characterCreaterCam;
    [SerializeField] private CinemachineVirtualCamera wideShot;
    [SerializeField] private CinemachineVirtualCamera closeShot;

    private int changeMusicEveryXLevels = 5;
    private int curMusicIndex = 0;
    public int npcsInRange = 0;
    [SerializeField] private AudioClip [] music;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioClip uiClickSound;
    [SerializeField] private AudioClip uiHoverSound;
    [SerializeField] private AudioSource uiAudioSource;

    internal void EnterRangeOfNPC()
    {
        if(npcsInRange == 0)
        {
            closeShot.Priority = 30;
        }
        npcsInRange++;
    }

    public Player player;
    public EventManager events;

    internal void ExitRangeOfNPC()
    {
        Debug.Log("ExitRange");
        npcsInRange--;
        if(npcsInRange == 0)
        {
            closeShot.Priority = 0;
        }
    }

    public CharacterSheet characterSheet;

    public static GameLogic GetInstance()
    {
        return instance;
    }

    #region game loop

    void Awake()
    {
        instance = this;
        _npcs = new List<NPC>();
        NPC[] npcs = FindObjectsOfType<NPC>();
        if(npcs.Length == 0)
        {
            Debug.Log("No NPCs found!");
        }
        foreach(NPC npc in npcs)
        {
            if(npc.includedInDeliveryList)
            {
                _npcs.Add(npc);
            }
        }
        player.BuildAbilityTree(abilities);
        player.events.AddEventListener("onPlayerLevelUp", OnPlayerLevelUp);
        AudioClip newMusic = music[0];
        musicPlayer.clip = newMusic;
        musicPlayer.Play();
    }

    void Update()
    {
        if(curGameStage != GameStage.MainGame) return;
        _curNpcIndex++;
        if(_curNpcIndex > _npcs.Count)
        {
            _curNpcIndex = 0;
        }
        if(_curNpcIndex < _npcs.Count)
        {
            _npcs[_curNpcIndex].UpdateNPC(player);
        }
    }

    #endregion

    #region public members

    internal bool ExpectingParcel(NPC deliverTo, out Parcel expectedParcel)
    {
        return parcelManager.ExpectingParcel(deliverTo, out expectedParcel);
    }

    public void DeliverParcel(Parcel parcel, NPC recipient)
    {
        parcelManager.SetParcelInactive(parcel);
        recipient.ReceiveParcel(parcel);
        player.AddXP(80);
        parcelManager.GenerateNewParcel();
    }

    public NPC GetRandomNPC()
    {
        int index = UnityEngine.Random.Range(0, _npcs.Count);
        return _npcs[index];
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenCharacterSheet()
    {
        characterSheet.Open();
    }

    public void OpenFullScreenPanel()
    {
        if(_fullScreenPanelCountOpen == 0)
        {
            PlayerInput input = player.GetComponent<PlayerInput>();
            input.enabled = false;
            events.DispatchEvent("onFullScreenPanelOpened");
        }
        _fullScreenPanelCountOpen++;
    }

    public void CloseFullScreenPanel()
    {
        _fullScreenPanelCountOpen--;
        if(_fullScreenPanelCountOpen == 0)
        {
            PlayerInput input = player.GetComponent<PlayerInput>();
            input.enabled = true;
            events.DispatchEvent("onFullScreenPanelClosed");
        }
    }

    public void Add100XP()
    {
        player.AddXP(100);
    }

    public Player GetPlayer()
    {
        return player;
    }

    private void OnPlayerLevelUp()
    {
        int newMusicIndex = player.level.level/changeMusicEveryXLevels;
        if(newMusicIndex != curMusicIndex && newMusicIndex < music.Length)
        {
            curMusicIndex = newMusicIndex;
            AudioClip newMusic = music[newMusicIndex];
            musicPlayer.Stop();
            musicPlayer.clip = newMusic;
            musicPlayer.Play();
        }
    }

    private void PlayAudio(AudioSource source, AudioClip clip)
    {
        if(source.isPlaying)
        {
            source.Stop();
        }
        source.clip = clip;
        source.Play();
    }

    public void PlayHoverAudio()
    {
        PlayAudio(uiAudioSource, uiHoverSound);
    }

    public void PlayClickAudio()
    {
        PlayAudio(uiAudioSource, uiClickSound);
    }

    public void CompleteCharacterCreation()
    {
        characterCreaterCam.Priority = 0;
        curGameStage = GameStage.CallOver;
    }

    public void ArrivedAtPostman()
    {
        EnterRangeOfNPC();
        PlayerInput input = player.GetComponent<PlayerInput>();
        input.enabled = false;
        curGameStage = GameStage.Tutorial;
    }

    public void FinishPostmanConversation()
    {
        ExitRangeOfNPC();
        PlayerInput input = player.GetComponent<PlayerInput>();
        input.enabled = true;
        player.AddXP(110);
        curGameStage = GameStage.MainGame;
    }

    #endregion
}
