using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Board", menuName = "ScriptableObjects/Board", order = 1)]
public class BoardScriptableObject : ScriptableObject
{
    public Vector2Int gridSize;
    public Vector2 cellSize;
}
