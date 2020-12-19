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
    [SerializeField] private string greetingText = "Hello Friend!";
    [SerializeField] private string goodbyeText = "Goodbye!";
    [SerializeField] private string abilityToBeImpressedBy = "LEVITATION";
    [SerializeField] private int abilityLevel = 1;
    [SerializeField] private string notImpressedText = "Huh, you don't even have a LEVITATION level 1 skill...";
    [SerializeField] private string impressedText = "Wow, I'm impressed. You have LEVITATION level 1!";

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
                    Say(greetingText, 2f);
                }
                GameLogic.GetInstance().EnterRangeOfNPC();
            }
            else
            {
                Say(goodbyeText);
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
