using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHairstyle", menuName = "Create New Hairstyle", order = 2)]
public class Hair : ScriptableObject
{
    public Sprite image;
    public Vector2 offset;
    public Vector2 hatOffset;
}
