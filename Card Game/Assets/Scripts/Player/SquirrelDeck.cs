using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelDeck : MonoBehaviour, ISelectable
{
    public Stack<GameObject> cards = new Stack<GameObject>();
    public GameObject prefab;
    public BattlePlayer player;
    public int buildNum;

    [SerializeField]
    private GameObject selectResponse;

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
        if (selectResponse == null) { return; }
        selectResponse.GetComponent<ISelectResponse>().OnSelect();
    }

    public void OnDeselect()
    {
        if (selectResponse == null) { return; }
        selectResponse.GetComponent<ISelectResponse>().OnDeselect();

    }

    public void OnClick()
    {
        if(player.selectManager.state == SelectManager.State.DrawFromDeck)
        {
            Debug.Log($"Clicked on {this.gameObject.name}");
            player.DrawOneSquirrel();
        }
    }
}
