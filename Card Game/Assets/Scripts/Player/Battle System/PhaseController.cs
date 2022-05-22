using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class for handling the flow in each battle from the start of player turn to the end of enemy turn (a full cycle)
/// ���{��F�o�g�����̃v���C���[�̃^�[�����瑊��̃^�[���܂Łi��̃T�C�N���j�̗������������N���X
/// </summary>
public class PhaseController : MonoBehaviour
{
    public BattlePlayer player;

    public delegate void PreparationFinished();
    public static event PreparationFinished OnPreparationFinished;

    private void OnEnable()
    {
        BellSelectable.OnBellRinged += StartBattleCoroutine;
    }

    private void OnDisable()
    {
        BellSelectable.OnBellRinged -= StartBattleCoroutine;
    }

    private void Start()
    {
        PrepareBattleField();
    }

    public void PrepareBattleField()
    {
        // Preparation 





        Debug.Log("Preparation for Battle Field is finished");
        OnPreparationFinished();
    }

    /// <summary>
    /// English: Start the coroutine that handles the battle phase after the bell
    /// ���{��F�x���̖炵�̌�̃o�g���t�F�[�X�̃R���[�`�����J�n����
    /// </summary>
    public void StartBattleCoroutine()
    {
        Debug.Log("Start Battle Phase");
        StartCoroutine(BattleCoroutine());
    }

    /// <summary>
    /// English: The coroutine that handles the battle phase after the bell
    /// ���{��F�x���̖炵�̌�̃o�g���t�F�[�X�̃R���[�`��
    /// </summary>
    private IEnumerator BattleCoroutine()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            yield return StartCoroutine(transform.GetChild(i).gameObject.GetComponent<IPhase>().StartPhase());
        }

        Debug.Log("End Battle Phase");
        // Start of the next round, allowing player to draw a card from deck and making other simple movement
        player.selectManager.SetSelectState(SelectManager.State.DrawFromDeck);
    }



}
