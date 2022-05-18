using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: Base class for modularized abilities to be put on cards
/// 日本語：カードに載せる能力のベースクラス
/// </summary>
[System.Serializable]
public class Ability
{
    /// <summary>
    /// English: Types of ability
    /// 日本語：能力の種類
    /// </summary>
    public enum AbilityType
    {

    }

    public AbilityType m_type;

    /// <summary>
    /// English: Trigger the functionality of the ability
    /// 日本語：能力を発揮させる
    /// </summary>
    public void ActivateAbility()
    {

    }
}
