using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(EventManager))]
public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private PlayerNotification alert;
    [SerializeField] private float _parcelGrabDistanceSq = 5f;
    [SerializeField] private float _parcelHoldDistance = 5f;
    private List<Ability> _acquiredAbilities;
    private Vector3 _parcelOffset;
    public Parcel heldParcel;
    public Parcel highlightedParcel;

    public EventManager events;
    public PlayerLevel level;
    public float speed = 1f;
    private float speedIncrement = 0.01f;

    void Awake()
    {
        events = GetComponent<EventManager>();
        _acquiredAbilities = new List<Ability>();
        speedIncrement = 0.01f * speed;
    }

    void Update()
    {
        if(heldParcel)
        {
            heldParcel.transform.position = player.transform.position + _parcelOffset;
        }
    }

    internal void BuildAbilityTree(Ability[] abilities)
    {
        level.SetUp(this, abilities);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        player.velocity = movement * speed;
        if(movement.sqrMagnitude != 0)
        {
            _parcelOffset = movement * _parcelHoldDistance;
            _parcelOffset.y /= 3;
        }
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(highlightedParcel != null)
            {
                heldParcel = highlightedParcel;
                highlightedParcel = null;
                heldParcel.SetHeld(true);
                events.DispatchEvent("onPlayerGrabParcel");
            }
        }
        else if(!context.performed) // Not started + not performed, so releasing the button.
        {
            if(heldParcel != null)
            {
                heldParcel.SetHeld(false);
                heldParcel = null;
                events.DispatchEvent("onPlayerDropParcel");
            }
        }
    }

    public Vector3 GetPosition()
    {
        return player.transform.position;
    }

    public bool IsHoldingParcel()
    {
        return heldParcel != null;
    }

    public void AddXP(int amount)
    {
        level.AddXP(amount);
        alert.Alert("+" + amount + "xp");
        events.DispatchEvent("onPlayerXPUpdated");
    }

    public void HighlightNearbyParcel(List<Parcel> activeParcels)
    {
        Parcel nearestParcel = null;
        float closestDistanceSq = float.MaxValue;
        foreach(Parcel parcel in activeParcels)
        {
            float distance = (player.transform.position - parcel.transform.position).sqrMagnitude;
            if(distance < _parcelGrabDistanceSq && distance < closestDistanceSq)
            {
                nearestParcel = parcel;
                closestDistanceSq = distance;
            }
        }
        if(closestDistanceSq < _parcelGrabDistanceSq)
        {
            if(highlightedParcel != nearestParcel)
            {
                if(highlightedParcel != null)
                {
                    highlightedParcel.SetHighlighted(false);
                }
                highlightedParcel = nearestParcel;
                nearestParcel.SetHighlighted(true);
                events.DispatchEvent("onPlayerHighlightParcel");
            }
        }
        else if(highlightedParcel != null)
        {
            highlightedParcel.SetHighlighted(false);
            highlightedParcel = null;
            events.DispatchEvent("onPlayerClearParcelHighlight");
        }
    }

    internal void AddAbilitySpeed()
    {
        speed += speedIncrement;
    }
}
