using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class that handles passive (mouse hover) and active (mouse click) selection
/// 日本語：パッシブ（マウスオーバー）とアクティブ（マウスクリック）選択を処理するクラス
/// </summary>
public class SelectManager
{
    public ISelectable curSelect;
    public ISelectable prevSelect;

    /// <summary>
    /// English: Different states that allow player to select different objects
    /// 日本語：特定のオブジェクトのみを選べることにする複数の状態
    /// </summary>
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

    public State state { get; private set; }

    /// <summary>
    /// English: Default constructor. State to None
    /// 日本語：デフォルトコンストラクタ。状態は None
    /// </summary>
    public SelectManager()
    {
        state = State.None;
    }

    /// <summary>
    /// English: Process passive selection when hovering mouse. OnSelect() for new selection. OnDeselect() for previous selection
    /// 日本語：パッシブ選択（マウスオーバー）を処理する。新たに選択されたオブジェクトをOnSelect()させ、前に選択されたオブジェクトをOnDeselect()させる
    /// </summary>
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
        else
        {
            if(prevSelect != null)
                prevSelect.OnDeselect();
        }
    }

    /// <summary>
    /// English: Process active selection when clicking mouse.　Perform OnClick() function for current selection
    /// 日本語：パッシブ選択（マウスクリック）を処理する。現在に選択されているオブジェクトのOnClick()の関数を実行させる
    /// </summary>
    public void ProcessActiveSelection()
    {
        curSelect = GetSelectableAtMousePosition();
        if (curSelect != null)
        {
            curSelect.OnClick();
        }
    }

    /// <summary>
    /// English: Set the state of the select manager
    /// 日本語：状態を設定する
    /// </summary>
    /// <param name="state"></param>
    public void SetSelectState(State state)
    {
        this.state = state;
    }

    /// <summary>
    /// English: Return any instance that implement ISelectable at mouse position.
    /// 日本語：マウスの位置にあり、ISelectableを実装するインスタンスを返す。
    /// </summary>
    /// <returns></returns>
    public ISelectable GetSelectableAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask(Tags.SELECTABLE_LAYER), QueryTriggerInteraction.Ignore);
        if (hit.collider != null)
        {
            return hit.collider.gameObject.GetComponent<ISelectable>();
        }
        return null;
    }
}
