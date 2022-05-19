using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class for containing and handling the movement of cards within the player's hand
/// 日本語：プレイヤーの手にあるカードを持ち、カードの動きを管理するクラス
/// </summary>
public class PlayerHand : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> cards = new List<GameObject>();
    public int initialDrawNumber;
    public GameObject pickedCard { get; private set; }

    [Header("Movers")]
    [SerializeField]
    private GameObject toHandMovementTrigger;
    [SerializeField]
    private GameObject inHandMovementTriggers;
    [SerializeField]
    private GameObject inHandCardSelectResponseMovers;

    [Header("Spread Card Mover Specifics")]
    [SerializeField]
    private Transform spreadCircleCenterPosition;
    [SerializeField]
    private float maxCardAngle;
    [SerializeField]
    private float maxHandAngle;
    [SerializeField]
    private bool displayGizmos;

    /// <summary>
    /// English: Add a card to this class's list. And trigger the move animation towards the player's hand
    /// 日本語：カードをこのクラスのリストに追加する。その後、カードがプレイヤーの手に移動するよう、トリガーする
    /// </summary>
    /// <param name="card"></param>
    public IEnumerator AddCardToHand(GameObject card)
    {
        if(card == null) { yield break; }

        card.transform.parent = transform;
        cards.Add(card);
        Debug.Log($"Add {card.name} to hand");
        // Trigger animation to move the card to hand and adjust orientation
        yield return StartCoroutine(MoveCardToHand());

        card.layer = LayerMask.NameToLayer(Tags.SELECTABLE_LAYER);
    }

    /// <summary>
    /// English: Set this class's picked card to the parameter to perform movements such as playing card to the board
    /// 日本語：ボードに出せるよう、このクラスのピックアップされたカードを設定する
    /// </summary>
    /// <param name="card"></param>
    public void AssignPickedUpCard(GameObject card)
    {
        if(card == null) { pickedCard = null; }
       pickedCard = card;
    }

    /// <summary>
    /// English: for each card in the list, move them to the appropriate position on the hand.
    /// 日本語：リストにあるカードごとに、プレイヤーの手にある適切な位置に移動させる
    /// </summary>
    private IEnumerator MoveCardToHand()
    {
        // Calculate the positions into an array
        float radius = (spreadCircleCenterPosition.position - transform.position).magnitude;
        float totalHandAngle = cards.Count * maxCardAngle;
        if (totalHandAngle > maxHandAngle) { totalHandAngle = maxHandAngle; }

        Vector3[] cardPositions = new Vector3[cards.Count];
        for (int i = 0; i < cardPositions.Length; i++)
        {
            float angle = 90 - totalHandAngle / cards.Count * i + totalHandAngle / 2;
            float x = spreadCircleCenterPosition.position.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = spreadCircleCenterPosition.position.y + radius + Mathf.Cos(Mathf.Deg2Rad * angle) * radius / 10;
            float z = spreadCircleCenterPosition.position.z + radius - Mathf.Cos(Mathf.Deg2Rad * angle) * radius / 10;

            cardPositions[i] = new Vector3(x, y, z);
            Debug.DrawLine(cardPositions[i] + Vector3.forward, cardPositions[i] + Vector3.back, Color.yellow, 1);
        }

        // Initialize the movers with their unique moveObject and movePosition (calculated above)
        for (int i = cards.Count - 1; i >= 0; i--)
        {
            // Instantiate new movers and select responses if needed
            if (inHandMovementTriggers.transform.childCount < cards.Count)
            {
                GameObject newTrigger = Instantiate(inHandMovementTriggers.transform.GetChild(0).gameObject, inHandMovementTriggers.transform);
                newTrigger.name = inHandMovementTriggers.transform.GetChild(0).name;
                GameObject newCardSelectResponse = Instantiate(inHandCardSelectResponseMovers.transform.GetChild(0).gameObject, inHandCardSelectResponseMovers.transform);
                newCardSelectResponse.name = inHandCardSelectResponseMovers.transform.GetChild(0).name;
            }

            if (i < cards.Count - 1 && cards.Count > 1)
            {
                // In hand to adjust for incoming card
                inHandMovementTriggers.transform.GetChild(i).GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(cards[i], cardPositions[i]);
                inHandMovementTriggers.transform.GetChild(i).GetComponent<IMovementTrigger>().Trigger();
                yield return new WaitUntil(() => inHandMovementTriggers.transform.GetChild(i).GetComponent<IMovementTrigger>().isFinished);
            }
            else
            {
                // From deck
                toHandMovementTrigger.GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(cards[i], cardPositions[i]);
                toHandMovementTrigger.GetComponent<IMovementTrigger>().Trigger();
                yield return new WaitUntil(() => toHandMovementTrigger.transform.GetComponent<IMovementTrigger>().isFinished);
                inHandMovementTriggers.transform.GetChild(i).GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(cards[i], cardPositions[i]);
                inHandMovementTriggers.transform.GetChild(i).GetComponent<IMovementTrigger>().Trigger();
                yield return new WaitUntil(() => inHandMovementTriggers.transform.GetChild(i).GetComponent<IMovementTrigger>().isFinished);
            }

            inHandCardSelectResponseMovers.transform.GetChild(i).gameObject.GetComponent<SlightMovementSelectResponse>().moveObject = cards[i];
            inHandCardSelectResponseMovers.transform.GetChild(i).gameObject.GetComponent<SlightMovementSelectResponse>().initializedOnce = false;

            cards[i].GetComponent<CardSelectable>().SetSelectResponse(inHandCardSelectResponseMovers.transform.GetChild(i).gameObject);
            
        }
    }

    
    private void OnDrawGizmos()
    {
        if (!displayGizmos) { return; }
        // Draw the line where the cards will be placed in world position

        Gizmos.color = Color.magenta;
        float radius = (spreadCircleCenterPosition.position - transform.position).magnitude;
        Vector3[] vertices = new Vector3[20];
        for(int index = 0; index < 20; index++)
        {
            float angle = 90 - maxHandAngle / 20 * index + maxHandAngle / 2;
            float x = spreadCircleCenterPosition.position.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = spreadCircleCenterPosition.position.y + radius + Mathf.Cos(Mathf.Deg2Rad * angle) * radius / 10;
            float z = spreadCircleCenterPosition.position.z + radius - Mathf.Cos(Mathf.Deg2Rad * angle) * radius / 10;

            vertices[index] = new Vector3(x, y, z);

        }

        for(int index = 0; index < 19; index++)
        {
            Gizmos.DrawLine(vertices[index], vertices[index + 1]);
        }
    }
}
