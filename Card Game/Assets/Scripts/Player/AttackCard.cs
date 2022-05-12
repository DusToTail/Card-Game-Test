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
    private IDeathResponse deathResponse;
    [SerializeField]
    private CardScriptableObject cardScriptableObject;
    

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

    }

    private void Update()
    {
        if (!_isPlayed) { return; }

        if(_curHealth <= 0)
        {
            Die();
        }
    }

    public void Attack(IHaveHealth[] healths)
    {
        bool directAttack = false;
        if(healths == null) { directAttack = true; }
        if(healths[0] == null) { directAttack = true; }

        _actualAttackDamage = _curAttackDamage;

        if(directAttack)
        {
            Debug.Log("Attack Directly");
            Debug.Log($"{cardName} attack for {_actualAttackDamage}");
        }
        else
        {
            Debug.Log("Attack Opposite Enemy");
            for (int i = 0; i < healths.Length; i++)
            {
                if(healths[i] == null) { continue; }
                // Add animation of attacking in between
                int opponentHealth = healths[i].GetCurrentHealth();

                // Trigger enemy's GotHit function

                healths[i].MinusCurrentHealth(_actualAttackDamage);

                Debug.Log($"{cardName} attack for {_actualAttackDamage}");

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
    }

    public void PlusInitialHealth(int amount)
    {
        if(amount <= 0) { return; }
        initialHealth += amount;
    }

    public void MinusInitialHealth(int amount)
    {
        if (amount > 0) { return; }
        initialHealth -= amount;
        if(initialHealth < 0) { initialHealth = 0; }
    }

    public void PlusCurrentHealth(int amount)
    {
        if (amount <= 0) { return; }
        _curHealth += amount;
    }

    public void MinusCurrentHealth(int amount)
    {
        if (amount > 0) { return; }
        _curHealth -= amount;
        if (_curHealth < 0) { _curHealth = 0; }
    }

    public int GetCurrentHealth()
    {
        return _curHealth;
    }

    public int GetInitialHealth()
    {
        return initialHealth;
    }

    public bool HealthIsZero()
    {
        if(_curHealth <= 0) { return true; }
        return false;
    }

    public void Die()
    {
        deathResponse.Trigger();
    }

    public string GetName()
    {
        return cardName;
    }
    public string GetDescription()
    {
        return cardDescription;
    }

    


    /*
    public void Attack(Cell[] _targets)
    {
        _actualAttackDamage = _curAttackDamage;
        if(_targets[0].m_card == null)
        {
            // Attack opposite player
            Debug.Log($"{cardName} attacks opposite player for {_actualAttackDamage}");
        }

        // Attack opposite cards
        for (int index = 0; index < _targets.Length; index++)
        {
            int targetHealth = _targets[index].m_card._curHealth;

            Debug.Log($"{cardName} attacks {_targets[index].m_card.cardName} for {_actualAttackDamage}");
            _targets[index].m_card.AddCurrentHealth(-_actualAttackDamage);

            _actualAttackDamage -= targetHealth;
            if(_actualAttackDamage < 0) { _actualAttackDamage = 0; }
        }
        
    }

    private void Die()
    {
        Debug.Log($"{cardName} dies");
        OnDeath();
        Destroy(gameObject);
    }

    public void OnDrawn()
    {
        Debug.Log($"{cardName} does something when drawn");

        _curHealth = initialHealth;
        _curAttackDamage = initialAttackDamage;
    }

    public void OnPlayed()
    {
        Debug.Log($"{cardName} does something when played");
    }

    public void OnAttack()
    {
        Debug.Log($"{gameObject.name} does something when attacking");
    }

    public void OnHit()
    {
        Debug.Log($"{cardName} does something when attacked");
    }

    public void OnDeath()
    {
        Debug.Log($"{cardName} does something when dying");
    }

    public void OnSacrificed()
    {
        Debug.Log($"{cardName} does something when sacrificed");
    }

    public void OnTurnStart()
    {
        Debug.Log($"{cardName} does something at the beginning of the turn");
    }

    public void OnTurnEnd()
    {
        Debug.Log($"{cardName} does something at the end of the turn");
    }


    public void AddCurrentHealth(int _amount)
    {
        Debug.Log($"Add {_amount} of health to {cardName}");
        _curHealth += _amount;
    }

    public void AddCurrentAttackDamage(int _amount)
    {
        Debug.Log($"Add {_amount} of attack damage to {cardName}");
        _curAttackDamage += _amount;
    }

    public void AddAbility(Ability _ability)
    {
        if (abilities.Contains(_ability)) { return; }
        Debug.Log($"Add ability {_ability} to {cardName}");
        abilities.Add(_ability);
    }
    */
}
