using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class for handling the creation of the squirrel deck. Draw a squirrel if clicked
/// ���{��F���X�f�b�L���Ǘ�����N���X�B�N���b�N����ƁA���X����C�Ԃ�
/// </summary>
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
        BuildDeck(buildNum);
    }

    private void Start()
    {
    }

    /// <summary>
    /// English: Clear the deck before creating a new one
    /// ���{��F�V�ȃf�b�L�����O�Ƀf�b�L���N���A����
    /// </summary>
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

    /// <summary>
    /// *** MAY NEED REIMPLEMENTATION ***
    /// English: Create a deck of count number of prefab cards stacking on top of each other
    /// ���{��Fcount���̃v���n�u�̃J�[�h�̃f�b�L�����B
    /// </summary>
    /// <param name="count"></param>
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
        if (player.selectManager.state == SelectManager.State.DrawFromDeck && cards.Count > 0)
        {
            selectResponse.GetComponent<SlightMovementSelectResponse>().moveObject = cards.Peek();
            selectResponse.GetComponent<ISelectResponse>().OnSelect();
        }
    }

    public void OnDeselect()
    {
        if (selectResponse == null) { return; }
        if (player.selectManager.state == SelectManager.State.DrawFromDeck && cards.Count > 0)
        {
            selectResponse.GetComponent<ISelectResponse>().OnDeselect();
        }
    }

    public void OnClick()
    {
        if(player.selectManager.state == SelectManager.State.DrawFromDeck)
        {
            Debug.Log($"Clicked on {this.gameObject.name}");
            selectResponse.GetComponent<SlightMovementSelectResponse>().moveObject = null;
            StartCoroutine(player.DrawOneSquirrel(false));
        }
    }
}
