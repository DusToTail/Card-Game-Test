using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: Class that handles the actions of the cards from left to right on the player's field
/// 日本語：左から右までプレイヤー陣のカードの行動をさせるクラス
/// </summary>
/// <returns></returns>
public class PlayerBattlePhase : MonoBehaviour, IPhase
{
    private PhaseController phaseController;

    private void Start()
    {
        phaseController = GetComponentInParent<PhaseController>();
    }


    public IEnumerator StartPhase()
    {
        // Prohibit player from selecting / making actions when cards are battling
        phaseController.player.selectManager.SetSelectState(SelectManager.State.Battle);

        for (int index = 0; index < phaseController.player.board.gridController.gridSize.x; index++)
        {
            ICell cell = phaseController.player.board.gridController.grid[0, index];

            yield return StartCoroutine(phaseController.player.board.CardAtCellCommitAttack(cell.gridPosition));

            // *** TO BE IMPLEMENTED ***
            // Trigger battle and abilities for each card as below:
            // Before attack of the card (ability?)
            // Attack of the card (health - damage calculation)
            // Opposite card damaged (ability?)
            // Opposite card death (trigger death animation + ability?)

            // Highlight in Scene View
            TwoDimensionGridController.HighlightCellForSeconds(cell, 0.5f);

            yield return new WaitUntil(() => !phaseController.player.board.isBusy);
        }

        // Prohibit player from selecting / making actions during enemy's turn
        phaseController.player.selectManager.SetSelectState(SelectManager.State.None);
        Debug.Log($"End Player's Turn");
    }
}
