using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellSelectable : MonoBehaviour, ISelectable
{
    [SerializeField]
    private ISelectResponse selectResponse;

    public delegate void BellRinged();
    public static event BellRinged OnBellRinged;

    public void RingBell()
    {
        if (OnBellRinged != null)
        {
            Debug.Log("Bell Ringed");
            OnBellRinged();
        }
    }

    public void OnSelect()
    {
        if(selectResponse == null) { return; }
        selectResponse.OnSelect();
    }

    public void OnDeselect()
    {
        if (selectResponse == null) { return; }
        selectResponse.OnDeselect();
    }

    public void OnClick()
    {
        RingBell();
    }
}
