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

    private GameObject pickedCard;
    private bool cardPicked;

    private void Start()
    {
        cardPicked = false;
        pickedCard = null;
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
                    Debug.Log($"Clicked at {hit.name}");
                    hand.DrawOneFromPlayerDeck();
                }
                else if (hit.tag == Tags.SQUIRREL_DECK_TAG)
                {
                    Debug.Log($"Clicked at {hit.name}");
                    hand.DrawOneSquirrel();
                }
                else if(hit.tag == Tags.BELL_TAG)
                {
                    Debug.Log($"Clicked at {hit.name}");
                    bell.RingBell();
                }
                else if (hit.tag == Tags.CARD_TAG)
                {
                    Debug.Log($"Clicked at {hit.name}");
                    pickedCard = hit;
                }
                else if(hit.tag == Tags.BATTLE_BOARD_CELL_TAG)
                {
                    Debug.Log($"Clicked at {hit.name}");
                    board.PlayCardAt(pickedCard, new Vector2Int(int.Parse(hit.name[0].ToString()), int.Parse(hit.name[2].ToString())));
                    pickedCard = null;
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
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red, 10);
        return null;
    }






}
