using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensionGridController : IGridController
{
    public ICell[,] grid { get; private set; }

    public Vector2Int gridSize { get; private set; }

    public Vector2 cellSize { get; private set; }

    public Vector3 startWorldPosition { get; private set; }

    public TwoDimensionGridController(Transform parent, Vector2Int gridSize, Vector2 cellSize)
    {
        InitializeGrid(parent, gridSize, cellSize);

        Vector2Int centerGridPosition = new Vector2Int(Mathf.FloorToInt(gridSize.x / 2), Mathf.FloorToInt(gridSize.y / 2));
        Vector2Int dirFromCenter = new Vector2Int(0, 0) - centerGridPosition;
        startWorldPosition = new Vector3(dirFromCenter.x * cellSize.x, parent.position.y, dirFromCenter.y * cellSize.y);
        this.gridSize = gridSize;
        this.cellSize = cellSize;
    }


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

    public void InitializeGrid(Transform parent, Vector2Int gridSize, Vector2 cellSize)
    {
        grid = new Cell[gridSize.y, gridSize.x];
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject cell = new GameObject($"{x}:{y}");
                cell.transform.parent = parent;
                cell.gameObject.layer = LayerMask.NameToLayer(Tags.SELECTABLE_LAYER);
                cell.gameObject.tag = Tags.BATTLE_BOARD_CELL_TAG;
                cell.AddComponent<BoxCollider>();
                cell.GetComponent<BoxCollider>().size = new Vector3(cellSize.x, 1, cellSize.y);
                cell.transform.localPosition = Vector3.zero;
                Vector2Int centerGridPosition = new Vector2Int(Mathf.FloorToInt(gridSize.x / 2), Mathf.FloorToInt(gridSize.y / 2));
                Vector2Int dirFromCenter = new Vector2Int(x, y) - centerGridPosition;
                cell.transform.position += new Vector3(dirFromCenter.x * cellSize.x, 0, dirFromCenter.y * cellSize.y);
                grid[y, x] = new Cell(new Vector2Int(x,y), cell.transform.position, cellSize);
            }
        }
    }
}
