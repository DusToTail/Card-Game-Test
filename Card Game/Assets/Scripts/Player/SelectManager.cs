using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager
{
    public ISelectable curSelect;
    public ISelectable prevSelect;

    public enum State
    {
        DrawFromDeck,
        // CAN:
        // + move camera (WASD and mouse control)
        // + active selection for deck selectable to draw card
        // + active selection for check selectable (eg. in ability sigil) to check information .
        // CAN NOT:
        // + active selection for card selectable in hand
        // + active selection for cell selectable in boards.
        // + active selection for bell selectable
        // + active selection for generic items selectable (not implemented yet) to interact and may affect the player deck and the story progress

        CardInHand,
        // CAN:
        // + move camera (WASD and mouse control)
        // + active selection for card selectable in hand to pick up card (when there is no picked up card)
        // + active selection for bell selectable to signal the next phase
        // + active selection for check selectable (eg. in ability sigil) to check information .
        // CAN NOT:
        // + active selection for cell selectable in boards (when there is picked up card) to sacrifice / play on board
        // + active selection for deck selectable
        // + active selection for generic items selectable (not implemented yet) to interact and may affect the player deck and the story progress

        CardToBoard,
        // CAN:
        // + active selection for cell selectable in boards (when there is picked up card) to sacrifice / play on board
        // + active selection for check selectable (eg. in ability sigil) to check information .
        // CAN NOT:
        // + move camera (WASD and mouse control)
        // + active selection for card selectable in hand to pick up card (when there is no picked up card)
        // + active selection for bell selectable to signal the next phase
        // + active selection for deck selectable
        // + active selection for generic items selectable (not implemented yet) to interact and may affect the player deck and the story progress

        Exploration,
        // CAN:
        // + move body (with camera) (WASD)
        // + active selection for generic items selectable (not implemented yet) to interact and may affect the player deck and the story progress
        // CAN NOT:
        // + active selection for card selectable in hand to pick up card (when there is no picked up card)
        // + active selection for bell selectable to signal the next phase
        // + active selection for deck selectable

        Puzzle,
        // CAN:
        // + active selection for puzzle piece selectable (not implemented yet) to interact and may affect the player deck and the story progress
        // CAN NOT:
        // + move body (with camera) (WASD)
        // + active selection for card selectable in hand to pick up card (when there is no picked up card)
        // + active selection for bell selectable to signal the next phase
        // + active selection for deck selectable
        // + active selection for generic items selectable (not implemented yet) to interact and may affect the player deck and the story progress

        Battle,
        // CAN:
        // + None? since battle is automated
        // CAN NOT:
        // + move camera (WASD and mouse control)
        // + active selection for card selectable in hand to pick up card (when there is no picked up card)
        // + active selection for cell selectable in boards (when there is picked up card) to sacrifice / play on board
        // + active selection for bell selectable to signal the next phase
        // + active selection for check selectable (eg. in ability sigil) to check information .
        // + active selection for deck selectable
        // + active selection for generic items selectable (not implemented yet) to interact and may affect the player deck and the story progress

        End,
        // CAN:
        // + None? since ending is automated
        // CAN NOT:
        // + move camera (WASD and mouse control)
        // + active selection for card selectable in hand to pick up card (when there is no picked up card)
        // + active selection for cell selectable in boards (when there is picked up card) to sacrifice / play on board
        // + active selection for bell selectable to signal the next phase
        // + active selection for check selectable (eg. in ability sigil) to check information .
        // + active selection for deck selectable
        // + active selection for generic items selectable (not implemented yet) to interact and may affect the player deck and the story progress

        None

    }

    private State _state;

    public SelectManager()
    {
        _state = State.None;
    }

    public void ProcessPassiveSelection()
    {
        prevSelect = curSelect;
        curSelect = GetSelectableAtMousePosition();
        if(curSelect != null)
        {
            if(curSelect != prevSelect)
            {
                curSelect.OnSelect();
                if(prevSelect != null)
                {
                    prevSelect.OnDeselect();
                }
            }
        }
    }

    public void ProcessActiveSelection()
    {
        curSelect = GetSelectableAtMousePosition();
        if(curSelect != null)
        {
            curSelect.OnClick();
        }
    }

    public void SetSelectState(State state)
    {
        _state = state;
    }

    public ISelectable GetSelectableAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask(Tags.SELECTABLE_LAYER), QueryTriggerInteraction.Ignore);
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.gameObject.name);
            return hit.collider.gameObject.GetComponent<ISelectable>();
        }
        return null;
    }
}
