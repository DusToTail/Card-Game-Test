using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class that processs the selection of the bell used to signal the start of the battle phase
/// ���{��F�o�g���t�F�[�X���V�O�i������x�����I�����ꂽ���̏���������N���X
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
    /// ���{��FOnBellRinged �C�x���g���J�[������
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
