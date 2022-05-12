using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : ICell
{
    public Vector2Int gridPosition { get; private set; }
    public Vector3 worldPosition { get; private set; }
    public Vector2 cellSize { get; private set; }

    public Cell(Vector2Int _gridPosition, Vector3 _worldPosition, Vector2 _cellSize)
    {
        gridPosition = _gridPosition;
        worldPosition = _worldPosition;
        cellSize = _cellSize;
    }

}
