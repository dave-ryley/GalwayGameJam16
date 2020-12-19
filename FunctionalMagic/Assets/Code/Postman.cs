using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Postman : MonoBehaviour
{
    private NPC nPC;
    public string [] heyOverHere;
    public string [] introDialog;
    public int curIntroIndex = 0;
    public bool callOverSequence = true;
    public bool tutorialSequence = false;
    public float timer = 0f;

    void Start()
    {
        nPC = GetComponent<NPC>();
    }

    public void SayNext()
    {
        if(curIntroIndex >= introDialog.Length)
        {
            GameLogic.GetInstance().FinishPostmanConversation();
            tutorialSequence = false;
            return;
        }
        nPC.Say(introDialog[curIntroIndex], 3f);
        timer = Time.time + 3f;
        curIntroIndex++;
    }

    public void CallOver()
    {
        nPC.Say(heyOverHere[Random.Range(0, heyOverHere.Length)], 1f);
        timer = Time.time + 1f;
    }

    void Update()
    {
        if(callOverSequence)
        {
            UpdatePostman(GameLogic.GetInstance().player);
            if(timer < Time.time)
            {
                CallOver();
            }
        }
        else if(tutorialSequence)
        {
            if(timer < Time.time)
            {
                SayNext();
            }
        }
    }

    public void UpdatePostman(Player player)
    {
        Vector3 distance = player.GetPosition() - transform.position;
        distance.z = 0f;
        bool nowInRange = distance.sqrMagnitude < NPC.squareDistanceRequired/2;
        if (nowInRange)
        {
            callOverSequence = false;
            tutorialSequence = true;
            GameLogic game = GameLogic.GetInstance();
            game.ArrivedAtPostman();
        }
    }
}
