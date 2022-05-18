using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: An implementation of cells
/// ���{��F�Z���̎���
/// </summary>
public class Cell : ICell
{
    public Vector2Int gridPosition { get; private set; }
    public Vector3 worldPosition { get; private set; }
    public Vector2 cellSize { get; private set; }

    /// <summary>
    /// English: Public constructor for cell on the grid
    /// ���{��F�O���b�h��ɂ���Z���̃p�u���b�N�R���X�g���N�^
    /// </summary>
    /// <param name="_gridPosition"></param>
    /// <param name="_worldPosition"></param>
    /// <param name="_cellSize"></param>
    public Cell(Vector2Int _gridPosition, Vector3 _worldPosition, Vector2 _cellSize)
    {
        gridPosition = _gridPosition;
        worldPosition = _worldPosition;
        cellSize = _cellSize;
    }

}
