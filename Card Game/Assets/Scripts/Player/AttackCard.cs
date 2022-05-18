using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AttackCard : MonoBehaviour, IBasicInfo, IHaveHealth, IHaveAttack, IHaveAbility, INeedSacrifice
{
    [SerializeField]
    private string cardName;
    [SerializeField]
    private string cardDescription;
    [SerializeField]
    private int initialHealth;
    [SerializeField]
    private int initialAttackDamage;
    [SerializeField]
    private GameObject abilityObject;
    [SerializeField]
    private int sacrificeNum;
    [SerializeField]
    private GameObject deathResponse;
    [SerializeField]
    private CardScriptableObject cardScriptableObject;

    [Header("Text")]
    [SerializeField]
    private TMPro.TextMeshPro nameTextMesh;
    [SerializeField]
    private TMPro.TextMeshPro healthTextMesh;
    [SerializeField]
    private TMPro.TextMeshPro powerTextMesh;


    private int _curHealth;
    private int _curAttackDamage;
    private int _actualAttackDamage;
    private List<IAbility> _abilityList = new List<IAbility>();
    private int _curSacrificeCount;


    private bool _isPlayed;




    private void Awake()
    {

    }

    private void Start()
    {
        _isPlayed = false;
        InitializeStats();

    }

    private void Update()
    {
        if (!_isPlayed) { return; }

    }

    public void Attack(IHaveHealth[] healths)
    {
        bool directAttack = false;
        if(healths == null) { directAttack = true; }
        if(healths[0] == null) { directAttack = true; }

        _actualAttackDamage = _curAttackDamage;

        if(directAttack)
        {
            Debug.Log($"{cardName} attack for {_actualAttackDamage} directly");
        }
        else
        {
            for (int i = 0; i < healths.Length; i++)
            {
                if(healths[i] == null) { continue; }

                // Add animation of attacking in between? (currently handled by BattleBoard class)
                int opponentHealth = healths[i].GetCurrentHealth();
                Debug.Log($"{cardName} attack opposite enemy for {_actualAttackDamage}");


                // Trigger enemy's TriggerHit function
                healths[i].TriggerHit(_actualAttackDamage);

                _actualAttackDamage -= opponentHealth;
                if(_actualAttackDamage <= 0) { _actualAttackDamage = 0; break; }
            }
        }
        
    }

    public bool IsSatisfied()
    {
        return _curSacrificeCount >= sacrificeNum;
    }

    public void Sacrifice(IHaveHealth[] sacrificeCount)
    {

    }

    public void AddAbility(IAbility ability)
    {
    }

    public void RemoveAbility(IAbility ability)
    {
    }

    public List<IAbility> GetAbilities()
    {
        return _abilityList;
    }

    public void InitializeStats()
    {
        _curHealth = initialHealth;
        _curAttackDamage = initialAttackDamage;
        _actualAttackDamage = _curAttackDamage;
        _curSacrificeCount = 0;

        nameTextMesh.text = cardName;
        healthTextMesh.text = _curHealth.ToString();
        powerTextMesh.text = _actualAttackDamage.ToString();
    }

    public void UpdateStatsText(bool duringBattle)
    {
        if(duringBattle)
        {
            healthTextMesh.text = _curHealth.ToString();
            powerTextMesh.text = _actualAttackDamage.ToString();
        }
        else
        {
            healthTextMesh.text = initialHealth.ToString();
            powerTextMesh.text = initialAttackDamage.ToString();
        }
        
    }

    public void PlusInitialHealth(int amount)
    {
        if(amount <= 0) { return; }
        initialHealth += amount;
        UpdateStatsText(false);
    }

    public void MinusInitialHealth(int amount)
    {
        if (amount <= 0) { return; }
        initialHealth -= amount;
        if(initialHealth < 0) { initialHealth = 0; }
        UpdateStatsText(false);
    }

    public void PlusCurrentHealth(int amount)
    {
        if (amount <= 0) { return; }
        _curHealth += amount;
        UpdateStatsText(true);
    }

    public void MinusCurrentHealth(int amount)
    {
        if (amount <= 0) { return; }
        _curHealth -= amount;
        if (_curHealth < 0) { _curHealth = 0; }
        UpdateStatsText(true);
    }

    public int GetCurrentHealth()
    {
        return _curHealth;
    }

    public int GetInitialHealth()
    {
        return initialHealth;
    }

    public void TriggerHit(int amount)
    {
        Debug.Log($"{gameObject.name} takes {amount} damage whilst having {GetCurrentHealth()}");
        MinusCurrentHealth(amount);
        if(HealthIsZero())
        {
            Debug.Log($"{gameObject.name}'s health is zero and is destroyed");
            // Trigger any death animation
            Die();
        }
    }

    public bool HealthIsZero()
    {
        if(GetCurrentHealth() <= 0) { return true; }
        return false;
    }

    public void Die()
    {
        if(deathResponse == null) { return; }
        deathResponse.GetComponent<IDeathResponse>().Trigger();
    }

    public string GetName()
    {
        return cardName;
    }
    public string GetDescription()
    {
        return cardDescription;
    }

    


    
}
