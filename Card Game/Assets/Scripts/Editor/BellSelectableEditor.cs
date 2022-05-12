using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BellSelectable))]
public class BellSelectableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BellSelectable myBell = (BellSelectable)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Ring Bell"))
        {
            myBell.RingBell();
        }

    }
}
