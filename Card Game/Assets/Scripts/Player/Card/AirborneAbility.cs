using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: Make the card fly and be able to attack over obstacle (opponent card) directly at the opponent
/// ���{��F�J�[�h�𕂂��ׂāA��Q���i�������̃J�[�h�j���΂��A���ڍU��������
/// </summary>
public class AirborneAbility : MonoBehaviour, IAbility
{
    public IAbility.Stages stage;

    [SerializeField]
    private string abilityName;
    [SerializeField]
    private string abilityDescription;

    public IEnumerator ActivateAbility()
    {
        yield return null;
    }

    public string GetName() { return abilityName; }
    public string GetDescription() { return abilityDescription; }

    public IAbility.Stages GetStage() { return stage; }
}
