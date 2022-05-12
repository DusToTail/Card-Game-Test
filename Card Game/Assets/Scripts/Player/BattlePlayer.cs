using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    public string playerName;
    public int playerIndex;

    public PlayerDeck playerDeck;
    public SquirrelDeck squirrelDeck;
    public PlayerHand hand;
    public BattleBoard board;

    public enum State
    {
        Draw,
        Prepare,
        Battle,
        End
    }

    private State _state;

    private SelectManager _selectManager;

    private void Start()
    {
        _selectManager = new SelectManager();
        InjectDependency();
    }


    private void Update()
    {
        if (!gameObject.activeSelf) { return; }
        // Hover
        _selectManager.ProcessPassiveSelection();

        if (Input.GetMouseButtonUp(0))
        {
            // Click
            _selectManager.ProcessActiveSelection();
        }

    }

    public void DrawInitialHand()
    {
        DrawOneSquirrel();
        for (int i = 0; i < hand.initialDrawNumber - 1; i++)
        {
            DrawOneFromPlayerDeck();
        }
    }

    public void DrawOneFromPlayerDeck()
    {
        if (playerDeck.cards.Count == 0) { return; }
        Debug.Log("Draw one from player deck");
        GameObject card = playerDeck.cards.Pop();
        hand.AddCardToHand(card);
    }

    public void DrawOneSquirrel()
    {
        if (squirrelDeck.cards.Count == 0) { return; }
        Debug.Log("Draw one from squirrel deck");
        GameObject card = squirrelDeck.cards.Pop();
        hand.AddCardToHand(card);
    }

    public void InjectDependency()
    {
        playerDeck.battlePlayer = this;
        squirrelDeck.battlePlayer = this;
        board.InitializeBoard(this);
    }


}
