using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public const float squareDistanceRequired = 5f * 5f;
    public bool playerInRange = false;
    [SerializeField] private DialogBox _dialogBox;
    [SerializeField] private AudioSource _audioSource;

    public string characterName;
    public bool includedInDeliveryList = true;

    public AudioClip [] simSpeak;

    void Start()
    {
        _dialogBox.Setup(_audioSource);
        _dialogBox.SetName(characterName);
    }

    public void UpdateNPC(Player player)
    {
        Vector3 distance = player.GetPosition() - transform.position;
        distance.z = 0f;
        bool nowInRange = distance.sqrMagnitude < squareDistanceRequired;
        if (nowInRange != playerInRange)
        {
            playerInRange = nowInRange;
            if(playerInRange)
            {
                if(GameLogic.GetInstance().ExpectingParcel(this, out Parcel expectedParcel))
                {
                    Say(expectedParcel.waitingForDialog, 3f);
                }
                else
                {
                    Say("Hello Friend!", 2f);
                }
                GameLogic.GetInstance().EnterRangeOfNPC();
            }
            else
            {
                Say("Goodbye!");
                GameLogic.GetInstance().ExitRangeOfNPC();
            }
        }
    }

    internal void ReceiveParcel(Parcel parcel)
    {
        Say(parcel.recievedDialog, 3f);
    }

    public void Say(string dialog, float time = 1f)
    {
        int randomAudioIndex = UnityEngine.Random.Range(0, simSpeak.Length);
        AudioClip clip = simSpeak[randomAudioIndex];
        _dialogBox.Say(dialog, time, clip);
    }
}
