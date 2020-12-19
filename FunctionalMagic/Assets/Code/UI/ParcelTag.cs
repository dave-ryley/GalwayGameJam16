using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParcelTag : MonoBehaviour
{
    public TextMeshProUGUI fromText;
    public TextMeshProUGUI toText;
    public TextMeshProUGUI noteText;

    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        GameLogic game = GameLogic.GetInstance();
        Player player = game.GetPlayer();

        player.events.AddEventListener("onPlayerGrabParcel", UpdateParcelTag);
        player.events.AddEventListener("onPlayerDropParcel", UpdateParcelTag);
        player.events.AddEventListener("onPlayerHighlightParcel", UpdateParcelTag);
        player.events.AddEventListener("onPlayerClearParcelHighlight", UpdateParcelTag);

        UpdateParcelTag();
    }

    // Update is called once per frame
    void UpdateParcelTag()
    {
        Player player = GameLogic.GetInstance().GetPlayer();
        Parcel parcel = null;
        if(player.heldParcel != null)
        {
            parcel = player.heldParcel;
        }
        else if(player.highlightedParcel != null)
        {
            parcel = player.highlightedParcel;
        }
        if(parcel != null)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            fromText.text = parcel.from;
            toText.text = parcel.deliverTo.characterName;
            noteText.text = parcel.note;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
