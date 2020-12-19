using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private Hat hat;
    private Hair hair;
    [SerializeField] private SpriteRenderer hatSprite;
    [SerializeField] private SpriteRenderer hairSprite;
    [SerializeField] private SpriteRenderer head;
    [SerializeField] private SpriteRenderer leftHand;
    [SerializeField] private SpriteRenderer rightHand;
    [SerializeField] private SpriteRenderer neck;
    [SerializeField] private SpriteRenderer clothing;

    public void SetHat(Hat hat)
    {
        this.hat = hat;
        hatSprite.gameObject.SetActive(hat != null);
        if(hat != null)
        {
            hatSprite.sprite = hat.image;
            float xPos = hat.offset.x;
            float yPos = hat.offset.y;
            if(hair != null)
            {
                xPos += hair.hatOffset.x;
                yPos += hair.hatOffset.y;
            }
            hatSprite.transform.localPosition = new Vector3(xPos, yPos, this.hatSprite.transform.localPosition.z);
        }
    }

    public void SetHairstyle(Hair hair)
    {
        this.hair = hair;
        hairSprite.gameObject.SetActive(hair != null);
        if(hair != null)
        {
            hairSprite.sprite = hair.image;
            hairSprite.transform.localPosition = new Vector3(hair.offset.x, hair.offset.y, hairSprite.transform.localPosition.z);
        }
        if(hat != null)
        {
            SetHat(hat);
        }
    }

    public void SetSkinColor(Color skinColor)
    {
        head.color = skinColor;
        leftHand.color = skinColor;
        rightHand.color = skinColor;
        neck.color = skinColor;
    }

    public void SetClothingColor(Color clothingColor)
    {
        clothing.color = clothingColor;
    }
}
