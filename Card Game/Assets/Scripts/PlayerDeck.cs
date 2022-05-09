using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public Stack<GameObject> cards = new Stack<GameObject>();
    [SerializeField]
    private GameObject testPrefab;

    private void Start()
    {
        int currentHeight = 0; 
        for (int i = 0; i < 10; i++)
        {
            GameObject newSquirrel = Instantiate(testPrefab);
            newSquirrel.transform.parent = transform;
            newSquirrel.transform.position = transform.position + Vector3.up * 0.1f * currentHeight;
            newSquirrel.transform.rotation = transform.rotation;
            cards.Push(newSquirrel);
            currentHeight++;
        }
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
