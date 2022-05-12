using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeathResponse : MonoBehaviour, IDeathResponse
{
    public void Trigger()
    {
        Destroy(gameObject);
    }
}
