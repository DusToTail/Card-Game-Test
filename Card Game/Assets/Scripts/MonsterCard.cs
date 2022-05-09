using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCard : Card
{
    public int m_initialHealth;
    public int m_initialAttackDamage;

    public int m_curHealth { get; private set; }
    public int m_curAttackDamage { get; private set; }

    public List<Ability> abilities;


    public override void OnDrawn()
    {
        
    }

    public override void OnPlayed()
    {

    }

    public override void OnAttack()
    {

    }

    public override void OnHit()
    {
        
    }

    public override void OnKilled()
    {
        
    }

    public override void OnSacrificed()
    {

    }

    public override void OnTurnStart()
    {

    }

    public override void OnTurnEnd()
    {

    }


    public void AddCurrentHealth(int _amount)
    {

    }

    public void AddCurrentAttackDamage(int _amount)
    {

    }

    public void AddAbility(Ability _ability)
    {
        if (abilities.Contains(_ability)) { return; }

    }



}
