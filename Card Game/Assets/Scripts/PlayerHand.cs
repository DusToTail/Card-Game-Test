using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();
    [SerializeField]
    private PlayerDeck playerDeck;
    [SerializeField]
    private SquirrelDeck squirrelDeck;
    [SerializeField]
    private int initialCardNumber;

    private void Start()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void DrawInitialHand()
    {
        DrawOneSquirrel();
        for(int i = 0; i < initialCardNumber - 1; i++)
        {
            DrawOneFromPlayerDeck();
        }
    }

    public void DrawOneFromPlayerDeck()
    {
        if(playerDeck.cards.Count == 0) { return; }
        GameObject card = playerDeck.cards.Pop();
        card.transform.parent = transform;
        card.transform.position = transform.position;
        card.transform.rotation = Quaternion.LookRotation(transform.up);
        cards.Add(card);
    }

    public void DrawOneSquirrel()
    {
        if (squirrelDeck.cards.Count == 0) { return; }
        GameObject card = squirrelDeck.cards.Pop();
        card.transform.parent = transform;
        card.transform.position = transform.position;
        card.transform.rotation = Quaternion.LookRotation(transform.up);
        cards.Add(card);
    }

    public Card GetCardAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);

        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000,  LayerMask.GetMask(Tags.SELECTABLE_LAYER), QueryTriggerInteraction.Ignore);
        if(hit.collider.gameObject.GetComponent<Card>() != null)
        {
            return hit.collider.gameObject.GetComponent<Card>();
        }

        return null;
    }

}
