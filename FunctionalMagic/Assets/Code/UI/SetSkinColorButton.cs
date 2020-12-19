using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSkinColorButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Color color = GetComponent<Image>().color;
        GameLogic.GetInstance().GetPlayer().SetSkinColor(color);
    }
}
