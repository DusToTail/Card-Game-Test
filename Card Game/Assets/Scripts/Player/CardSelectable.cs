using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectable : MonoBehaviour, ISelectable
{
    [SerializeField]
    private ISelectResponse selectResponse;
    [SerializeField]
    private Transform defaultPosition;
    [SerializeField]
    private Transform holdUpPosition;
    [SerializeField]
    private BattleBoard board;
    [SerializeField]
    private BattlePlayer player;

    private bool isPicked = false;

    public void OnSelect()
    {
        if(selectResponse == null) { return; }
        selectResponse.OnSelect();
    }

    public void OnDeselect()
    {
        if (selectResponse == null) { return; }
        selectResponse.OnDeselect();
    }

    public void OnClick()
    {
        if(isPicked)
        {
            //PlayCardOnBoard(cell);
        }
        else
        {
            PickCardUp();
        }
    }

    public void PickCardUp()
    {
        gameObject.transform.parent = holdUpPosition;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer(Tags.UNSELECTABLE_LAYER);
        player.hand.AssignPickedUpCard(gameObject);
        isPicked = true;
        Debug.Log("Card picked up");
    }

    public void PlayCardOnBoard(ICell cell)
    {
        board.PlayCardAtCell(gameObject, cell);
        player.hand.AssignPickedUpCard(null);
        isPicked = false;
        Debug.Log("Card played on board");

    }

    public void PutCardBack()
    {
        gameObject.transform.parent = defaultPosition;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer(Tags.SELECTABLE_LAYER);
        player.hand.AssignPickedUpCard(null);
        isPicked = false;
        Debug.Log("Card returned to default (hand)");

    }

}
