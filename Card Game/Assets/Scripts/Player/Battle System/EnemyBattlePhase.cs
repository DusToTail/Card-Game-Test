using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: Class that handles the actions of the cards from left to right on the opponent's field
/// 日本語：左から右まで相手陣のカードの行動をさせるクラス
/// </summary>
/// <returns></returns>
public class EnemyBattlePhase : MonoBehaviour, IPhase
{
    private PhaseController phaseController;

    private void Start()
    {
        phaseController = GetComponentInParent<PhaseController>();
    }


    public IEnumerator StartPhase()
    {
        for (int index = 0; index < phaseController.player.board.gridController.gridSize.x; index++)
        {
            ICell backCell = phaseController.player.board.gridController.grid[2, index];

            ICell frontCell = phaseController.player.board.gridController.grid[1, index];

            yield return StartCoroutine(phaseController.player.board.CardAtCellMoveTo(backCell.gridPosition, frontCell.gridPosition));
            TwoDimensionGridController.HighlightCellForSeconds(backCell, 0.5f);

            yield return StartCoroutine(phaseController.player.board.CardAtCellCommitAttack(frontCell.gridPosition));

            // *** TO BE IMPLEMENTED ***
            // Trigger battle and abilities for each card as below:
            // Before attack of the card (ability?)
            // Attack of the card (health - damage calculation)
            // Opposite card damaged (ability?)
            // Opposite card death (trigger death animation + ability?)

            // Highlight in Scene View
            TwoDimensionGridController.HighlightCellForSeconds(frontCell, 0.5f);

            yield return new WaitUntil(() => !phaseController.player.board.isBusy);
        }

        Debug.Log($"End Enemy's Turn");
    }
}
