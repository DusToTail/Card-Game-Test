using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveAbility
{
    public List<IAbility> GetAbilities();

    public void AddAbility(IAbility ability);

    public void RemoveAbility(IAbility ability);
}
