using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSelectResponse : MonoBehaviour, ISelectResponse
{
    [SerializeField]
    private Material highlightMaterial;
    private Material defaultMaterial;

    private void Start()
    {
        defaultMaterial = GetComponentInParent<Renderer>().material;   
    }

    public void OnDeselect()
    {
        GetComponentInParent<Renderer>().material = defaultMaterial;
    }

    public void OnSelect()
    {
        GetComponentInParent<Renderer>().material = highlightMaterial;
    }

}
