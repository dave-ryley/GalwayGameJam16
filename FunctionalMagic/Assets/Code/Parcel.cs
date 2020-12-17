using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcel : MonoBehaviour
{
    enum HeldState { None, Active, Held }

    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private SpriteRenderer _highlightSprite;
    private HeldState _heldState = HeldState.None;
    private Animator _animator;
    private bool _highlighted = false;

    public NPC deliverTo;
    public string from = "";
    public string note = "";
    public string waitingForDialog = "";
    public string recievedDialog = "Thanks for the parcel!";

    void Start()
    {
        _animator = GetComponent<Animator>();
        UpdateState();
    }

    public bool IsVisible()
    {
        return _sprite.isVisible;
    }

    public void SetHighlighted(bool state)
    {
        _highlighted = state;
        if(_heldState != HeldState.Held)
        {
            _heldState = (_highlighted) ? HeldState.Active : HeldState.None;
        }
        UpdateState();
    }

    public void SetHeld(bool state)
    {
        if(state)
        {
            _heldState = HeldState.Held;
            _animator.SetBool("Hold", true);
        }
        else
        {
            _animator.SetBool("Hold", false);
            if(deliverTo.playerInRange)
            {
                GameLogic.GetInstance().DeliverParcel(this, deliverTo);
                _highlighted = false;
            }
            _heldState = (_highlighted) ? HeldState.Active : HeldState.None;
        }
        UpdateState();
    }

    public void SetParcelSprites(Sprite image, Sprite highlight)
    {
        _sprite.sprite = image;
        _highlightSprite.sprite = highlight;
    }

    private void UpdateState()
    {
        switch(_heldState)
        {
            case HeldState.None:
                _highlightSprite.gameObject.SetActive(false);
                break;
            case HeldState.Active:
                _highlightSprite.gameObject.SetActive(true);
                _highlightSprite.color = Color.cyan;
                break;
            case HeldState.Held:
                _highlightSprite.gameObject.SetActive(true);
                _highlightSprite.color = Color.yellow;
                break;
        }
    }
}
