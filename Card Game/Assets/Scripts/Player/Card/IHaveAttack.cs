using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveAttack
{
    public void Attack(IHaveHealth[] healths);

    public void PlusInitialAttackDamage(int amount);
    public void MinusInitialAttackDamage(int amount);

    public void PlusCurrentAttackDamage(int amount);
    public void MinusCurrentAttackDamage(int amount);

    public int GetActualAttackDamage();
    public int GetCurrentAttackDamage();
    public int GetInitialAttackDamage();
}
