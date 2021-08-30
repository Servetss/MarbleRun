using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Jump))]
public class MarbleJumpEditor : Editor
{
    private Jump _jump;

    private void OnEnable()
    {
        _jump = target as Jump;
    }

    private void OnSceneGUI()
    {
        if (_jump._trajectoryPoints != null)
        {
            Handles.color = Color.red;

            Handles.DrawLines(_jump._trajectoryPoints);
        }
    }
}
