using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class that centralize the battle systems and handles player's input
/// 日本語：バトルシステムを統括し、プレイヤーのインプットを処理するクラス
/// </summary>
public class BattlePlayer : MonoBehaviour
{
    public string playerName;
    public int playerIndex;

    public PlayerDeck playerDeck;
    public SquirrelDeck squirrelDeck;
    public PlayerHand hand;
    public BattleBoard board;
    public PhaseController battleController;
    
    public SelectManager selectManager { get; private set; }

    private void OnEnable()
    {
        PhaseController.OnPreparationFinished += StartWithInitialHand;
    }

    private void OnDisable()
    {
        PhaseController.OnPreparationFinished -= StartWithInitialHand;

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

    /// <summary>
    /// English: Start the game with the initial hand
    /// 日本語：デッキからドローすることを可能にする
    /// </summary>
    public void StartWithInitialHand()
    {
        StartCoroutine(DrawInitialHand());
    }


    /// <summary>
    /// English: Draw the intial hand when the battle starts
    /// 日本語：バトル開始に初めの手をドローする
    /// </summary>
    public IEnumerator DrawInitialHand()
    {
        yield return StartCoroutine(DrawOneSquirrel(true));
        for (int i = 0; i < hand.initialDrawNumber - 1; i++)
        {
            yield return StartCoroutine(DrawOneFromPlayerDeck(true));
        }
        selectManager.SetSelectState(SelectManager.State.CardInHand);
    }

    /// <summary>
    /// English: Draw one card from player deck
    /// 日本語：プレイヤーデッキから一枚を引く
    /// </summary>
    public IEnumerator DrawOneFromPlayerDeck(bool isInitialHand)
    {
        if (playerDeck.cards.Count == 0) { yield break; }
        Debug.Log("Draw one from player deck");
        GameObject card = playerDeck.cards.Pop();
        selectManager.SetSelectState(SelectManager.State.None);
        yield return StartCoroutine(hand.AddCardToHand(card));
        if (!isInitialHand)
            selectManager.SetSelectState(SelectManager.State.CardInHand);

    }

    /// <summary>
    /// English: Draw one card from squirrel deck
    /// 日本語：リスデッキから一枚を引く
    /// </summary>
    public IEnumerator DrawOneSquirrel(bool isInitialHand)
    {
        if (squirrelDeck.cards.Count == 0) { yield break; }
        Debug.Log("Draw one from squirrel deck");
        GameObject card = squirrelDeck.cards.Pop();
        selectManager.SetSelectState(SelectManager.State.None);
        yield return StartCoroutine(hand.AddCardToHand(card));
        if(!isInitialHand)
            selectManager.SetSelectState(SelectManager.State.CardInHand);

    }

    public void InjectDependency()
    {
        playerDeck.player = this;
        squirrelDeck.player = this;
    }

}
