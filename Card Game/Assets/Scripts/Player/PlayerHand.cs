using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> cards = new List<GameObject>();
    public int initialDrawNumber;
    public GameObject pickedCard { get; private set; }

    private void Start()
    {

    }

    public void AddCardToHand(GameObject card)
    {
        if(card == null) { return; }

        card.transform.parent = transform;
        card.layer = LayerMask.NameToLayer(Tags.SELECTABLE_LAYER);
        cards.Add(card);

        // Trigger animation to move the card to hand
        card.transform.localPosition = Vector3.zero;
    }

    public void AssignPickedUpCard(GameObject card)
    {
        if(card == null) { pickedCard = null; }
       pickedCard = card;
    }

    /*
    private void MoveCardAwayFrom(GameObject _card, bool _fromDeck)
    {
        if (_fromDeck)
        {
            while (Vector3.Distance(transform.position, _card.transform.position) < 20)
            {
                _card.transform.position -= _card.transform.forward * 0.01f;
            }
            _card.transform.position = rightPosition.position;
            _card.transform.rotation = Quaternion.LookRotation(transform.up);
        }
    }

    private void AdjustCardsInHand()
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
            Debug.DrawLine(cardPositions[i] + Vector3.forward, cardPositions[i] + Vector3.back, Color.yellow, 10);
        }
        for (int i = 0; i < cardPositions.Length; i++)
        {
            cards[i].transform.position = Vector3.Lerp(cards[i].transform.position, cardPositions[i], 1);
            cards[i].transform.rotation = Quaternion.LookRotation(Camera.main.transform.up);
        }
    }

    public AttackCard GetCardAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);

        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000,  LayerMask.GetMask(Tags.SELECTABLE_LAYER), QueryTriggerInteraction.Ignore);
        if(hit.collider.gameObject.GetComponent<AttackCard>() != null)
        {
            return hit.collider.gameObject.GetComponent<AttackCard>();
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        float radius = (spreadCircleCenterPosition.position - transform.position).magnitude;
        Gizmos.DrawWireSphere(spreadCircleCenterPosition.position, radius);
    }
    */
}
