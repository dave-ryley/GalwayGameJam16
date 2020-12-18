using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnvironmentPiece))]
public class EnvironmentPieceEditor : Editor
{
    public void OnSceneGUI()
    {
        var t = target as EnvironmentPiece;

        Transform child = t.transform.GetChild(0);
        child.position = new Vector3(child.position.x, child.position.y, t.transform.position.y);
    }
}
