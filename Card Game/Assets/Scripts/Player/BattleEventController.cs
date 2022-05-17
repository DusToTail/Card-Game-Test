using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventController : MonoBehaviour
{
    public BattlePlayer player;

    public delegate void PreparationFinished();
    public static event PreparationFinished OnPreparationFinished;

    public delegate void PlayerTurnStarted();
    public static event PlayerTurnStarted OnPlayerTurnStarted;
    public delegate void CardDrawn();
    public static event CardDrawn OnCardDrawn;
    public delegate void CardAddedToHand();
    public static event CardAddedToHand OnCardAddedToHand;
    public delegate void CardPicked();
    public static event CardPicked OnCardPicked;
    public delegate void CardPlayedToBoard();
    public static event CardPlayedToBoard OnCardPlayedToBoard;



    private void OnEnable()
    {
        BellSelectable.OnBellRinged += StartPlayerBattlePhaseCoroutine;
    }

    private void OnDisable()
    {
        BellSelectable.OnBellRinged -= StartPlayerBattlePhaseCoroutine;
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

    public void StartPlayerBattlePhaseCoroutine()
    {
        StartCoroutine(PlayerBattlePhaseCoroutine());
    }

    public IEnumerator PlayerBattlePhaseCoroutine()
    {
        // TRIGGER BATTLE PHASE EVENT (bell)

        player.selectManager.SetSelectState(SelectManager.State.Battle);

        // Attack
        for (int index = 0; index < player.board.gridController.gridSize.x; index++)
        {
            ICell cell = player.board.gridController.grid[0, index];

            player.board.CardAtCellCommitAttack(cell.gridPosition);

            TwoDimensionGridController.HighlightCellForSeconds(cell, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
        // Trigger battle and abilities for each card as below:
        // Before attack of the card (ability?)
        // Attack of the card (health - damage calculation)
        // Opposite card damaged (ability?)
        // Opposite card death (trigger death animation + ability?)
        // Repeat

        player.selectManager.SetSelectState(SelectManager.State.None);
        Debug.Log($"End Player's Turn");

        StartEnemyBattlePhaseCoroutine();
    }

    public void StartEnemyBattlePhaseCoroutine()
    {
        StartCoroutine(EnemyBattlePhaseCoroutine());
    }

    public IEnumerator EnemyBattlePhaseCoroutine()
    {
        // Attack
        for (int index = 0; index < player.board.gridController.gridSize.x; index++)
        {
            ICell backCell = player.board.gridController.grid[2, index];

            ICell frontCell = player.board.gridController.grid[1, index];
            
            player.board.CardAtCellMoveTo(backCell.gridPosition, frontCell.gridPosition);
            TwoDimensionGridController.HighlightCellForSeconds(backCell, 0.5f);
            yield return new WaitForSeconds(0.5f);

            player.board.CardAtCellCommitAttack(frontCell.gridPosition);

            TwoDimensionGridController.HighlightCellForSeconds(frontCell, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
        // Trigger battle and abilities for each card as below:
        // Before attack of the card (ability?)
        // Attack of the card (health - damage calculation)
        // Opposite card damaged (ability?)
        // Opposite card death (trigger death animation + ability?)
        // Repeat

        player.selectManager.SetSelectState(SelectManager.State.DrawFromDeck);
        Debug.Log($"End Enemy's Turn");
    }



    public void StartDrawPhase()
    {
        // TRIGGER DRAW PHASE EVENT (turn controller, after the enemy turn finished attacking)
        player.selectManager.SetSelectState(SelectManager.State.DrawFromDeck);
        // Draw cards for current player

        // Stop and wait for current player to draw a card
    }

    public void EndDrawPhase()
    {
        player.selectManager.SetSelectState(SelectManager.State.CardInHand);

    }

    public void PreparePhase()
    {
        // TRIGGER PREPARE PHASE EVENT (after Draw Phase

        // Stop and wait for current player to ring the bell
    }

    
    public void EndPhase()
    {
        // TRIGGER END PHASE EVENT (right after end phase)

        // Trigger abilities for each card (if applicable):
    }
    

}
