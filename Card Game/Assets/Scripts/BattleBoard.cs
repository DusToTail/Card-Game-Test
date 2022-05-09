using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BattleBoard : MonoBehaviour
{
    public Vector2Int m_gridSize;
    public Cell[,] m_grid;
    public Vector2 m_cellSize { get; private set; }


    [SerializeField]
    private Texture m_texture;
    [SerializeField]
    private Sprite m_portrait;

    private int m_curPlayerIndex;

    [SerializeField]
    private bool displayGizmos;

    private void OnEnable()
    {
        Bell.OnBellRinged += PlayBattlePhaseOfPlayer;
    }

    private void OnDisable()
    {
        Bell.OnBellRinged -= PlayBattlePhaseOfPlayer;
    }

    private void Start()
    {
        m_curPlayerIndex = 0;

        m_cellSize = new Vector2(transform.lossyScale.x / m_gridSize.x, transform.lossyScale.z / m_gridSize.y);
        GetComponent<BoxCollider>().size = new Vector3(m_cellSize.x * (float)m_gridSize.x, 1, m_cellSize.y * (float)m_gridSize.y);
        m_grid = new Cell[m_gridSize.y, m_gridSize.x];
        for(int y = 0; y < m_gridSize.y; y++)
        {
            for(int x = 0; x < m_gridSize.x; x++)
            {
                m_grid[y, x] = new Cell(m_gridSize, m_cellSize);
                GameObject cell = new GameObject($"Cell [{x},{y}]");
                cell.transform.parent = transform;
                cell.AddComponent<BoxCollider>();
                cell.GetComponent<BoxCollider>().size = new Vector3(m_cellSize.x, 1, m_cellSize.y);
                cell.transform.localPosition = Vector3.zero;
                Vector2Int centerGridPosition = new Vector2Int(Mathf.FloorToInt(m_gridSize.x / 2), Mathf.FloorToInt(m_gridSize.y / 2));
                Vector2Int dirFromCenter = new Vector2Int(x,y) - centerGridPosition;
                cell.transform.position += new Vector3(dirFromCenter.x * m_cellSize.x, 0, dirFromCenter.y * m_cellSize.y);
            }
        }

    }


    public void PlayCardAt(Card _card, Vector2Int _cellPosition)
    {
        if (m_grid[_cellPosition.x, _cellPosition.y].m_card != null) { Debug.Log($"There is already a card at {_cellPosition}"); return; }


        Debug.Log($"Play {_card.m_cardName} at {_cellPosition}");
        m_grid[_cellPosition.x, _cellPosition.y].InsertCard(_card);
    }

    public void PlayBattlePhaseOfPlayer()
    {
        for(int index = 0; index < m_gridSize.x; index++)
        {
            if(m_grid[index, m_curPlayerIndex] == null) { continue; }
            if(m_grid[index, m_curPlayerIndex].m_card == null) { continue; }
            CardAtCellCommitAttack(new Vector2Int(index, m_curPlayerIndex));
        }
    }

    private void CardAtCellCommitAttack(Vector2Int _cellPosition)
    {
        if(m_grid[_cellPosition.x, _cellPosition.y].m_card == null) { return; }

        m_grid[_cellPosition.x, _cellPosition.y].m_card.Attack(GetOppositeCells(m_grid[_cellPosition.x, _cellPosition.y]));

    }

    private Cell[] GetOppositeCells(Cell _cell)
    {
        if(_cell.m_gridPosition.y == 0) 
        { 
            return new Cell[2]{ m_grid[_cell.m_gridPosition.x, 1], m_grid[_cell.m_gridPosition.x, 2] }; 
        }
        else 
        { 
            return new Cell[1] { m_grid[_cell.m_gridPosition.x, 0] }; 
        }
    }

    private void SwitchPlayerIndex()
    {
        if (m_curPlayerIndex == 0) { m_curPlayerIndex = 1; }
        else { m_curPlayerIndex = 0; }
    }

    private void OnDrawGizmos()
    {
        if (!displayGizmos) { return; }
        Gizmos.color = Color.green;
        Vector2 cellSize = new Vector2(transform.lossyScale.x / m_gridSize.x, transform.lossyScale.z / m_gridSize.y);
        for (int x = 0; x < m_gridSize.x; x++)
        {
            for (int y = 0; y < m_gridSize.y; y++)
            {
                Vector2Int centerGridPosition = new Vector2Int(Mathf.FloorToInt(m_gridSize.x / 2), Mathf.FloorToInt(m_gridSize.y / 2));
                Vector2Int dirFromCenter = new Vector2Int(x, y) - centerGridPosition;
                Vector3 cellPosition = transform.position + new Vector3(dirFromCenter.x * cellSize.x, 0, dirFromCenter.y * cellSize.y);
                Gizmos.DrawWireCube(cellPosition, new Vector3(cellSize.x, 0.1f, cellSize.y));
            }
        }
    }


}
