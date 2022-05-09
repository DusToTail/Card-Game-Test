using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public Stack<GameObject> cards;
    [SerializeField]
    private GameObject testPrefab;

    private void Awake()
    {
        cards = new Stack<GameObject>();
        int currentHeight = 0;
        for (int i = 0; i < 10; i++)
        {
            GameObject testCard = Instantiate(testPrefab);
            testCard.transform.parent = transform;
            testCard.transform.position = transform.position + Vector3.up * 0.1f * currentHeight;
            testCard.transform.rotation = transform.rotation;
            cards.Push(testCard);
            currentHeight++;
        }
    }

    private void Start()
    {
        
    }

    public void ShuffleDeck()
    {
        Debug.Log($"Deck is shuffled");

    }



    public void DisplayDeckContents()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Debug.Log($"At index {i}, {cards.ToArray()[i].GetComponent<Card>().m_cardName}");
        }
    }



}
