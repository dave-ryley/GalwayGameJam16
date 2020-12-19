using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
    public string [] randomNames;
    public Hair [] hairstyles;
    public Hat [] hats;
    private int hatIndex = -1;
    private int hairIndex = -1;

    void Start()
    {
        Open();
    }

    public void Open()
    {
        GameLogic game = GameLogic.GetInstance();
        game.OpenFullScreenPanel();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        GameLogic game = GameLogic.GetInstance();
        game.CloseFullScreenPanel();
        gameObject.SetActive(false);
    }

    public string GetRandomName()
    {
        return randomNames[UnityEngine.Random.Range(0, randomNames.Length)];
    }

    public void PreviousHairstyle()
    {
        Hair hair = null;
        if(hairIndex < 0)
        {
            hairIndex = hairstyles.Length - 1;
            hair = hairstyles[hairIndex];
        }
        else if(hairIndex > 0)
        {
            hairIndex--;
            hair = hairstyles[hairIndex];
        }
        else
        {
            hairIndex--;
        }
        GameLogic.GetInstance().GetPlayer().SetHairstyle(hair);
    }

    public void NextHairstyle()
    {
        Hair hair = null;
        if(hairIndex == hairstyles.Length - 1)
        {
            hairIndex = -1;
        }
        else
        {
            hairIndex++;
            hair = hairstyles[hairIndex];
        }
        GameLogic.GetInstance().GetPlayer().SetHairstyle(hair);

    }

    public void PreviousHat()
    {
        Hat hat = null;
        if(hatIndex < 0)
        {
            hatIndex = hats.Length - 1;
            hat = hats[hatIndex];
        }
        else if(hatIndex > 0)
        {
            hatIndex--;
            hat = hats[hatIndex];
        }
        else
        {
            hatIndex--;
        }
        GameLogic.GetInstance().GetPlayer().SetHat(hat);
    }

    public void NextHat()
    {
        Hat hat = null;
        if(hatIndex == hats.Length - 1)
        {
            hatIndex = -1;
        }
        else
        {
            hatIndex++;
            hat = hats[hatIndex];
        }
        GameLogic.GetInstance().GetPlayer().SetHat(hat);
    }
}
