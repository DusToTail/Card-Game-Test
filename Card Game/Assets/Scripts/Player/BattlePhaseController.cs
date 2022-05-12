using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePhaseController : IPhaseController
{
    private BattleBoard board;

    private int phaseNum;
    private int curPhase;
    private int curPlayerIndex;

    public BattlePhaseController(BattleBoard board, int phaseNum = 4, int startPlayerIndex = 0)
    {
        curPhase = 0;
        this.phaseNum = phaseNum;
        curPlayerIndex = startPlayerIndex;
        this.board = board;
        BellSelectable.OnBellRinged += NextPhase;
    }

    ~BattlePhaseController()
    {
        BellSelectable.OnBellRinged -= NextPhase;
    }


    public void DrawPhase()
    {
        // TRIGGER DRAW PHASE EVENT (turn controller)
        for (int index = 0; index < board.gridController.gridSize.x; index++)
        {
            ICell cell = board.gridController.grid[curPlayerIndex, index];
            if (cell == null) { continue; }
            GameObject card = board.GetCardAtCell(cell);
            if (card == null) { continue; }

            board.CardAtCellCommitAttack(cell.gridPosition);

        }

        // Draw cards for current player

        // Stop and wait for current player to draw a card
    }

    public void PreparePhase()
    {
        // TRIGGER PREPARE PHASE EVENT (after Draw Phase

        // Stop and wait for current player to ring the bell
    }

    public void BattlePhase()
    {
        // TRIGGER BATTLE PHASE EVENT (bell)
        for (int index = 0; index < board.gridController.gridSize.x; index++)
        {
            ICell cell = board.gridController.grid[curPlayerIndex, index];
            if (cell == null) { continue; }
            GameObject card = board.GetCardAtCell(cell);
            if (card == null) { continue; }

            board.CardAtCellCommitAttack(cell.gridPosition);

        }
        // Trigger battle and abilities for each card as below:
        // Before attack of the card (ability?)
        // Attack of the card (health - damage calculation)
        // Opposite card damaged (ability?)
        // Opposite card death (trigger death animation + ability?)
        // Repeat

    }
    public void EndPhase()
    {
        // TRIGGER END PHASE EVENT (right after end phase)

        // Trigger abilities for each card (if applicable):
    }
    
    public void ExecuteCurrentPhase()
    {
        switch(curPhase)
        {
            case 0:
                DrawPhase();
                break;
            case 1:
                PreparePhase();
                break;
            case 2:
                BattlePhase();
                break ;
            case 3:
                EndPhase();
                break;

            default:
                break; 
        }
    }
    
    public void NextPhase()
    {
        curPhase = (curPhase + 1) % phaseNum;
        ExecuteCurrentPhase();
    }
}
