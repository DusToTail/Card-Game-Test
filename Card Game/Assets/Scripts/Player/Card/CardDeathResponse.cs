using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class that processes the death response of cards
/// ���{��F�J�[�h�p�̃f�X��������������N���X
/// </summary>
public class CardDeathResponse : MonoBehaviour, IDeathResponse
{
    public void Trigger()
    {
        Destroy(gameObject);
    }
}
