using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> cards = new List<GameObject>();
    public int initialDrawNumber;
    public GameObject pickedCard { get; private set; }

    [SerializeField]
    private GameObject toHandMovementTrigger;
    [SerializeField]
    private GameObject inHandMovementTriggers;

    [SerializeField]
    private Transform spreadCircleCenterPosition;
    [SerializeField]
    private float maxCardAngle;
    [SerializeField]
    private float maxHandAngle;

    private void Start()
    {

    }

    public void AddCardToHand(GameObject card)
    {
        if(card == null) { return; }

        card.transform.parent = transform;
        card.layer = LayerMask.NameToLayer(Tags.SELECTABLE_LAYER);
        cards.Add(card);

        // Trigger animation to move the card to hand and adjust orientation
        MoveCardToHand();
    }

    public void AssignPickedUpCard(GameObject card)
    {
        if(card == null) { pickedCard = null; }
       pickedCard = card;
    }

    private void MoveCardToHand()
    {
        float radius = (spreadCircleCenterPosition.position - transform.position).magnitude;
        float totalHandAngle = cards.Count * maxCardAngle;
        if (totalHandAngle > maxHandAngle) { totalHandAngle = maxHandAngle; }

        Vector3[] cardPositions = new Vector3[cards.Count];
        for (int i = 0; i < cardPositions.Length; i++)
        {
            float angle = 90 - totalHandAngle / cards.Count * i + totalHandAngle / 2;
            float x = spreadCircleCenterPosition.position.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = spreadCircleCenterPosition.position.y + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            cardPositions[i] = new Vector3(x, y, transform.position.z);
            Debug.DrawLine(cardPositions[i] + Vector3.forward, cardPositions[i] + Vector3.back, Color.yellow, 1);
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if(i < cards.Count - 1)
            {
                // In hand to adjust for incoming card
                if(inHandMovementTriggers.transform.childCount < cards.Count)
                {
                    GameObject newTrigger = Instantiate(inHandMovementTriggers.transform.GetChild(0).gameObject, inHandMovementTriggers.transform);
                    newTrigger.name = inHandMovementTriggers.transform.GetChild(0).name;
                }
                inHandMovementTriggers.transform.GetChild(i).GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(cards[i], cardPositions[i]);
                inHandMovementTriggers.transform.GetChild(i).GetComponent<IMovementTrigger>().Trigger();
            }
            else
            {
                // From deck
                toHandMovementTrigger.GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(cards[i], cardPositions[i]);
                toHandMovementTrigger.GetComponent<IMovementTrigger>().Trigger();

            }
            
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        float radius = (spreadCircleCenterPosition.position - transform.position).magnitude;
        Gizmos.DrawWireSphere(spreadCircleCenterPosition.position, radius);
    }
}
