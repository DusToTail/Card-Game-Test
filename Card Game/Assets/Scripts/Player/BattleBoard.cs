using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BattleBoard : MonoBehaviour
{
    public IGridController gridController { get; private set; }

    [SerializeField]
    private BoardScriptableObject boardSetting;
    [SerializeField]
    private CardScriptableObject cardSetting;
    [SerializeField]
    private GameObject cardMoverOnBoard;

    [SerializeField]
    private bool displayGizmos;

    private void Awake()
    {

    }

    private void Start()
    {
        GetComponent<BoxCollider>().size = new Vector3(boardSetting.cellSize.x * (float)boardSetting.gridSize.x, 1, boardSetting.cellSize.y * (float)boardSetting.gridSize.y);

    }

    public void InitializeBoard(BattlePlayer player)
    {
        boardSetting.cellSize = new Vector2(transform.lossyScale.x / boardSetting.gridSize.x, transform.lossyScale.z / boardSetting.gridSize.y);
        gridController = new TwoDimensionGridController(transform, boardSetting.gridSize, boardSetting.cellSize);

        // Clear Childs
        int count = transform.childCount;
        for(int i = count - 1; i >= 0; i--)
        {
            if(Application.isPlaying)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            else
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        // Initialize Childs
        for (int y = 0; y < boardSetting.gridSize.y; y++)
        {
            for (int x = 0; x < boardSetting.gridSize.x; x++)
            {
                // Instantiate Prefab for interaction with only collider and visualization when hover over
                GameObject cell = new GameObject($"{x}:{y}");
                cell.transform.parent = transform;
                cell.gameObject.layer = LayerMask.NameToLayer(Tags.SELECTABLE_LAYER);
                cell.gameObject.tag = Tags.BATTLE_BOARD_CELL_TAG;
                cell.AddComponent<BoxCollider>();
                cell.AddComponent<CellSelectable>();
                cell.GetComponent<CellSelectable>().InitializeCell(gridController.grid[y, x]);
                cell.GetComponent<CellSelectable>().player = player;
                cell.GetComponent<BoxCollider>().size = new Vector3(boardSetting.cellSize.x, 1, boardSetting.cellSize.y);
                cell.transform.localPosition = Vector3.zero;
                Vector2Int centerGridPosition = new Vector2Int(Mathf.FloorToInt(boardSetting.gridSize.x / 2), Mathf.FloorToInt(boardSetting.gridSize.y / 2));
                Vector2Int dirFromCenter = new Vector2Int(x, y) - centerGridPosition;
                cell.transform.position += new Vector3(dirFromCenter.x * boardSetting.cellSize.x, 0, dirFromCenter.y * boardSetting.cellSize.y);
            }
        }

    }

    public ICell GetCellAtCoordinateXZ(Vector3 worldPosition)
    {
        return gridController.GetCellAtCoordinateXZ(worldPosition);
    }


    public GameObject GetCardAtCell(ICell cell)
    {
        if(cell == null) { return null; }
        int index = cell.gridPosition.y * gridController.gridSize.x + cell.gridPosition.x;
        GameObject checkChild = transform.GetChild(index).gameObject;
        if(checkChild.transform.childCount == 0) { return null ; }
        else { return checkChild.transform.GetChild(0).gameObject; }
    }

    private ICell[] GetOppositeCells(ICell cell)
    {
        if(cell == null) { return null;}

        List<ICell> oppositeCells = new List<ICell>();
        if(cell.gridPosition.y == 0)
        {
            oppositeCells.Add(gridController.grid[1, cell.gridPosition.x]);
            oppositeCells.Add(gridController.grid[2, cell.gridPosition.x]);
        }
        else
        {
            oppositeCells.Add(gridController.grid[0, cell.gridPosition.x]);
        }
        return oppositeCells.ToArray();
    }

    public void CardAtCellCommitAttack(Vector2Int cellGridPosition)
    {
        ICell cell = gridController.grid[cellGridPosition.y, cellGridPosition.x];
        if (cell == null) { return; }
        GameObject card = GetCardAtCell(cell);
        if (card == null) { return; }

        ICell[] oppositeCells = GetOppositeCells(cell);
        AttackCard[] healths = new AttackCard[oppositeCells.Length];
        for(int i = 0; i < oppositeCells.Length; i++)
        {
            GameObject oppositeCard = GetCardAtCell(oppositeCells[i]);
            if(oppositeCard == null && i == 0) { break; }
            if(oppositeCard == null) { break; }

            if (oppositeCard.GetComponent<AttackCard>() == null)
                healths[i] = null;
            else
                healths[i] = GetCardAtCell(oppositeCells[i]).GetComponent<AttackCard>();
        }
        card.GetComponent<IHaveAttack>().Attack(healths);
    }

    public void CardAtCellMoveTo(Vector2Int cellGridPosition, Vector2Int moveToGridPosition)
    {
        ICell cell = gridController.grid[cellGridPosition.y, cellGridPosition.x];
        if (cell == null) { return; }
        GameObject card = GetCardAtCell(cell);
        if (card == null) { return; }

        ICell moveToCell = gridController.grid[moveToGridPosition.y, moveToGridPosition.x];
        if (moveToCell == null) { return; }
        GameObject cardAtMoveToCell = GetCardAtCell(moveToCell);
        if (cardAtMoveToCell == null)
        {
            cardMoverOnBoard.GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(card, moveToCell.worldPosition + transform.up);
            cardMoverOnBoard.GetComponent<IMovementTrigger>().Trigger();

            int childIndex = moveToCell.gridPosition.y * gridController.gridSize.x + moveToCell.gridPosition.x;
            GameObject childCell = transform.GetChild(childIndex).gameObject;

            if (cell.gridPosition.y == 0)
            {
                // For player
                card.transform.rotation = transform.rotation;
                card.transform.parent = childCell.transform;
            }
            else
            {
                // For AI / Opponent
                card.transform.rotation = Quaternion.LookRotation(-transform.forward, transform.up);
                card.transform.parent = childCell.transform;
            }

            Debug.Log($"Moved card to {moveToCell.gridPosition}");
            return; 
        }
        else
        {
            Debug.Log($"Cant move card to {moveToCell.gridPosition} due to occupied card");
            return;
        }


    }

    /*

    public void PlayBattlePhaseOfPlayer()
    {
        for(int index = 0; index < boardSetting.gridSize.x; index++)
        {
            if(grid[_curPlayerIndex, index] == null) { continue; }
            if(grid[_curPlayerIndex, index].card == null) { continue; }
            CardAtCellCommitAttack(new Vector2Int(index, _curPlayerIndex));
        }
        SwitchPlayerIndex();
    }

    private void CardAtCellCommitAttack(Vector2Int _cellPosition)
    {
        if(grid[_cellPosition.y, _cellPosition.x].card == null) { return; }

        grid[_cellPosition.y, _cellPosition.x].card.Attack(GetOppositeCells(grid[_cellPosition.y, _cellPosition.x]));
        Debug.DrawLine(grid[_cellPosition.y, _cellPosition.x].worldPosition, grid[_cellPosition.y, _cellPosition.x].worldPosition + Vector3.up, Color.green, 10);
        foreach(var cell in GetOppositeCells(grid[_cellPosition.y, _cellPosition.x]))
        {
            Debug.DrawLine(cell.worldPosition, cell.worldPosition + Vector3.up, Color.red, 10);
        }
    }

    private Cell[] GetOppositeCells(Cell _cell)
    {
        if(_cell.gridPosition.y == 0) 
        { 
            return new Cell[2]{ grid[1, _cell.gridPosition.y], grid[2, _cell.gridPosition.y] }; 
        }
        else 
        { 
            return new Cell[1] { grid[0, _cell.gridPosition.y] }; 
        }
    }

    private void SwitchPlayerIndex()
    {
        if (_curPlayerIndex == 0) { _curPlayerIndex = 1; }
        else { _curPlayerIndex = 0; }
    }
    */
    private void OnDrawGizmos()
    {
        if (!displayGizmos) { return; }
        Vector2 cellSize = new Vector2(transform.lossyScale.x / boardSetting.gridSize.x, transform.lossyScale.z / boardSetting.gridSize.y);
        for (int x = 0; x < boardSetting.gridSize.x; x++)
        {
            for (int y = 0; y < boardSetting.gridSize.y; y++)
            {
                Vector2Int centerGridPosition = new Vector2Int(Mathf.FloorToInt(boardSetting.gridSize.x / 2), Mathf.FloorToInt(boardSetting.gridSize.y / 2));
                Vector2Int dirFromCenter = new Vector2Int(x, y) - centerGridPosition;
                Vector3 cellPosition = transform.position + new Vector3(dirFromCenter.x * cellSize.x, 0, dirFromCenter.y * cellSize.y);
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(cellPosition, new Vector3(cellSize.x - 0.1f, 0.1f, cellSize.y - 0.1f));
            }
        }
    }


}
