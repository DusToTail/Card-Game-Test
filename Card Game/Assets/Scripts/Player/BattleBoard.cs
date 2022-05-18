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
    private GameObject allyMoversOnBoard;
    [SerializeField]
    private GameObject allyAttackersOnBoard;
    [SerializeField]
    private GameObject enemyMoversOnBoard;
    [SerializeField]
    private GameObject enemyAttackersOnBoard;

    [SerializeField]
    private bool displayGizmos;

    private void Awake()
    {
        gridController = new TwoDimensionGridController(transform, boardSetting.gridSize, boardSetting.cellSize);
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).GetComponent<CellSelectable>().InitializeCell(gridController.grid[i / gridController.gridSize.x, i % gridController.gridSize.x]);
        }
    }

    private void Start()
    {
        GetComponent<BoxCollider>().size = new Vector3(boardSetting.cellSize.x * (float)boardSetting.gridSize.x, 1, boardSetting.cellSize.y * (float)boardSetting.gridSize.y);

    }

    public void InitializeBoard(BattlePlayer player)
    {
        boardSetting.cellSize = new Vector2(transform.lossyScale.x / boardSetting.gridSize.x, transform.lossyScale.z / boardSetting.gridSize.y);

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
                cell.GetComponent<BoxCollider>().size = new Vector3(boardSetting.cellSize.x, 0.1f, boardSetting.cellSize.y);
                cell.transform.localPosition = Vector3.zero;
                Vector2Int centerGridPosition = new Vector2Int(Mathf.FloorToInt(boardSetting.gridSize.x / 2), Mathf.FloorToInt(boardSetting.gridSize.y / 2));
                Vector2Int dirFromCenter = new Vector2Int(x, y) - centerGridPosition;
                cell.transform.position += new Vector3(dirFromCenter.x * boardSetting.cellSize.x, 0.5f, dirFromCenter.y * boardSetting.cellSize.y);
                Debug.Log($"Cell {x}:{y} exist in {cell.transform.parent.name} and added selectable");
            }
        }

        for (int i = count - 1; i >= 0; i--)
        {
            transform.GetChild(i).gameObject.AddComponent<CellSelectable>();
            transform.GetChild(i).GetComponent<CellSelectable>().InitializeCell(gridController.grid[i / gridController.gridSize.x, i % gridController.gridSize.x]);
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

        if (cell.gridPosition.y == 0)
        {
            // For player
            // Trigger animation for attacking
            IMovementTrigger attackTrigger = allyAttackersOnBoard.transform.GetChild(cell.gridPosition.x).GetComponent<IMovementTrigger>();

            if (healths[0] == null)
            {
                // Direck Attack
                attackTrigger.InitializeMoveObjectTowards(card, oppositeCells[1].worldPosition + transform.up);
            }
            else
            {
                // Attack opposite card(s)
                attackTrigger.InitializeMoveObjectTowards(card, oppositeCells[0].worldPosition + transform.up);
            }
            attackTrigger.Trigger();
        }
        else
        {
            // For AI / Opponent
            // Trigger animation for attacking
            IMovementTrigger attackTrigger = enemyAttackersOnBoard.transform.GetChild(cell.gridPosition.x).GetComponent<IMovementTrigger>();

            if (healths[0] == null)
            {
                // Direck Attack
                attackTrigger.InitializeMoveObjectTowards(card, oppositeCells[0].worldPosition + transform.up);

            }
            else
            {
                // Attack opposite card(s)
                attackTrigger.InitializeMoveObjectTowards(card, oppositeCells[0].worldPosition + transform.up);
            }
            attackTrigger.Trigger();
        }

        // Wait until the animation of attacking is finished

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
            int childIndex = moveToCell.gridPosition.y * gridController.gridSize.x + moveToCell.gridPosition.x;
            GameObject childCell = transform.GetChild(childIndex).gameObject;

            if (cell.gridPosition.y == 0)
            {
                // For player
                allyMoversOnBoard.transform.GetChild(cell.gridPosition.x).GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(card, moveToCell.worldPosition + transform.up);
                allyMoversOnBoard.transform.GetChild(cell.gridPosition.x).GetComponent<IMovementTrigger>().Trigger();
                card.transform.parent = childCell.transform;
            }
            else
            {
                // For AI / Opponent
                enemyMoversOnBoard.transform.GetChild(cell.gridPosition.x).GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(card, moveToCell.worldPosition + transform.up);
                enemyMoversOnBoard.transform.GetChild(cell.gridPosition.x).GetComponent<IMovementTrigger>().Trigger();
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
