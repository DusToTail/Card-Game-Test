using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveHealth
{
    public void PlusInitialHealth(int amount);
    public void MinusInitialHealth(int amount);

    public void PlusCurrentHealth(int amount);
    public void MinusCurrentHealth(int amount);

    public int GetCurrentHealth();
    public int GetInitialHealth();

    public void TriggerHit(int amount);
    public bool HealthIsZero();
    public void Die();
}
