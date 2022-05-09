using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerName;
    public int playerIndex;

    [SerializeField]
    private PlayerDeck playerDeck;
    [SerializeField]
    private SquirrelDeck squirrelDeck;
    [SerializeField]
    private PlayerHand hand;
    [SerializeField]
    private BattleBoard board;
    [SerializeField]
    private Bell bell;

    private void Start()
    {
        
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

        }

        if (Input.GetMouseButton(0))
        {

        }

        if (Input.GetMouseButtonUp(0))
        {
            GameObject hit = GetSelectableAtMousePosition();
            if(hit != null)
            {
                if (hit.tag == Tags.PLAYER_DECK_TAG)
                {
                    hand.DrawOneFromPlayerDeck();
                }
                else if (hit.tag == Tags.SQUIRREL_DECK_TAG)
                {
                    hand.DrawOneSquirrel();
                }
                else if (hit.tag == Tags.CARD_TAG)
                {

                }

            }

        }

    }

    private GameObject GetSelectableAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);

        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask(Tags.SELECTABLE_LAYER), QueryTriggerInteraction.Ignore);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }

        return null;
    }






}
