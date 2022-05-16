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
            float y = spreadCircleCenterPosition.position.y + radius + Mathf.Cos(Mathf.Deg2Rad * angle) * radius / 10;
            float z = spreadCircleCenterPosition.position.z + radius - Mathf.Cos(Mathf.Deg2Rad * angle) * radius / 10;

            cardPositions[i] = new Vector3(x, y, z);
            Debug.DrawLine(cardPositions[i] + Vector3.forward, cardPositions[i] + Vector3.back, Color.yellow, 1);
        }
        for (int i = cards.Count - 1; i >= 0; i--)
        {
            if (inHandMovementTriggers.transform.childCount < cards.Count)
            {
                GameObject newTrigger = Instantiate(inHandMovementTriggers.transform.GetChild(0).gameObject, inHandMovementTriggers.transform);
                newTrigger.name = inHandMovementTriggers.transform.GetChild(0).name;
            }

            if (i < cards.Count - 1)
            {
                // In hand to adjust for incoming card
                inHandMovementTriggers.transform.GetChild(i).GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(cards[i], cardPositions[i]);
                inHandMovementTriggers.transform.GetChild(i).GetComponent<IMovementTrigger>().Trigger();
            }
            else
            {
                // From deck
                toHandMovementTrigger.GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(cards[i], cardPositions[i]);
                inHandMovementTriggers.transform.GetChild(i).GetComponent<IMovementTrigger>().InitializeMoveObjectTowards(cards[i], cardPositions[i]);
                toHandMovementTrigger.GetComponent<IMovementTrigger>().nextMovementTrigger = inHandMovementTriggers.transform.GetChild(i).gameObject;
                toHandMovementTrigger.GetComponent<IMovementTrigger>().Trigger();

            }
            
        }
    }

    
    private void OnDrawGizmos()
    {
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
        //Gizmos.DrawWireSphere(spreadCircleCenterPosition.position, radius);
    }
}
