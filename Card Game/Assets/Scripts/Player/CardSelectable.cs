using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        selectResponse.GetComponent<ISelectResponse>().OnSelect();
    }

    public void OnDeselect()
    {
        if (selectResponse == null) { return; }
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

    public void PickCardUp()
    {
        // Trigger animation to move from hand to hold position
        toHoldMovementTrigger.GetComponent<IMovementTrigger>().InitializeMoveObject(gameObject);
        backMovementTrigger.GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(gameObject, gameObject.transform);

        toHoldMovementTrigger.GetComponent <IMovementTrigger>().Trigger();

        gameObject.layer = LayerMask.NameToLayer(Tags.UNSELECTABLE_LAYER);
        player.hand.AssignPickedUpCard(gameObject);
        _isPicked = true;

        // Switch state of selectmanager and battle event controller
        player.selectManager.SetSelectState(SelectManager.State.CardToBoard);

        Debug.Log("Card picked up");

    }

    public void PlayCardOnBoard(ICell cell)
    {
        if (cell == null) { Debug.Log("Cell is null"); return; }
        if (board.GetCardAtCell(cell) != null) { return; }

        

        // Trigger animation to move from current position to right above the cell
        toBoardMovementTrigger.GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(gameObject, cell.worldPosition + Vector3.up);
        toBoardMovementTrigger.GetComponent<IMovementTrigger>().Trigger();

        int childIndex = cell.gridPosition.y * board.gridController.gridSize.x + cell.gridPosition.x;
        GameObject childCell = board.transform.GetChild(childIndex).gameObject;

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

    public void PutCardBack()
    {
        // Trigger animation to move from hold position back to its original position on hand

        backMovementTrigger.GetComponent<IMovementTrigger>().ReInitializeSelf();
        backMovementTrigger.GetComponent<IMovementTrigger>().Trigger();

        gameObject.layer = LayerMask.NameToLayer(Tags.SELECTABLE_LAYER);
        player.hand.AssignPickedUpCard(null);
        _isPicked = false;

        // Switch state of selectmanager and battle event controller
        player.selectManager.SetSelectState(SelectManager.State.CardInHand);

        Debug.Log("Card returned to hand");

    }

}
