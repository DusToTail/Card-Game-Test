using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class that process that selection for cards
/// 日本語：カード用の選択を処理するクラス
/// </summary>
public class CardSelectable : MonoBehaviour, ISelectable
{
    [SerializeField]
    private GameObject selectResponse;
    [SerializeField]
    private GameObject toBoardMovementTrigger;
    [SerializeField]
    private GameObject toHoldMovementTrigger;
    [SerializeField]
    private GameObject backMovementTrigger;
    [SerializeField]
    private BattleBoard board;
    [SerializeField]
    private BattlePlayer player;

    private bool _isPicked = false;

    public void OnSelect()
    {
        if(selectResponse == null) { return; }
        if(player.selectManager.state == SelectManager.State.CardInHand)
        {
            selectResponse.GetComponent<ISelectResponse>().OnSelect();
        }
    }

    public void OnDeselect()
    {
        if (selectResponse == null) { return; }
        if (player.selectManager.state == SelectManager.State.CardInHand)
            selectResponse.GetComponent<ISelectResponse>().OnDeselect();
    }

    public void OnClick()
    {
        if(player.selectManager.state == SelectManager.State.CardInHand)
        {
            if(!_isPicked)
            {
                PickCardUp();
            }
        }
    }

    /// <summary>
    /// English: Prevent the player from selecting this card and trigger the movement of the card to hold position
    /// 日本語：このカードを選択することを不可にし、ホルド位置に移動させる
    /// </summary>
    public void PickCardUp()
    {
        // Trigger animation to move from hand to hold position
        toHoldMovementTrigger.GetComponent<IMovementTrigger>().InitializeMoveObject(gameObject);
        // Initialize the back movement trigger before hand (not used yet)
        backMovementTrigger.GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(gameObject, gameObject.transform);

        toHoldMovementTrigger.GetComponent <IMovementTrigger>().Trigger();

        gameObject.layer = LayerMask.NameToLayer(Tags.UNSELECTABLE_LAYER);
        player.hand.AssignPickedUpCard(gameObject);
        _isPicked = true;

        // Switch state of select manager to limit player's selection
        player.selectManager.SetSelectState(SelectManager.State.CardToBoard);

        // *** TO BE IMPLEMENTED ***
        // Trigger Camera Movement

        Debug.Log($"{gameObject.name} picked up");

    }

    /// <summary>
    /// English: Play this card and move it to the board at cell position
    /// 日本語：このカードを出し、ボード上にあるセルに移動させる
    /// </summary>
    /// <param name="cell"></param>
    public void PlayCardOnBoard(ICell cell)
    {
        // Validation Check
        if (cell == null) { Debug.Log("Cell is null"); return; }
        if (board.GetCardAtCell(cell) != null) { return; }

        

        // Trigger animation to move from current position to right above the cell
        toBoardMovementTrigger.GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(gameObject, cell.worldPosition + board.transform.up);
        toBoardMovementTrigger.GetComponent<IMovementTrigger>().Trigger();

        int childIndex = cell.gridPosition.y * board.gridController.gridSize.x + cell.gridPosition.x;
        GameObject childCell = board.transform.GetChild(childIndex).gameObject;

        // *** NEED REIMPLEMENTATION (due to being overwritten by toBoardMovementTrigger) ***
        // Rotate the card to the correct rotation and put it the correct cell transform
        if (cell.gridPosition.y == 0)
        {
            // For player
            transform.rotation = board.transform.rotation;
            transform.parent = childCell.transform;
        }
        else
        {
            // For AI / Opponent
            transform.rotation = Quaternion.LookRotation(- board.transform.forward, board.transform.up);
            transform.parent = childCell.transform;
        }

        gameObject.layer = LayerMask.NameToLayer(Tags.UNSELECTABLE_LAYER);

        player.hand.AssignPickedUpCard(null);
        _isPicked = false;

        player.hand.cards.Remove(gameObject);
        // Switch state of selectmanager and battle event controller
        player.selectManager.SetSelectState(SelectManager.State.CardInHand);

        Debug.Log("Card played on board");

    }

    /// <summary>
    /// *** NOT USED YET ***
    /// English: Put the card back to its original position (mover initialized when picked up)
    /// 日本語：カードを前の位置に戻す（ピックアップされた時に初期化された）
    /// </summary>
    public void PutCardBack()
    {
        // Trigger animation to move from hold position back to its original position on hand
        backMovementTrigger.GetComponent<IMovementTrigger>().ReInitializeSelf();　// Due to the object has already moved to a new position
        backMovementTrigger.GetComponent<IMovementTrigger>().Trigger();

        gameObject.layer = LayerMask.NameToLayer(Tags.SELECTABLE_LAYER);
        player.hand.AssignPickedUpCard(null);
        _isPicked = false;

        // Switch state of selectmanager and battle event controller
        player.selectManager.SetSelectState(SelectManager.State.CardInHand);

        Debug.Log($"{gameObject.name} returned to hand");

    }

    /// <summary>
    /// English: Set the object select response
    /// 日本語：カードの選択反応を設定する
    /// </summary>
    /// <param name="selectResponse"></param>
    public void SetSelectResponse(GameObject selectResponse)
    {
        this.selectResponse = selectResponse;
    }

}
