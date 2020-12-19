using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(EventManager))]
public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private GameObject spriteGroup;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerNotification alert;
    [SerializeField] private float _parcelGrabDistanceSq = 5f;
    [SerializeField] private float _parcelHoldDistance = 5f;

    [SerializeField] private AudioClip liftParcel;
    [SerializeField] private AudioClip dropParcel;
    [SerializeField] private AudioClip [] footsteps;
    [SerializeField] private AudioClip addXPSFX;
    [SerializeField] private AudioClip levelUpSFX;
    [SerializeField] private AudioClip abilityUnlockedSFX;
    [SerializeField] private AudioSource notificationAudioSource;
    private bool useFootstep1 = true;
    [SerializeField] private AudioSource footstepsAudioSource1;
    [SerializeField] private AudioSource footstepsAudioSource2;
    [SerializeField] private AudioSource holdPackageAudioSource;
    [SerializeField] private AudioSource holdPackageAudioSourceLoop;

    private Hat hat;
    private Hair hair;
    private Color skinColor;
    private Color clothingColor;
    public bool allowedToLevitate = false;


    private bool running = false;
    private bool holdingParcel = false;
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

    private void PlayAudio(AudioSource source, AudioClip clip, bool loop = false)
    {
        if(source.isPlaying)
        {
            source.Stop();
        }
        source.clip = clip;
        source.loop = loop;
        source.Play();
    }

    public void PlayFootstep()
    {
        int randomFootstep = UnityEngine.Random.Range(0, footsteps.Length);
        AudioSource source = (useFootstep1) ? footstepsAudioSource1 : footstepsAudioSource2;
        useFootstep1 = !useFootstep1;
        PlayAudio(source, footsteps[randomFootstep]);
    }

    public void UpdatePlayerVisuals(PlayerVisuals playerVisuals)
    {
        playerVisuals.SetHairstyle(hair);
        playerVisuals.SetHat(hat);
        playerVisuals.SetSkinColor(skinColor);
        playerVisuals.SetClothingColor(clothingColor);
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
            if(!running)
            {
                animator.SetBool("Running", true);
                running = true;
            }
            _parcelOffset = movement * _parcelHoldDistance;
            _parcelOffset.y /= 3;
        }
        else
        {
            if(running)
            {
                animator.SetBool("Running", false);
                running = false;
            }
        }
        if(movement.x != 0)
        {
            float xScale = 1f;
            if(movement.x < 0)
            {
                xScale = -1f;
            }
            spriteGroup.transform.localScale = new Vector3(xScale, 1f, 1f);
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
                if(!holdingParcel)
                {
                    holdingParcel = true;
                    animator.SetBool("HoldParcel", true);
                }
                PlayAudio(holdPackageAudioSource, liftParcel, false);
                holdPackageAudioSourceLoop.Play();
                events.DispatchEvent("onPlayerGrabParcel");
            }
        }
        else if(!context.performed) // Not started + not performed, so releasing the button.
        {
            if(heldParcel != null)
            {
                heldParcel.SetHeld(false);
                heldParcel = null;
                if(holdingParcel)
                {
                    holdingParcel = false;
                    animator.SetBool("HoldParcel", false);
                }
                PlayAudio(holdPackageAudioSource, dropParcel, false);
                holdPackageAudioSourceLoop.Stop();
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

    public void PlayXPAudio()
    {
        PlayAudio(notificationAudioSource, addXPSFX, false);
    }

    public void PlayLevelUpAudio()
    {
        PlayAudio(notificationAudioSource, levelUpSFX, false);
    }

    public void PlayAbilityUnlockedAudio()
    {
        PlayAudio(notificationAudioSource, abilityUnlockedSFX, false);
    }

    public void AddXP(int amount)
    {
        level.AddXP(amount);
        alert.Alert("+" + amount + "xp");
        events.DispatchEvent("onPlayerXPUpdated");
    }

    public void HighlightNearbyParcel(List<Parcel> activeParcels)
    {
        if(!allowedToLevitate) return;
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

    public void SetSkinColor(Color color)
    {
        skinColor = color;
        PlayerVisuals playerVisuals = player.GetComponent<PlayerVisuals>();
        playerVisuals.SetSkinColor(skinColor);
    }

    public void SetClothingColor(Color color)
    {
        clothingColor = color;
        PlayerVisuals playerVisuals = player.GetComponent<PlayerVisuals>();
        playerVisuals.SetClothingColor(clothingColor);
    }

    public void SetHairstyle(Hair hair)
    {
        this.hair = hair;
        PlayerVisuals playerVisuals = player.GetComponent<PlayerVisuals>();
        playerVisuals.SetHairstyle(hair);
    }

    public void SetHat(Hat hat)
    {
        this.hat = hat;
        PlayerVisuals playerVisuals = player.GetComponent<PlayerVisuals>();
        playerVisuals.SetHat(hat);
    }

    internal void AddAbilitySpeed()
    {
        speed += speedIncrement;
    }
}
