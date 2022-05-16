using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellSelectable : MonoBehaviour, ISelectable
{
    [SerializeField]
    private GameObject selectResponse;
    [SerializeField]
    private BattlePlayer player;

    public delegate void BellRinged();
    public static event BellRinged OnBellRinged;

    public void RingBell()
    {
        if (OnBellRinged != null)
        {
            Debug.Log("Bell Ringed");
            OnBellRinged();
        }
    }

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
        if(player.selectManager.state == SelectManager.State.CardInHand || player.selectManager.state == SelectManager.State.CardToBoard)
        {
            RingBell();
        }
    }
}
