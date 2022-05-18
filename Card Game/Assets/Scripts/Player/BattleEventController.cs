using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class for handling the flow in each battle from the start of player turn to the end of enemy turn (a full cycle)
/// 日本語：バトル中のプレイヤーのターンから相手のターンまで（一つのサイクル）の流れを処理するクラス
/// </summary>
public class BattleEventController : MonoBehaviour
{
    public BattlePlayer player;

    public delegate void PreparationFinished();
    public static event PreparationFinished OnPreparationFinished;

    //public delegate void PlayerTurnStarted();
    //public static event PlayerTurnStarted OnPlayerTurnStarted;
    //public delegate void CardDrawn();
    //public static event CardDrawn OnCardDrawn;
    //public delegate void CardAddedToHand();
    //public static event CardAddedToHand OnCardAddedToHand;
    //public delegate void CardPicked();
    //public static event CardPicked OnCardPicked;
    //public delegate void CardPlayedToBoard();
    //public static event CardPlayedToBoard OnCardPlayedToBoard;



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

    /// <summary>
    /// English: Start the coroutine that handles the battle phase of the player after the bell
    /// 日本語：ベルの鳴らしの後のプレイヤーのバトルフェースのコルーチンを開始する
    /// </summary>
    public void StartPlayerBattlePhaseCoroutine()
    {
        StartCoroutine(PlayerBattlePhaseCoroutine());
    }

    /// <summary>
    /// English: Coroutine that handles the actions of the cards from left to right on the player's field
    /// 日本語：左から右までプレイヤー陣のカードの行動をさせるコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayerBattlePhaseCoroutine()
    {
        // Prohibit player from selecting / making actions when cards are battling
        player.selectManager.SetSelectState(SelectManager.State.Battle);

        for (int index = 0; index < player.board.gridController.gridSize.x; index++)
        {
            ICell cell = player.board.gridController.grid[0, index];

            player.board.CardAtCellCommitAttack(cell.gridPosition);

            // *** TO BE IMPLEMENTED ***
            // Trigger battle and abilities for each card as below:
            // Before attack of the card (ability?)
            // Attack of the card (health - damage calculation)
            // Opposite card damaged (ability?)
            // Opposite card death (trigger death animation + ability?)

            // Highlight in Scene View
            TwoDimensionGridController.HighlightCellForSeconds(cell, 0.5f);

            // *** TO BE REIMPLEMENTED ***
            yield return new WaitForSeconds(0.5f);
        }
        
        // Prohibit player from selecting / making actions during enemy's turn
        player.selectManager.SetSelectState(SelectManager.State.None);
        Debug.Log($"End Player's Turn");

        // Start enemy battle phase
        StartEnemyBattlePhaseCoroutine();
    }

    /// <summary>
    /// English: Start the coroutine that handles the battle phase of the opponent after the bell
    /// 日本語：ベルの鳴らしの後の相手のバトルフェースのコルーチンを開始する
    /// </summary>
    public void StartEnemyBattlePhaseCoroutine()
    {
        StartCoroutine(EnemyBattlePhaseCoroutine());
    }


    /// <summary>
    /// English: Coroutine that handles the actions of the cards from left to right on the opponent's field
    /// 日本語：左から右まで相手陣のカードの行動をさせるコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnemyBattlePhaseCoroutine()
    {
        for (int index = 0; index < player.board.gridController.gridSize.x; index++)
        {
            ICell backCell = player.board.gridController.grid[2, index];

            ICell frontCell = player.board.gridController.grid[1, index];
            
            player.board.CardAtCellMoveTo(backCell.gridPosition, frontCell.gridPosition);
            TwoDimensionGridController.HighlightCellForSeconds(backCell, 0.5f);

            // *** TO BE REIMPLEMENTED ***
            yield return new WaitForSeconds(0.5f);

            player.board.CardAtCellCommitAttack(frontCell.gridPosition);

            // *** TO BE IMPLEMENTED ***
            // Trigger battle and abilities for each card as below:
            // Before attack of the card (ability?)
            // Attack of the card (health - damage calculation)
            // Opposite card damaged (ability?)
            // Opposite card death (trigger death animation + ability?)

            // Highlight in Scene View
            TwoDimensionGridController.HighlightCellForSeconds(frontCell, 0.5f);

            // *** TO BE REIMPLEMENTED ***
            yield return new WaitForSeconds(0.5f);
        }
        
        // Start of the next round, allowing player to draw a card from deck and making other simple movement
        player.selectManager.SetSelectState(SelectManager.State.DrawFromDeck);
        Debug.Log($"End Enemy's Turn");
    }

    // NOT USED YET
    /*
    public void StartDrawPhase()
    {
        player.selectManager.SetSelectState(SelectManager.State.DrawFromDeck);
    }
    public void EndDrawPhase()
    {
        player.selectManager.SetSelectState(SelectManager.State.CardInHand);
    }
    public void PreparePhase()
    {
    }
    public void EndPhase()
    {
    }
    */

}
