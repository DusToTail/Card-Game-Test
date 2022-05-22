using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveAbility
{
    public List<GameObject> GetAbilities();

    public void AddAbility(GameObject ability);

    public void RemoveAbility(GameObject ability);
}
