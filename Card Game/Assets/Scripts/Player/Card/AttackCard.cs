using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A base class for cards for monsters (cards that can participate in battles)
/// ���{��F�����X�^�[�i�o�g���ɎQ���ł���j�̃x�[�X�N���X
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class AttackCard : MonoBehaviour, IBasicInfo, IHaveHealth, IHaveAttack, IHaveAbility, INeedSacrifice
{
    [Header("Info")]
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


    private void Start()
    {
        // *** MAY NEED REIMPLEMENTATION (to be called in battle event controller class, etc) ***
        // Initialization of the cards
        _isPlayed = false;
        InitializeStats();

    }

    private void Update()
    {
        if (!_isPlayed) { return; }
    }

    /// <summary>
    /// *** MAY NEED REIMPLEMENTATION (to combine with attack movement called in battle board class) ***
    /// English: Attempt to attack cards (or opponent directly) with deductable attack damage
    /// ���{��F�ቺ�\�̍U���͂ŃJ�[�h�i�܂��͑���j���s�������݂�
    /// </summary>
    /// <param name="healths"></param>
    public void Attack(IHaveHealth[] healths)
    {
        // Check whether the attack is going to be direct or not
        bool directAttack = false;
        if(healths == null) { directAttack = true; }
        if(healths[0] == null) { directAttack = true; }

        // Initialize deductable attack damage (by minus opposite card's health)
        _actualAttackDamage = _curAttackDamage;

        if(directAttack)
        {
            // *** TO BE IMPLEMENTED ***
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

                // Deduct from attack damage
                _actualAttackDamage -= opponentHealth;

                // Check if surplus damage is available to continue or not
                if(_actualAttackDamage <= 0) { _actualAttackDamage = 0; break; }
            }
        }
        
    }

    /// <summary>
    /// English: Adds an amount to the card's initial attack damage. Does not accept negative or zero amount
    /// ���{��F�J�[�h�̏����̍U���͂𑝂₷�B�ʂ̓|�W�e�B�u�̂�
    /// </summary>
    /// <param name="amount"></param>
    public void PlusInitialAttackDamage(int amount)
    {
        if (amount <= 0) { return; }
        initialAttackDamage += amount;
        UpdateStatsText(false);
    }

    /// <summary>
    /// English: Minus an amount to the card's initial attack damage. Does not accept negative or zero amount
    /// ���{��F�J�[�h�̏����̍U���͂�ቺ����B�ʂ̓|�W�e�B�u�̂�
    /// </summary>
    /// <param name="amount"></param>
    public void MinusInitialAttackDamage(int amount)
    {
        if (amount <= 0) { return; }
        initialAttackDamage -= amount;
        if (initialAttackDamage < 0) { initialAttackDamage = 0; }
        UpdateStatsText(false);
    }

    /// <summary>
    /// English: Adds an amount to the card's current attack damage. Does not accept negative or zero amount
    /// ���{��F�J�[�h�̌��݂̍U���͂𑝂₷�B�ʂ̓|�W�e�B�u�̂�
    /// </summary>
    /// <param name="amount"></param>
    public void PlusCurrentAttackDamage(int amount)
    {
        if (amount <= 0) { return; }
        _curAttackDamage += amount;
        UpdateStatsText(true);
    }

    /// <summary>
    /// English: Minus an amount to the card's current attack damage. Does not accept negative or zero amount
    /// ���{��F�J�[�h�̌��݂̍U���͂�ቺ����B�ʂ̓|�W�e�B�u�̂�
    /// </summary>
    /// <param name="amount"></param>
    public void MinusCurrentAttackDamage(int amount)
    {
        if (amount <= 0) { return; }
        _curAttackDamage -= amount;
        if (_curAttackDamage < 0) { _curAttackDamage = 0; }
        UpdateStatsText(true);
    }

    /// <summary>
    /// English: Return the card's actual attack damage
    /// ���{��F�J�[�h�̎��ۂ̍U���͂�Ԃ�
    /// </summary>
    /// <returns></returns>
    public int GetActualAttackDamage()
    {
        return _actualAttackDamage;
    }

    /// <summary>
    /// English: Return the card's current attack damage
    /// ���{��F�J�[�h�̌��݂̍U���͂�Ԃ�
    /// </summary>
    /// <returns></returns>
    public int GetCurrentAttackDamage()
    {
        return _curAttackDamage;
    }

    /// <summary>
    /// English: Return the card's initial attack damage
    /// ���{��F�J�[�h�̏����̍U���͂�Ԃ�
    /// </summary>
    /// <returns></returns>
    public int GetInitialAttackDamage()
    {
        return initialAttackDamage;
    }

    

    

    /// <summary>
    /// English: Adds an amount to the card's initial health. Does not accept negative or zero amount
    /// ���{��F�J�[�h�̏����̗̑͂𑝂₷�B�ʂ̓|�W�e�B�u�̂�
    /// </summary>
    /// <param name="amount"></param>
    public void PlusInitialHealth(int amount)
    {
        if(amount <= 0) { return; }
        initialHealth += amount;
        UpdateStatsText(false);
    }

    /// <summary>
    /// English: Minus an amount to the card's initial health. Does not accept negative or zero amount
    /// ���{��F�J�[�h�̏����̗̑͂�ቺ����B�ʂ̓|�W�e�B�u�̂�
    /// </summary>
    /// <param name="amount"></param>
    public void MinusInitialHealth(int amount)
    {
        if (amount <= 0) { return; }
        initialHealth -= amount;
        if(initialHealth < 0) { initialHealth = 0; }
        UpdateStatsText(false);
    }

    /// <summary>
    /// English: Adds an amount to the card's current health. Does not accept negative or zero amount
    /// ���{��F�J�[�h�̌��݂̗̑͂𑝂₷�B�ʂ̓|�W�e�B�u�̂�
    /// </summary>
    /// <param name="amount"></param>
    public void PlusCurrentHealth(int amount)
    {
        if (amount <= 0) { return; }
        _curHealth += amount;
        UpdateStatsText(true);
    }

    /// <summary>
    /// English: Minus an amount to the card's current health. Does not accept negative or zero amount
    /// ���{��F�J�[�h�̌��݂̗̑͂�ቺ����B�ʂ̓|�W�e�B�u�̂�
    /// </summary>
    /// <param name="amount"></param>
    public void MinusCurrentHealth(int amount)
    {
        if (amount <= 0) { return; }
        _curHealth -= amount;
        if (_curHealth < 0) { _curHealth = 0; }
        UpdateStatsText(true);
    }

    /// <summary>
    /// English: Return the card's current health
    /// ���{��F�J�[�h�̌��݂̗̑͂�Ԃ�
    /// </summary>
    /// <returns></returns>
    public int GetCurrentHealth()
    {
        return _curHealth;
    }

    /// <summary>
    /// English: Return the card's initial health
    /// ���{��F�J�[�h�̏����̗̑͂�Ԃ�
    /// </summary>
    /// <returns></returns>
    public int GetInitialHealth()
    {
        return initialHealth;
    }

    /// <summary>
    /// English: React to an amount of damage after being attacked. For example, minus health or activate ability
    /// ���{��F�U�����ꂽ��A��������B�̗͂�ቺ������A�\�͂𔭊������肷��Ȃ�
    /// </summary>
    /// <param name="amount"></param>
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

    /// <summary>
    /// English: Check if the card's current health is zero or not
    /// ���{��F�J�[�h�̌��݂̗̑͂��[���ł��邩�ǂ����`�F�b�N����
    /// </summary>
    /// <returns></returns>
    public bool HealthIsZero()
    {
        if(GetCurrentHealth() <= 0) { return true; }
        return false;
    }

    /// <summary>
    /// English: Trigger the card's death response
    /// ���{��F�J�[�h�̃f�X�������g���K�[����
    /// </summary>
    public void Die()
    {
        if(deathResponse == null) { return; }
        deathResponse.GetComponent<IDeathResponse>().Trigger();
    }

    /// <summary>
    /// English: Return the card's name
    /// ���{��F�J�[�h�̖��O��Ԃ�
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        return cardName;
    }

    /// <summary>
    /// English: Return the card's description
    /// ���{��F�J�[�h�̃f�X�N���v�V������Ԃ�
    /// </summary>
    /// <returns></returns>
    public string GetDescription()
    {
        return cardDescription;
    }

    /// <summary>
    /// English: Initialize the stats of this card to prepare for battle
    /// ���{��F�o�g���̏��߂ɂ��̃J�[�h�̐��l������������
    /// </summary>
    public void InitializeStats()
    {
        _curHealth = initialHealth;
        _curAttackDamage = initialAttackDamage;
        _actualAttackDamage = _curAttackDamage;
        _curSacrificeCount = 0;

        UpdateNameText();
        UpdateStatsText(false);
    }

    /// <summary>
    /// English: Update the text regarding stats of this card during OR outside battle
    /// ���{��F�o�g�����܂��̓o�g���O�A���̃J�[�h�̐��l�ɉ����ăe�N�X�g���X�V����
    /// </summary>
    /// <param name="duringBattle"></param>
    public void UpdateStatsText(bool duringBattle)
    {
        if (duringBattle)
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

    /// <summary>
    /// English: Update the text regarding the name of this card
    /// ���{��F���̃J�[�h�̐��l�ɉ����ăe�N�X�g���X�V����
    /// </summary>
    public void UpdateNameText()
    {
        nameTextMesh.text = cardName;
    }

    /// <summary>
    /// *** MAY BE IMPLEMENTED
    /// English: Check if enough sacrifices are made to play this card
    /// ���{��F���̃J�[�h���o���̂ɕK�v�Ȑ��т̐��͏\�����ǂ����`�F�b�N����
    /// </summary>
    /// <returns></returns>
    public bool IsSatisfied()
    {
        return _curSacrificeCount >= sacrificeNum;
    }

    /// <summary>
    /// *** TO BE IMPLEMENTED
    /// English: Sacrifice cards with health
    /// ���{��F�̗͂̂���J�[�h���]������
    /// </summary>
    /// <param name="sacrificeCount"></param>
    public void Sacrifice(IHaveHealth[] sacrificeCount)
    {
        // Trigger sacrificed card sacriface animation and destroy them
    }

    /// <summary>
    /// *** TO BE IMPLEMENTED ***
    /// English: Add an ability to this card
    /// ���{��F�\�͂����̃J�[�h�ɒǉ�����
    /// </summary>
    /// <param name="ability"></param>
    public void AddAbility(IAbility ability)
    {
        // Trigger spin animation to prevent player from looking at the front of the card when adding the ability
        // Add a game object containing the ability
    }

    /// <summary>
    /// *** TO BE IMPLEMENTED ***
    /// English: Remove an ability from this card
    /// ���{��F���̃J�[�h����\�͂𔲂�
    /// </summary>
    /// <param name="ability"></param>
    public void RemoveAbility(IAbility ability)
    {
        // Trigger a removal animation (similar to Inscryption's paint brush item)
        // Remove the according game object containing the ability
    }

    /// <summary>
    /// English: Return the list containing this card's abilities
    /// ���{��F���̃J�[�h�̔\�̓��X�g��Ԃ�
    /// </summary>
    /// <returns></returns>
    public List<IAbility> GetAbilities()
    {
        return _abilityList;
    }
}
