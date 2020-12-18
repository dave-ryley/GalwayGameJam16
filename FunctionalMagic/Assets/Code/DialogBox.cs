using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogBox : MonoBehaviour
{
    private float hideTime = 0f;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TextMeshProUGUI nameField;
    private AudioSource audioSource;

    void Update()
    {
        if(hideTime > 0 && hideTime < Time.time)
        {
            hideTime = 0f;
            speechBubble.SetActive(false);
            audioSource.enabled = false;
        }
    }

    public void Setup(AudioSource audioSource)
    {
        speechBubble.SetActive(false);
        this.audioSource = audioSource;
    }

    public void SetName(string text)
    {
        nameField.text = text;
    }

    public void Say(string text, float duration, AudioClip clip)
    {
        speechBubble.SetActive(true);
        textField.text = text;
        hideTime = duration + Time.time;
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.enabled = true;
        audioSource.Play();
    }
}
