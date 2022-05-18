using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class for handling the creation of the player deck. Draw the top card if clicked
/// 日本語：プレイヤーのデッキを管理するクラス。クリックすると、トップにあるカード返す
/// </summary>
public class PlayerDeck : MonoBehaviour, ISelectable
{
    public Stack<GameObject> cards = new Stack<GameObject>();
    public GameObject deck;
    public BattlePlayer battlePlayer;

    [SerializeField]
    private GameObject selectResponse;

    private int _curHeight = 0;

    private void Awake()
    {
        BuildDeck(deck);
    }

    private void Start()
    {
        
    }

    /// <summary>
    /// English: Clear the deck before creating a new one
    /// 日本語：新なデッキを作る前にデッキをクリアする
    /// </summary>
    public void ClearDeck()
    {
        int count = transform.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            else
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        cards.Clear();
        cards.TrimExcess();
        _curHeight = 0;
    }

    /// <summary>
    /// *** MAY NEED REIMPLEMENTATION ***
    /// English: Create a deck from the persistent deck game object with each card stacking on top of each other, and then shuffle it
    /// 日本語：固定のデッキからプレイヤーのデッキを作り、シャッフルする。
    /// </summary>
    /// <param name="deck"></param>
    public void BuildDeck(GameObject deck)
    {
        // Skip 0 because squirrel is at index 0 in the persistent deck (deck to be copied from)
        for (int i = 1; i < deck.transform.childCount; i++)
        {
            GameObject newCard = Instantiate(deck.transform.GetChild(i).gameObject);
            newCard.transform.parent = transform;
            newCard.transform.position = transform.position + Vector3.up * 0.1f * _curHeight;
            newCard.transform.rotation = Quaternion.LookRotation(-transform.forward, -transform.up);
            newCard.GetComponent<AttackCard>().InitializeStats();
            cards.Push(newCard);
            _curHeight++;
        }
        ShuffleDeck();
    }

    /// <summary>
    /// English: Shuffle the deck in random position
    /// 日本語：ランダムにシャッフルする
    /// </summary>
    public void ShuffleDeck()
    {
        GameObject[] newDeck = new GameObject[cards.Count];
        cards.ToArray().CopyTo(newDeck, 0);
        for (int i = newDeck.Length - 1; i >= 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            GameObject temp = newDeck[i];
            newDeck[i] = newDeck[rand];
            newDeck[rand] = temp;
            newDeck[i].transform.parent = null;
        }

        ClearDeck();

        int currentHeight = 0;
        for (int i = 0; i < newDeck.Length; i++)
        {
            if(newDeck[i] == null) { Debug.Log("Null"); }
            else
            {
                newDeck[i].transform.parent = transform;
                newDeck[i].transform.position = transform.position + Vector3.up * 0.1f * currentHeight;
                newDeck[i].transform.rotation = Quaternion.LookRotation(-transform.forward, -transform.up);

                cards.Push(newDeck[i]);
                currentHeight++;
            }
            
        }
    }

    /// <summary>
    /// English: Return a list of card names from the deck
    /// 日本語：デッキからカードの名前の配列を返す
    /// </summary>
    /// <returns></returns>
    public string[] GetDeckContents()
    {
        string[] contents = new string[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            contents[i] = cards.ToArray()[i].name.ToString();
        }
        return contents;
    }

    public void OnSelect()
    {
        if(selectResponse == null) { return; }
        selectResponse.GetComponent<ISelectResponse>().OnSelect();
    }

    public void OnDeselect()
    {
        if (selectResponse == null) { return; }
        selectResponse.GetComponent<ISelectResponse>().OnDeselect();

    }

    public void OnClick()
    {
        if(battlePlayer.selectManager.state == SelectManager.State.DrawFromDeck)
        {
            Debug.Log($"Clicked on {this.gameObject.name}");
            battlePlayer.DrawOneFromPlayerDeck();
        }
        
    }
}
