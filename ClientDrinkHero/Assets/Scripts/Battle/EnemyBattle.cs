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

    private bool _alreadyDead;

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

        _alreadyDead = false;
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


        VFXObjectContainer.Instance.PlayAnimation("enemySpawn");

        UIDataContainer.Instance.EnemySlot.LoadNewSprite(enemyData.SpritePath);


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
        CheckDebuffsAndBuffs(ActivationTimeEnum.turnEnd);

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

        VFXObjectContainer.Instance.PlayAnimation("41");
        if(UnityEngine.Random.value > 0.5f)
            PlayerTeam.Instance.PlayAnimation("Hurt");
        else 
            PlayerTeam.Instance.PlayAnimation("HurtDizzy");


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

        UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.heal, "+" + value.ToString());

        HealthChange?.Invoke(value);
        enemyHealed?.Invoke();
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

            UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.shield, shieldDmg.ToString());

            ShieldChange?.Invoke(shieldDmg);
            enemyDamageBlocked?.Invoke();
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

        UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.dmg, healthDmg.ToString());

        HealthChange?.Invoke(healthDmg);
        enemyDamageReceived?.Invoke();
        return value;
    }

    public void TakeDmg(int value) {

        value = _defenceModifier.CalcValue(value);

        value = DmgShield(value);

        value = DmgHealth(value);


        if (health <= 0 && _alreadyDead == false) {

            string request = ClientFunctions.AddMoneyToUser(_enemyBaseData.MoneyDrop);

            UserSingelton.Instance.UserObject.UpdateUserDataRequest(request);




            EnemyObject.GoldGotThisSession = EnemyObject.GoldGotThisSession + _enemyBaseData.MoneyDrop;
            _alreadyDead = true;
            DiedEvent?.Invoke();

        }
    }

    public void TakeShieldDmg(int value) {
        value = _defenceModifier.CalcValue(value);

        value = DmgShield(value);
    }


    void ICharacterAction.Shield(int value) {
        shield = shield + value;

        UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.shield, "+" + value.ToString());

        ShieldChange?.Invoke(value);
        enemyShieldUp?.Invoke();
    }

    public void AddAttackModifier(int value) {

        if (value > 0) {
            UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "ATT UP");
        }
        else {
            UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "ATT DOWN");
        }


        _dmgModifier.AddModifier(value);

    }

    public void AddDefenceModifier(int value) {
        if (value > 0) {
            UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "DEF UP");
        }
        else {
            UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "DEF DOWN");
        }

        _defenceModifier.AddModifier(value);
    }

    public void SwapShieldWithEnemy() {

        UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "SHIELD SWAP");
        UIDataContainer.Instance.PlayerText.SpawnFlyingText(FlyingTextEnum.effect, "SHIELD SWAP");





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
        UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "REMOVE SHIELD");
        shield = 0;
        ShieldChange?.Invoke(0);
    }

    public void SkipTurn(int value) {
        UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "STUNNED");
        _skipTurn = value;
    }

    public void SetBuffMultihit(int value) {
        UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "MULTIHIT +" + value);
        _buffMultihit = value;
    }

    private void CheckDebuffsAndBuffs(ActivationTimeEnum activation, int? value = null) {
        for (int i = BuffList.Count; i > 0;) {
            i = i - 1;
            if (BuffList[i].ActivateEffectBase(this, activation, value) == false) {
                BuffList.RemoveAt(i);
            }

        }
        for (int i = DebuffList.Count; i > 0;) {
            i = i - 1;
            if (DebuffList[i].ActivateEffectBase(this, activation, value) == false) {
                DebuffList.RemoveAt(i);
            }

        }
    }


    public void ShieldAttack() {
        AttackEnemy(shield);
    }

    //public void DiscardHandCards(int value) {
    //    Debug.Log("enemy can't discard cards");
    //}

    public void AddFixedAttackModifier(int value) {
        if (value > 0) {
            UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "ATT UP");
        }
        else {
            UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "ATT DOWN");
        }
        _dmgModifier.addFixedModifier(value);
    }

    public void AddFixedDefenceModifier(int value) {
        if (value > 0) {
            UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "DEF UP");
        }
        else {
            UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "DEF DOWN");
        }
        _defenceModifier.addFixedModifier(value);
    }

    //public int GetDiscadHandCardsCount() {
    //    Debug.Log("enemy has no discard cards");
    //    return 0;
    //}

    //public void Mana(int value) {
    //    Debug.Log("enemy has no mana");
    //}

    public void RemoveDebuff(int value) {
        UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, "REMOVE DEBUFF +" + value);
        for (int i = 0; i < value;) {
            if (_debuffList.Count == 0) {
                break;
            }
            _debuffList.RemoveAt(_debuffList.Count - 1);
            i = i + 1;
        }
    }

    public void CallEffectText(string Text) {

        UIDataContainer.Instance.EnemyText.SpawnFlyingText(FlyingTextEnum.effect, Text);

    }

    //public void DrawCard(int value) {
    //    Debug.Log("enemy can't draw cards");
    //}
}
