using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerHand))]
public class PlayerHandEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerHand myHand = (PlayerHand)target;
        DrawDefaultInspector();

        if (Application.isPlaying)
        {
            for (int i = 0; i < myHand.cards.Count; i++)
            {
                EditorGUILayout.LabelField($"Card index {i}", myHand.cards[i].name);
            }

        }

    }
}
