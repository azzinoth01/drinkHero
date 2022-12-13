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

        //if (_skipTurn == true) {
        //    _skipTurn = false;
        //    ClientFunctions.SendMessageToDatabase("Enemy Turn End");
        //    EndTurn();
        //    return;
        //}

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
        ClientFunctions.SendMessageToDatabase("Enemy Turn End");
        EndTurn();
    }


    public void Heal(int value) {
        health = health + value;
        if (health > maxHealth) {
            health = maxHealth;
        }
        HealthChange?.Invoke(value);
    }

    public void TakeDmg(int value) {
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
        int healthDmg = 0;
        if (health - value < 0) {
            healthDmg = -health;
            health = 0;
        }
        else {
            healthDmg = -value;
            health -= value;
        }

        HealthChange?.Invoke(healthDmg);


        if (health <= 0) {

            DiedEvent?.Invoke();

        }
    }

    void ICharacterAction.Shield(int value) {
        shield = shield + value;
        ShieldChange?.Invoke(value);
    }

    public void AddAttackModifier(int value) {
        //  throw new NotImplementedException();
    }

    public void AddDefenceModifier(int value) {
        //  throw new NotImplementedException();
    }

    public void SwapShieldWithEnemy() {
        //  throw new NotImplementedException();
    }

    public void AttackEnemy(int value) {
        ICharacterAction playerActions = (ICharacterAction)UIDataContainer.Instance.Player;

        playerActions.TakeDmg(value);
    }

    public void RemoveShield() {
        shield = 0;
        ShieldChange?.Invoke(0);
    }

    public void SkipTurn() {
        //  throw new NotImplementedException();/
    }

    public void SetBaseMultihit(int value) {
        //  throw new NotImplementedException();
    }

    public void SetBuffMultihit(int value) {
        //  throw new NotImplementedException();
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
}
