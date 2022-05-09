using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Card : MonoBehaviour
{
    public string m_cardName;
    public string m_cardDescription;

    [SerializeField]
    protected Texture m_texture;
    [SerializeField]
    protected Sprite m_portrait;
    [SerializeField]
    protected Vector2 m_cardSize;

    public int m_initialHealth;
    public int m_initialAttackDamage;

    public int m_curHealth { get; private set; }
    public int m_curAttackDamage { get; private set; }
    public int m_actualAttackDamage { get; private set; }

    public List<Ability> abilities;

    private void Start()
    {
        GetComponent<BoxCollider>().size = new Vector3(m_cardSize.x, 1, m_cardSize.y);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = m_portrait;

    }

    public void Attack(Cell[] _targets)
    {
        m_actualAttackDamage = m_curAttackDamage;
        if(_targets[0].m_card == null)
        {
            // Attack opposite player
            Debug.Log($"{m_cardName} attacks opposite player for {m_actualAttackDamage}");
        }

        // Attack opposite cards
        for (int index = 0; index < _targets.Length; index++)
        {
            int targetHealth = _targets[index].m_card.m_curHealth;

            Debug.Log($"{m_cardName} attacks {_targets[index].m_card.m_cardName} for {m_actualAttackDamage}");
            _targets[index].m_card.AddCurrentHealth(-m_actualAttackDamage);

            m_actualAttackDamage -= targetHealth;
            if(m_actualAttackDamage < 0) { m_actualAttackDamage = 0; }
        }
        
    }

    public void OnDrawn()
    {
        Debug.Log($"{m_cardName} does something when drawn");
    }

    public void OnPlayed()
    {
        Debug.Log($"{m_cardName} does something when played");
    }

    public void OnAttack()
    {
        Debug.Log($"{gameObject.name} does something when attacking");
    }

    public void OnHit()
    {
        Debug.Log($"{m_cardName} does something when attacked");
    }

    public void OnKilled()
    {
        Debug.Log($"{m_cardName} does something when killed");
    }

    public void OnSacrificed()
    {
        Debug.Log($"{m_cardName} does something when sacrificed");
    }

    public void OnTurnStart()
    {
        Debug.Log($"{m_cardName} does something at the beginning of the turn");
    }

    public void OnTurnEnd()
    {
        Debug.Log($"{m_cardName} does something at the end of the turn");
    }


    public void AddCurrentHealth(int _amount)
    {
        Debug.Log($"Add {_amount} of health to {m_cardName}");
        m_curHealth += _amount;
    }

    public void AddCurrentAttackDamage(int _amount)
    {
        Debug.Log($"Add {_amount} of attack damage to {m_cardName}");
        m_curAttackDamage += _amount;
    }

    public void AddAbility(Ability _ability)
    {
        if (abilities.Contains(_ability)) { return; }
        Debug.Log($"Add ability {_ability} to {m_cardName}");
        abilities.Add(_ability);
    }
}
