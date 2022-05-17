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
    public BattleEventController battleController;
    
    public SelectManager selectManager { get; private set; }

    private void OnEnable()
    {
        BattleEventController.OnPreparationFinished += AllowSelection;
    }

    private void OnDisable()
    {
        BattleEventController.OnPreparationFinished -= AllowSelection;

    }

    private void Awake()
    {
        selectManager = new SelectManager();
        InjectDependency();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!gameObject.activeSelf) { return; }
        // Hover
        selectManager.ProcessPassiveSelection();

        if (Input.GetMouseButtonUp(0))
        {
            // Click
            selectManager.ProcessActiveSelection();
        }

    }

    public void AllowSelection()
    {
        selectManager.SetSelectState(SelectManager.State.DrawFromDeck);
    }

    public void DrawInitialHand()
    {
        DrawOneSquirrel();
        for (int i = 0; i < hand.initialDrawNumber - 1; i++)
        {
            DrawOneFromPlayerDeck();
        }
        selectManager.SetSelectState(SelectManager.State.CardInHand);
    }

    public void DrawOneFromPlayerDeck()
    {
        if (playerDeck.cards.Count == 0) { return; }
        Debug.Log("Draw one from player deck");
        GameObject card = playerDeck.cards.Pop();
        hand.AddCardToHand(card);
        selectManager.SetSelectState(SelectManager.State.CardInHand);

    }

    public void DrawOneSquirrel()
    {
        if (squirrelDeck.cards.Count == 0) { return; }
        Debug.Log("Draw one from squirrel deck");
        GameObject card = squirrelDeck.cards.Pop();
        hand.AddCardToHand(card);
        selectManager.SetSelectState(SelectManager.State.CardInHand);

    }

    public void InjectDependency()
    {
        playerDeck.battlePlayer = this;
        squirrelDeck.player = this;
    }

}
