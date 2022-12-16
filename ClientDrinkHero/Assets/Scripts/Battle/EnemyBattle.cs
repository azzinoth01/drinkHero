using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyBattle : ICharacter, ICharacterAction {
    [SerializeField] public EnemyDatabase _enemyBaseData;

    [SerializeField] public int maxHealth;
    [SerializeField] public int health;
    [SerializeField] public int shield;

    [SerializeField] private List<IBuff> _buffList;
    [SerializeField] private List<IDebuff> _debuffList;



    [SerializeField] protected ModifierStruct _healModifier;
    [SerializeField] protected ModifierStruct _dmgModifier;
    [SerializeField] protected ModifierStruct _defenceModifier;
    [SerializeField] protected ModifierStruct _shieldModifier;

    private int _skipTurn;
    private int _buffMultihit;

    public List<IBuff> BuffList {
        get {
            return _buffList;
        }

        set {
            _buffList = value;
        }
    }

    public List<IDebuff> DebuffList {
        get {
            return _debuffList;
        }

        set {
            _debuffList = value;
        }
    }

    public EnemyBattle() {

        UIDataContainer.Instance.Enemy = this;
    }
    public void ResetEnemy(EnemyDatabase enemyData) {
        _enemyBaseData = enemyData;
        maxHealth = enemyData.MaxHealth;
        health = maxHealth;
        shield = enemyData.Shield;

        _debuffList = new List<IDebuff>();
        _buffList = new List<IBuff>();

        ShieldChange?.Invoke(0);
        HealthChange?.Invoke(0);

        _skipTurn = 0;

        _dmgModifier = new ModifierStruct(0, 0);
        _healModifier = new ModifierStruct(0, 0);
        _defenceModifier = new ModifierStruct(0, 0);
        _shieldModifier = new ModifierStruct(0, 0);

    }

    public void SetBaseModificator(ModifierStruct healthModificator, ModifierStruct dmgModificator) {
        _dmgModifier = new ModifierStruct(dmgModificator);

        maxHealth = healthModificator.CalcValue(maxHealth);
        health = maxHealth;
        HealthChange?.Invoke(0);

    }


    public event Action<int> HealthChange;
    public event Action<int> ShieldChange;
    public event Action TurnEnded;
    public event Action DiedEvent;

    public static event Action enemyDamageReceived, enemyDamageBlocked, enemyHealed, enemyShieldUp;


    public void UpdateUI(int deltaHealth = 0, int deltaShield = 0) {
        HealthChange?.Invoke(deltaHealth);
        ShieldChange?.Invoke(deltaShield);
    }

    int ICharacter.MaxHealth() {
        return maxHealth;
    }

    public int CurrentHealth() {
        return health;
    }

    public int CurrentShield() {
        return shield;
    }

    public void EndTurn() {
        Debug.Log("turn end");
    }


    public void StartTurn() {
        ClientFunctions.SendMessageToDatabase("Enemy Turn Started");

        CheckDebuffsAndBuffs(ActivationTimeEnum.turnStart);


        bool usedSkill = false;

        if (_skipTurn > 0) {
            _skipTurn = _skipTurn - 1;
            ClientFunctions.SendMessageToDatabase("Enemy Turn End");
            EndTurn();
            TurnEnded?.Invoke();
            return;
        }

        //foreach (EnemySkill skill in _skillList.Values) {


        //    if (skill.CurrentCooldown == 0 && usedSkill == false) {
        //        usedSkill = true;

        //        int dmg = Random.Range(skill.MinAttack, skill.MaxAttack);


        //        GlobalGameInfos.Instance.PlayerObject.Player.TakeDmg(dmg);

        //        Debug.Log("Enemy Attacks Player!");

        //        int schildValue = Random.Range(skill.MinShield, skill.MaxSchield);
        //        _shield = _shield + schildValue;

        //        int healthValue = Random.Range(skill.MinHealth, skill.MaxHealth);
        //        _health = _health + healthValue;

        //        skill.StartCooldown();



        //    }
        //    else {
        //        skill.CooldownTick();
        //    }


        //}
        AttackEnemy(5);

        CheckDebuffsAndBuffs(ActivationTimeEnum.actionFinished);

        ClientFunctions.SendMessageToDatabase("Enemy Turn End");
        EndTurn();
        TurnEnded?.Invoke();
    }


    public void Heal(int value) {
        health = health + value;
        if (health > maxHealth) {
            health = maxHealth;
        }
        HealthChange?.Invoke(value);
    }



    private int DmgShield(int value) {
        int shieldDmg = 0;
        if (shield > 0) {
            if (shield > value) {
                shield = shield - value;
                shieldDmg = -value;
                value = 0;
            }
            else {
                value = value - shield;
                shieldDmg = -shield;
                shield = 0;

            }
            ShieldChange?.Invoke(shieldDmg);
        }
        return value;
    }

    private int DmgHealth(int value) {
        int healthDmg = 0;
        if (health - value < 0) {
            value = value - health;
            healthDmg = -health;
            health = 0;
        }
        else {
            healthDmg = -value;
            health -= value;
            value = 0;
        }

        HealthChange?.Invoke(healthDmg);
        return value;
    }

    public void TakeDmg(int value) {

        value = _defenceModifier.CalcValue(value);

        value = DmgShield(value);

        value = DmgHealth(value);


        if (health <= 0) {

            DiedEvent?.Invoke();

        }
    }

    public void TakeShieldDmg(int value) {
        value = _defenceModifier.CalcValue(value);

        value = DmgShield(value);
    }


    void ICharacterAction.Shield(int value) {
        shield = shield + value;
        ShieldChange?.Invoke(value);
    }

    public void AddAttackModifier(int value) {

        _dmgModifier.AddModifier(value);

    }

    public void AddDefenceModifier(int value) {
        _defenceModifier.AddModifier(value);
    }

    public void SwapShieldWithEnemy() {


        int tempShield = GlobalGameInfos.Instance.PlayerObject.Player.Shield;
        GlobalGameInfos.Instance.PlayerObject.Player.Shield = shield;
        shield = tempShield;
        UpdateUI();
        GlobalGameInfos.Instance.PlayerObject.Player.UpdateUI();

    }

    public void AttackEnemy(int value) {
        ICharacterAction playerActions = (ICharacterAction)UIDataContainer.Instance.Player;

        value = _dmgModifier.CalcValue(value);

        playerActions.TakeDmg(value);
    }

    public void RemoveShield() {
        shield = 0;
        ShieldChange?.Invoke(0);
    }

    public void SkipTurn(int value) {
        _skipTurn = value;
    }

    public void SetBuffMultihit(int value) {
        _buffMultihit = value;
    }

    private void CheckDebuffsAndBuffs(ActivationTimeEnum activation, int? value = null) {
        for (int i = BuffList.Count; i > 0;) {
            i = i - 1;
            if (BuffList[i].ActivateEffect(this, activation, value) == false) {
                BuffList.RemoveAt(i);
            }

        }
        for (int i = DebuffList.Count; i > 0;) {
            i = i - 1;
            if (DebuffList[i].ActivateEffect(this, activation, value) == false) {
                DebuffList.RemoveAt(i);
            }

        }
    }


    public void ShieldAttack() {
        AttackEnemy(shield);
    }

    public void DiscardHandCards(int value) {
        Debug.Log("enemy can't discard cards");
    }

    public void AddFixedAttackModifier(int value) {
        _dmgModifier.addFixedModifier(value);
    }

    public void AddFixedDefenceModifier(int value) {
        _defenceModifier.addFixedModifier(value);
    }

    public int GetDiscadHandCardsCount() {
        Debug.Log("enemy has no discard cards");
        return 0;
    }

    public void Mana(int value) {
        Debug.Log("enemy has no mana");
    }

    public void RemoveDebuff(int value) {
        for (int i = 0; i < value;) {
            if (_debuffList.Count == 0) {
                break;
            }
            _debuffList.RemoveAt(_debuffList.Count - 1);
            i = i + 1;
        }
    }

    public void DrawCard(int value) {
        Debug.Log("enemy can't draw cards");
    }
}