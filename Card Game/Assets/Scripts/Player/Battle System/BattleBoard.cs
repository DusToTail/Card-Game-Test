using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class for containing the grid that is used to place cards, and for handling movements on the boards
/// 日本語：カードを出し載せるグリッドを持ちながら、ボード上の動きを処理するクラス
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class BattleBoard : MonoBehaviour
{
    public IGridController gridController { get; private set; }
    public bool isBusy { get; private set; }

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
        isBusy = false;
    }

    /// <summary>
    /// English: Initialization of the boards by populating it with cells that are selectable
    /// 日本語：選択可能のセルを居住させることによるボードの初期化する
    /// </summary>
    /// <param name="player"></param>
    public void InitializeBoard(BattlePlayer player)
    {
        // Resize the board setting according to global scale of the object (*** MAY NEED TO BE REIMPLEMENTED ***)
        boardSetting.cellSize = new Vector2(transform.lossyScale.x / boardSetting.gridSize.x, transform.lossyScale.z / boardSetting.gridSize.y);

        // Clear any childs (cells gameObject) that may already exist
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

        // Create and transform childs whilst initializing them at the same time
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

        // Initialize the class that allow selection on the cells
        for (int i = count - 1; i >= 0; i--)
        {
            transform.GetChild(i).gameObject.AddComponent<CellSelectable>();
            transform.GetChild(i).GetComponent<CellSelectable>().InitializeCell(gridController.grid[i / gridController.gridSize.x, i % gridController.gridSize.x]);
        }

    }

    /// <summary>
    /// English: Command the card at grid position to attack (opposite card(s) or directly)
    /// 日本語：グリッド位置にあるカードを向こうのカードをまたは相手を直接に攻撃させる
    /// </summary>
    /// <param name="cellGridPosition"></param>
    public IEnumerator CardAtCellCommitAttack(Vector2Int cellGridPosition)
    {
        // Validation check
        ICell cell = gridController.grid[cellGridPosition.y, cellGridPosition.x];
        if (cell == null) { yield break; }
        GameObject card = GetCardAtCell(cell);
        if (card == null) { yield break; }
        if(card.GetComponent<IHaveAttack>().GetCurrentAttackDamage() == 0) { yield break; }


        // Check for cards that have health to attack 
        ICell[] oppositeCells = GetOppositeCells(cell);
        AttackCard[] healths = new AttackCard[oppositeCells.Length];
        for (int i = 0; i < oppositeCells.Length; i++)
        {
            GameObject oppositeCard = GetCardAtCell(oppositeCells[i]);
            if (oppositeCard == null && i == 0) { break; }
            if (oppositeCard == null) { break; }

            if (oppositeCard.GetComponent<AttackCard>() == null)
                healths[i] = null;
            else
                healths[i] = GetCardAtCell(oppositeCells[i]).GetComponent<AttackCard>();
        }

        // Trigger attack animation (movement) when attacking
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

            yield return new WaitUntil(() => attackTrigger.isFinished);
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

            yield return new WaitUntil(() => attackTrigger.isFinished);
        }

        // Instantly minus health of the opposing card(s) or directly
        card.GetComponent<IHaveAttack>().Attack(healths);

        isBusy = false;
    }

    /// <summary>
    /// English: Command the card at grid position to move to the specified grid position
    /// 日本語：グリッド位置にあるカードを指定されたグリッド位置に移動させる
    /// </summary>
    /// <param name="cellGridPosition"></param>
    /// <param name="moveToGridPosition"></param>
    public IEnumerator CardAtCellMoveTo(Vector2Int cellGridPosition, Vector2Int moveToGridPosition)
    {
        // Validation Check
        ICell cell = gridController.grid[cellGridPosition.y, cellGridPosition.x];
        if (cell == null) { yield break; }
        GameObject card = GetCardAtCell(cell);
        if (card == null) { yield break; }
        ICell moveToCell = gridController.grid[moveToGridPosition.y, moveToGridPosition.x];
        if (moveToCell == null) { yield break; }

        // Trigger movement (or animation)
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
                yield return new WaitUntil(() => allyMoversOnBoard.transform.GetChild(cell.gridPosition.x).GetComponent<IMovementTrigger>().isFinished);
            }
            else
            {
                // For AI / Opponent
                enemyMoversOnBoard.transform.GetChild(cell.gridPosition.x).GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(card, moveToCell.worldPosition + transform.up);
                enemyMoversOnBoard.transform.GetChild(cell.gridPosition.x).GetComponent<IMovementTrigger>().Trigger();
                card.transform.parent = childCell.transform;
                yield return new WaitUntil(() => enemyMoversOnBoard.transform.GetChild(cell.gridPosition.x).GetComponent<IMovementTrigger>().isFinished);
            }

            Debug.Log($"Moved card to {moveToCell.gridPosition}");
        }
        else
        {
            Debug.Log($"Cant move card to {moveToCell.gridPosition} due to occupied card");
        }

        isBusy = false;
    }

    /// <summary>
    /// English: Return cell from world position
    /// 日本語：ワールド位置からセルを返す
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public ICell GetCellAtCoordinateXZ(Vector3 worldPosition)
    {
        return gridController.GetCellAtCoordinateXZ(worldPosition);
    }

    /// <summary>
    /// English: Return card from cell
    /// 日本語：セルからカードを返す
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public GameObject GetCardAtCell(ICell cell)
    {
        // Validation Check
        if(cell == null) { return null; }

        int childIndex = cell.gridPosition.y * gridController.gridSize.x + cell.gridPosition.x;
        GameObject checkChild = transform.GetChild(childIndex).gameObject;
        if(checkChild.transform.childCount == 0) { return null ; }
        else { return checkChild.transform.GetChild(0).gameObject; }
    }

    /// <summary>
    /// *** MAY NEED TO BE REIMPLEMENTED (due to hard coded and the board design ***
    /// English: Return an array of opposite cells
    /// 日本語：向こうのセルの配列を返す
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private ICell[] GetOppositeCells(ICell cell)
    {
        // Validation Check
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

    

    
    private void OnDrawGizmos()
    {
        if (!displayGizmos) { return; }
        // Display an outline of the cells in yellow
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
