using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public const float squareDistanceRequired = 5f * 5f;
    public bool playerInRange = false;
    [SerializeField] private DialogBox dialogBox;

    public string characterName;
    public bool includedInDeliveryList = true;

    void Start()
    {
        dialogBox.SetName(characterName);
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
                    dialogBox.Say(expectedParcel.waitingForDialog, 3f);
                }
                else
                {
                    dialogBox.Say("Hello Friend!", 2f);
                }
            }
            else
            {
                dialogBox.Say("Goodbye!");
            }
        }
    }

    internal void ReceiveParcel(Parcel parcel)
    {
        dialogBox.Say(parcel.recievedDialog, 3f);
    }
}
