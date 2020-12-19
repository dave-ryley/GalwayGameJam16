using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHat", menuName = "Create New Hat", order = 3)]
public class Hat : ScriptableObject
{
    public Sprite image;
    public Vector2 offset;
}
