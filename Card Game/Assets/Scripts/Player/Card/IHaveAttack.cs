using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveAttack
{
    public void Attack(IHaveHealth[] healths);
}
