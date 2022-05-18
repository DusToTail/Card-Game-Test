using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class that centralize the battle systems and handles player's input
/// ���{��F�o�g���V�X�e���𓝊����A�v���C���[�̃C���v�b�g����������N���X
/// </summary>
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

    /// <summary>
    /// *** MAY NEED REIMPLEMENTATION or RENAME ***
    /// English: Allow draw from deck
    /// ���{��F�f�b�L����h���[���邱�Ƃ��\�ɂ���
    /// </summary>
    public void AllowSelection()
    {
        selectManager.SetSelectState(SelectManager.State.DrawFromDeck);
    }

    /// <summary>
    /// *** NEED REIMPLEMENTATION (due to positions of cards getting constantly overwritten as start position for movers before reaching the destination)
    /// English: Draw the intial hand when the battle starts
    /// ���{��F�o�g���J�n�ɏ��߂̎���h���[����
    /// </summary>
    public void DrawInitialHand()
    {
        DrawOneSquirrel();
        for (int i = 0; i < hand.initialDrawNumber - 1; i++)
        {
            DrawOneFromPlayerDeck();
        }
        selectManager.SetSelectState(SelectManager.State.CardInHand);
    }

    /// <summary>
    /// English: Draw one card from player deck
    /// ���{��F�v���C���[�f�b�L����ꖇ������
    /// </summary>
    public void DrawOneFromPlayerDeck()
    {
        if (playerDeck.cards.Count == 0) { return; }
        Debug.Log("Draw one from player deck");
        GameObject card = playerDeck.cards.Pop();
        hand.AddCardToHand(card);
        selectManager.SetSelectState(SelectManager.State.CardInHand);

    }

    /// <summary>
    /// English: Draw one card from squirrel deck
    /// ���{��F���X�f�b�L����ꖇ������
    /// </summary>
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
