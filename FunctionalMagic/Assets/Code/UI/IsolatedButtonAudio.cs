using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IsolatedButtonAudio : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioClip uiClickSound;
    [SerializeField] private AudioClip uiHoverSound;
    [SerializeField] private AudioSource uiAudioSource;

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayAudio(uiHoverSound);
    }

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        PlayAudio(uiClickSound);
    }

    private void PlayAudio(AudioClip clip)
    {
        if(uiAudioSource.isPlaying)
        {
            uiAudioSource.Stop();
        }
        uiAudioSource.clip = clip;
        uiAudioSource.Play();
    }
}
