using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BattlePlayer))]
public class BattlePlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BattlePlayer myPlayer = (BattlePlayer)target;
        DrawDefaultInspector();

        if (Application.isPlaying)
            EditorGUILayout.LabelField("Select Manager Current State", myPlayer.selectManager.state.ToString());

        if (GUILayout.Button("Draw Initial Hand"))
        {
            myPlayer.DrawInitialHand();
        }
        if (GUILayout.Button("Draw One From Player Deck"))
        {
            myPlayer.DrawOneFromPlayerDeck();
        }
        if (GUILayout.Button("Draw One Squirrel"))
        {
            myPlayer.DrawOneSquirrel();
        }
    }
}
