using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour, ISelectable
{
    public Stack<GameObject> cards = new Stack<GameObject>();
    public GameObject deck;
    public BattlePlayer battlePlayer;
    private int _curHeight = 0;

    private void Awake()
    {
        BuildDeck(deck);
    }

    private void Start()
    {
        
    }

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

    public void BuildDeck(GameObject deck)
    {
        // Skip 0 because squirrel
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
    }

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
                newDeck[i].transform.rotation = transform.rotation;

                cards.Push(newDeck[i]);
                currentHeight++;
            }
            
        }
    }

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

    }

    public void OnDeselect()
    {

    }

    public void OnClick()
    {
        Debug.Log($"Clicked on {this.gameObject.name}");
        battlePlayer.DrawOneFromPlayerDeck();
    }
}
