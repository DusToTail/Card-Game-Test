using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSelectable : MonoBehaviour, ISelectable
{
    public ICell cell { get; private set; }
    public BattlePlayer player;
    [SerializeField]
    private ISelectResponse selectResponse;

    public void InitializeCell(ICell cell)
    {
        this.cell = cell;
        player = GameObject.FindObjectOfType<BattlePlayer>();
    }

    public void OnSelect()
    {
        if (selectResponse == null) { return; }
        selectResponse.OnSelect();
    }

    public void OnDeselect()
    {
        if (selectResponse == null) { return; }
        selectResponse.OnDeselect();
    }

    public void OnClick()
    {
        if(player == null) { return; }
        if(player.hand == null) { return; }
        if(player.hand.pickedCard == null) { return;}

        if(player.selectManager.state == SelectManager.State.CardToBoard)
        {
            player.hand.pickedCard.GetComponent<CardSelectable>().PlayCardOnBoard(cell);
        }

    }




}
