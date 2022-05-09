using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector2Int m_gridPosition;
    public Vector2 m_cellSize;
    public Card m_card;

    public Cell(Vector2Int _gridPosition, Vector2 _cellSize)
    {
        m_gridPosition = _gridPosition;
        m_cellSize = _cellSize;
        m_card = null;
    }

    public void InsertCard(Card _card)
    {
        if(_card == null) { return; }
        m_card = _card;
    }

    public void RemoveCard()
    {
        m_card = null;
    }
}