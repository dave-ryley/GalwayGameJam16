using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameLogic game = GameLogic.GetInstance();
        game.events.AddEventListener("onFullScreenPanelOpened", OnFullScreenPanelOpened);
        game.events.AddEventListener("onFullScreenPanelClosed", OnFullScreenPanelClosed);
    }

    #region events

    private void OnFullScreenPanelOpened()
    {
        gameObject.SetActive(false);
    }

    private void OnFullScreenPanelClosed()
    {
        gameObject.SetActive(true);
    }

    #endregion
}
