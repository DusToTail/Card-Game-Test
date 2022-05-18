using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class to control the size of the grid and cell and return cell at world position
/// 日本語：グリッドとセルのサイズを管理したり、ワールド位置からセルを返したりするクラス
/// </summary>
public class TwoDimensionGridController : IGridController
{
    public ICell[,] grid { get; private set; }

    public Vector2Int gridSize { get; private set; }

    public Vector2 cellSize { get; private set; }

    public Vector3 startWorldPosition { get; private set; }

    /// <summary>
    /// English: Public constructor for initializing the necessary variables for creating the grid
    /// 日本語：グリッドを作成するのに必要な変数を初期化するコンストラクタ
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="gridSize"></param>
    /// <param name="cellSize"></param>
    public TwoDimensionGridController(Transform parent, Vector2Int gridSize, Vector2 cellSize)
    {
        InitializeGrid(parent, gridSize, cellSize);

        Vector2Int centerGridPosition = new Vector2Int(Mathf.FloorToInt(gridSize.x / 2), Mathf.FloorToInt(gridSize.y / 2));
        Vector2Int dirFromCenter = new Vector2Int(0, 0) - centerGridPosition;
        startWorldPosition = new Vector3(dirFromCenter.x * cellSize.x, parent.position.y, dirFromCenter.y * cellSize.y);
        this.gridSize = gridSize;
        this.cellSize = cellSize;
    }

    /// <summary>
    /// English; Return a cell at world position using XZ coordinates
    /// 日本語：ワールド位置のXZ座標でセルを返す
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public ICell GetCellAtCoordinateXZ(Vector3 worldPosition)
    {
        float x = (worldPosition.x - startWorldPosition.x) / (2 * cellSize.x);
        float y = (worldPosition.y - startWorldPosition.y) / (2 * cellSize.y);
        if (x < 0 || x > startWorldPosition.x + gridSize.x * (2 * cellSize.x))
            return null;
        if (y < 0 || y > startWorldPosition.y + gridSize.y * (2 * cellSize.y))
            return null;

        Vector2Int gridPosition = new Vector2Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
        return grid[gridPosition.y, gridPosition.x];
    }

    /// <summary>
    /// English: Initialize the grid
    /// 日本語：グリッドを初期化する
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="gridSize"></param>
    /// <param name="cellSize"></param>
    public void InitializeGrid(Transform parent, Vector2Int gridSize, Vector2 cellSize)
    {
        grid = new Cell[gridSize.y, gridSize.x];
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                int childIndex = y * gridSize.x + x;
                
                grid[y, x] = new Cell(new Vector2Int(x,y), parent.GetChild(childIndex).transform.position, cellSize);
            }
        }
    }

    /// <summary>
    /// English: Highlight the cell for seconds in scene view
    /// 日本語：シーンビューでセルを秒でハイライトする
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="timeInSeconds"></param>
    public static void HighlightCellForSeconds(ICell cell, float timeInSeconds)
    {
        Vector3[] vertices = new Vector3[4];
        vertices[0] = cell.worldPosition + new Vector3(-cell.cellSize.x / 2, 0, -cell.cellSize.y / 2);
        vertices[1] = cell.worldPosition + new Vector3(cell.cellSize.x / 2, 0, -cell.cellSize.y / 2);
        vertices[2] = cell.worldPosition + new Vector3(cell.cellSize.x / 2, 0, cell.cellSize.y / 2);
        vertices[3] = cell.worldPosition + new Vector3(-cell.cellSize.x / 2, 0, cell.cellSize.y / 2);
        for(int i = 0; i < vertices.Length; i++)
        {
            if (i == vertices.Length - 1)
                Debug.DrawLine(vertices[i], vertices[0], Color.red, timeInSeconds);
            else
                Debug.DrawLine(vertices[i], vertices[i + 1], Color.red, timeInSeconds);
        }
    }
}
