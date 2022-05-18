using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: Base class for modularized abilities to be put on cards
/// ���{��F�J�[�h�ɍڂ���\�͂̃x�[�X�N���X
/// </summary>
[System.Serializable]
public class Ability
{
    /// <summary>
    /// English: Types of ability
    /// ���{��F�\�͂̎��
    /// </summary>
    public enum AbilityType
    {

    }

    public AbilityType m_type;

    /// <summary>
    /// English: Trigger the functionality of the ability
    /// ���{��F�\�͂𔭊�������
    /// </summary>
    public void ActivateAbility()
    {

    }
}
