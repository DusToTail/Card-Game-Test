using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridController
{
    public ICell[,] grid { get; }
    public Vector2Int gridSize { get; }
    public Vector2 cellSize { get; }
    public Vector3 startWorldPosition { get; }
    public void InitializeGrid(Transform parent, Vector2Int gridSize, Vector2 cellSize);

    public ICell GetCellAtCoordinateXZ(Vector3 worldPosition);
}
