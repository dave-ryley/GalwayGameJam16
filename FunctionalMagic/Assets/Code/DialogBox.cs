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

    void Start()
    {
        speechBubble.SetActive(false);
    }

    void Update()
    {
        if(hideTime > 0 && hideTime < Time.time)
        {
            hideTime = 0f;
            speechBubble.SetActive(false);
        }
    }

    public void SetName(string text)
    {
        nameField.text = text;
    }

    public void Say(string text, float duration = 1f)
    {
        speechBubble.SetActive(true);
        textField.text = text;
        hideTime = duration + Time.time;
    }
}
