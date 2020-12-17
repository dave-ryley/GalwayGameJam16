using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EventManager))]
public class GameLogic : MonoBehaviour
{
    private static GameLogic instance;
    private int _fullScreenPanelCountOpen = 0;
    private List<NPC> _npcs;
    private int _curNpcIndex = 0;
    [SerializeField] private ParcelManager parcelManager;
    [SerializeField] private Ability[] abilities;

    public Player player;
    public EventManager events;

    public CharacterSheet characterSheet;

    public static GameLogic GetInstance()
    {
        return instance;
    }

    #region game loop

    void Awake()
    {
        instance = this;
        events = GetComponent<EventManager>();
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
    }

    void Update()
    {
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

    #endregion
}
