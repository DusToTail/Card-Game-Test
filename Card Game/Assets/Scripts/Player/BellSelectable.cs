using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class that processs the selection of the bell used to signal the start of the battle phase
/// 日本語：バトルフェースをシグナルするベルが選択された時の処理をするクラス
/// </summary>
public class BellSelectable : MonoBehaviour, ISelectable
{
    [SerializeField]
    private GameObject selectResponse;
    [SerializeField]
    private BattlePlayer player;

    public delegate void BellRinged();
    public static event BellRinged OnBellRinged;

    /// <summary>
    /// English: Call the OnBellRinged event
    /// 日本語：OnBellRinged イベントをカールする
    /// </summary>
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
        if (player.selectManager.state == SelectManager.State.CardInHand)
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
            RingBell();
        }
    }
}
