using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour
{
    public delegate void BellRinged();
    public static event BellRinged OnBellRinged;

    public void RingBell()
    {
        if (OnBellRinged != null)
        {
            OnBellRinged();
        }
    }



}
