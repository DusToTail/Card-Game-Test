using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelDeck : MonoBehaviour, ISelectable
{
    public Stack<GameObject> cards = new Stack<GameObject>();
    public GameObject prefab;
    public BattlePlayer battlePlayer;
    public int buildNum;

    private int _curHeight = 0;

    private void Awake()
    {
    }

    private void Start()
    {
        BuildDeck(buildNum);

    }

    public void ClearDeck()
    {
        int count = transform.childCount;
        for (int i = count - 1; i >= 0 ; i--)
        {
            if(Application.isPlaying)
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

    public void BuildDeck(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newSquirrel = Instantiate(prefab);
            newSquirrel.transform.parent = transform;
            newSquirrel.transform.position = transform.position + Vector3.up * 0.1f * _curHeight;
            newSquirrel.transform.rotation = Quaternion.LookRotation(-transform.forward, -transform.up);
            newSquirrel.GetComponent<AttackCard>().InitializeStats();
            cards.Push(newSquirrel);
            _curHeight++;
        }
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
        battlePlayer.DrawOneSquirrel();
    }
}
