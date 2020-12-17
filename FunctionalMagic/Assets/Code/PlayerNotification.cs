using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNotification : MonoBehaviour
{
    private float hideTime = 0f;
    [SerializeField] private TextMeshProUGUI textField;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(hideTime > 0 && hideTime < Time.time)
        {
            hideTime = 0f;
            gameObject.SetActive(false);
        }
    }

    public void Alert(string text, float duration = 1f)
    {
        gameObject.SetActive(true);
        textField.text = text;
        hideTime = duration + Time.time;
    }
}
