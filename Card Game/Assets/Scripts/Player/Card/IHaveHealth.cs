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

    public IEnumerator TriggerHit(int amount);
    public bool HealthIsZero();
    public IEnumerator Die();
}
