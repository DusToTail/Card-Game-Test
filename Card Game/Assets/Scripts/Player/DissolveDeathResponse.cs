using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveDeathResponse : MonoBehaviour, IDeathResponse
{
    [SerializeField]
    private Material disintegrateMaterial;

    private bool started = false;
    private float t = 0;

    private void Update()
    {
        if (!started) { return; }
        t += Time.deltaTime;

        if(t > 1)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public void Trigger()
    {
        //transform.parent.GetComponent<Renderer>().material = disintegrateMaterial;
        started = true;
    }
}
