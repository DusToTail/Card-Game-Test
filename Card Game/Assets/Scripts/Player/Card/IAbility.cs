using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility : IBasicInfo
{
    public enum Stages
    {
        OnAdded,
        StartOfTurn,
        OnDrawn,
        OnPlayed,
        OnAttack,
        OnDamaged,
        OnDeath,
        EndOfTurn
    }

    /// <summary>
    /// English: Trigger the functionality of the ability
    /// 日本語：能力を発揮させる
    /// </summary>
    public IEnumerator ActivateAbility();

    public Stages GetStage();
}
