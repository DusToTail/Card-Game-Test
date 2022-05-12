using UnityEngine;

public interface ICell
{
    public Vector2 cellSize { get; }
    public Vector2Int gridPosition { get; }
    public Vector3 worldPosition { get; }

}