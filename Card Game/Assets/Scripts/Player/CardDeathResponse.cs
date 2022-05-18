using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A class that processes the death response of cards
/// 日本語：カード用のデス反応を処理するクラス
/// </summary>
public class CardDeathResponse : MonoBehaviour, IDeathResponse
{
    public void Trigger()
    {
        Destroy(gameObject);
    }
}
