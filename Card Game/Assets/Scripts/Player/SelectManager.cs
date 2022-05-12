using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager
{
    public ISelectable curSelect;
    public ISelectable prevSelect;

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

    public ISelectable GetSelectableAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask(Tags.SELECTABLE_LAYER), QueryTriggerInteraction.Ignore);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            return hit.collider.gameObject.GetComponent<ISelectable>();
        }
        return null;
    }
}
