using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A select response that replace the old material with highlight material when hovered
/// ���{��F�}�E�X�I�[�o�[�̎��A�n�C���C�g�̃}�e���A�������ւ���I�𔽉��N���X
/// </summary>
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
