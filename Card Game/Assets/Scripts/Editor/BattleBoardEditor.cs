using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BattleBoard))]
public class BattleBoardEditor : Editor
{
    public BattlePlayer player;
    public override void OnInspectorGUI()
    {
        BattleBoard myBattleBoard = (BattleBoard)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Generate Cells"))
        {
            myBattleBoard.InitializeBoard(player);
        }

    }
}
