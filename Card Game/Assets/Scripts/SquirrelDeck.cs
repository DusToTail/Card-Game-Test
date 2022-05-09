using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelDeck : MonoBehaviour
{
    public Stack<GameObject> cards;
    [SerializeField]
    private GameObject squirrelPrefab;
    [SerializeField]
    private int squirrelCount;

    private void Awake()
    {
        cards = new Stack<GameObject>();
        int currentHeight = 0;
        for (int i = 0; i < squirrelCount; i++)
        {
            GameObject newSquirrel = Instantiate(squirrelPrefab);
            newSquirrel.transform.parent = transform;
            newSquirrel.transform.position = transform.position + Vector3.up * 0.1f * currentHeight;
            newSquirrel.transform.rotation = transform.rotation;
            cards.Push(newSquirrel);
            currentHeight++;
        }
    }

    private void Start()
    {
        
    }



}
