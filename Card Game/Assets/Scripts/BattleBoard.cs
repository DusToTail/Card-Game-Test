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
                GameObject cell = new GameObject($"{x}:{y}");
                cell.transform.parent = transform;
                cell.gameObject.layer = LayerMask.NameToLayer(Tags.SELECTABLE_LAYER);
                cell.gameObject.tag = Tags.BATTLE_BOARD_CELL_TAG;
                cell.AddComponent<BoxCollider>();
                cell.GetComponent<BoxCollider>().size = new Vector3(m_cellSize.x, 1, m_cellSize.y);
                cell.transform.localPosition = Vector3.zero;
                Vector2Int centerGridPosition = new Vector2Int(Mathf.FloorToInt(m_gridSize.x / 2), Mathf.FloorToInt(m_gridSize.y / 2));
                Vector2Int dirFromCenter = new Vector2Int(x,y) - centerGridPosition;
                cell.transform.position += new Vector3(dirFromCenter.x * m_cellSize.x, 0, dirFromCenter.y * m_cellSize.y);
                m_grid[y, x] = new Cell(m_gridSize, cell.transform.position, m_cellSize);
            }
        }



    }


    public void PlayCardAt(GameObject _card, Vector2Int _cellPosition)
    {
        if (m_grid[_cellPosition.y, _cellPosition.x].m_card != null) { Debug.Log($"There is already a card at {_cellPosition}"); return; }
        Debug.Log($"Play {_card.GetComponent<Card>().m_cardName} at {_cellPosition}");

        _card.transform.position = m_grid[_cellPosition.y, _cellPosition.x].m_worldPosition;
        if(_cellPosition.y == 0)
        {
            _card.transform.rotation = transform.rotation;
        }
        else
        {
            _card.transform.rotation = Quaternion.Inverse(transform.rotation);
        }
        m_grid[_cellPosition.y, _cellPosition.x].InsertCard(_card.GetComponent<Card>());
    }

    public void PlayBattlePhaseOfPlayer()
    {
        for(int index = 0; index < m_gridSize.x; index++)
        {
            if(m_grid[m_curPlayerIndex, index] == null) { continue; }
            if(m_grid[m_curPlayerIndex, index].m_card == null) { continue; }
            CardAtCellCommitAttack(new Vector2Int(index, m_curPlayerIndex));
        }
        SwitchPlayerIndex();
    }

    private void CardAtCellCommitAttack(Vector2Int _cellPosition)
    {
        if(m_grid[_cellPosition.y, _cellPosition.x].m_card == null) { return; }

        m_grid[_cellPosition.y, _cellPosition.x].m_card.Attack(GetOppositeCells(m_grid[_cellPosition.y, _cellPosition.x]));
        Debug.DrawLine(m_grid[_cellPosition.y, _cellPosition.x].m_worldPosition, m_grid[_cellPosition.y, _cellPosition.x].m_worldPosition + Vector3.up, Color.green, 10);
        foreach(var cell in GetOppositeCells(m_grid[_cellPosition.y, _cellPosition.x]))
        {
            Debug.DrawLine(cell.m_worldPosition, cell.m_worldPosition + Vector3.up, Color.red, 10);
        }
    }

    private Cell[] GetOppositeCells(Cell _cell)
    {
        if(_cell.m_gridPosition.y == 0) 
        { 
            return new Cell[2]{ m_grid[1, _cell.m_gridPosition.y], m_grid[2, _cell.m_gridPosition.y] }; 
        }
        else 
        { 
            return new Cell[1] { m_grid[0, _cell.m_gridPosition.y] }; 
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
        Vector2 cellSize = new Vector2(transform.lossyScale.x / m_gridSize.x, transform.lossyScale.z / m_gridSize.y);
        for (int x = 0; x < m_gridSize.x; x++)
        {
            for (int y = 0; y < m_gridSize.y; y++)
            {
                Vector2Int centerGridPosition = new Vector2Int(Mathf.FloorToInt(m_gridSize.x / 2), Mathf.FloorToInt(m_gridSize.y / 2));
                Vector2Int dirFromCenter = new Vector2Int(x, y) - centerGridPosition;
                Vector3 cellPosition = transform.position + new Vector3(dirFromCenter.x * cellSize.x, 0, dirFromCenter.y * cellSize.y);
                if (m_grid == null)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(cellPosition, new Vector3(cellSize.x - 0.1f, 0.1f, cellSize.y - 0.1f));
                }
                else if(m_grid[y,x].m_card == null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(cellPosition, new Vector3(cellSize.x - 0.1f, 0.1f, cellSize.y - 0.1f));
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(cellPosition, new Vector3(cellSize.x - 0.1f, 0.1f, cellSize.y - 0.1f));
                }
            }
        }
    }


}
