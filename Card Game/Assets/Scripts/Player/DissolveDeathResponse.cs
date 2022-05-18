using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// *** NEED DISSOLVE EFFECT ***
/// English: A class that trigger a dissolve effect and destroy the parent object upon triggered
/// 日本語：トリガーされた後、溶けるエフェクトをトリガーし、親オブジェクトを破壊するクラス
/// </summary>
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
