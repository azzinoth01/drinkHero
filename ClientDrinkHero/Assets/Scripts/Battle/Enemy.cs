using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Enemy : Character, IWaitingOnServer, ICharacter {

    [SerializeField] private int _attack;
    [SerializeField] private ElementEnum _element;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Dictionary<int, EnemySkill> _skillList;

    //public static event Action<int> updateEnemyShieldUI;
    //public static event Action enemyTurnDone;
    public static event Action enemyDamageReceived, enemyDamageBlocked, enemyHealed, enemyShieldUp;
    //public static event Action<float, float> updateEnemyHealthUI;
    public event Action<int> HealthChange;
    public event Action<int> ShieldChange;
    public event Action TurnEnded;

    private EnemyDatabase _enemyData;
    private bool _isWaitingOnServer;
    private int _requestEnemyId;

    public EnemyDatabase EnemyData {
        get {
            return _enemyData;
        }

        set {
            _enemyData = value;
            if (_enemyData != null) {
                if (ConvertEnemyData() == false) {
                    GlobalGameInfos.Instance.WaitOnServerObjects.Add(this);
                }
            }

        }
    }

    public void SetEnemyData(List<EnemyDatabase> enemyDatabases) {
        EnemyData = enemyDatabases[0];


    }

    public bool IsWaitingOnServer {
        get {
            return _isWaitingOnServer;
        }

        set {
            _isWaitingOnServer = value;
        }
    }

    public Dictionary<int, EnemySkill> SkillList {
        get {
            return _skillList;
        }


    }

    public Enemy() : base() {
        _skillList = new Dictionary<int, EnemySkill>();
        UIDataContainer.Instance.Enemy = this;
    }

    private bool ConvertEnemyData() {
        _isWaitingOnServer = false;
        _id = _enemyData.Id;
        _health = _enemyData.MaxHealth;
        _maxHealth = _enemyData.MaxHealth;
        _shield = _enemyData.Shield;


        List<EnemyToEnemySkill> enemyToEnemySkills = _enemyData.EnemyToEnemySkills;


        if (_enemyData.WaitingOnDataCount != 0) {
            _isWaitingOnServer = true;
        }

        if (_isWaitingOnServer) {
            return false;
        }


        foreach (EnemyToEnemySkill skill in enemyToEnemySkills) {
            if (skill.RefEnemySkill == null) {
                continue;
            }
            long id = skill.RefEnemySkill.Value;
            bool waitOn = false;
            int index = skill.Id;
            if (_skillList.TryGetValue(index, out EnemySkill enemySkill)) {

                enemySkill.EnemySkillData = skill.EnemySkill;

            }
            else {
                enemySkill = new EnemySkill();
                enemySkill.Id = id;
                enemySkill.EnemySkillData = skill.EnemySkill;
                _skillList.AddWithCascading(index, enemySkill, this);
            }
            if (skill.WaitingOnDataCount != 0) {
                waitOn = true;
            }
            _isWaitingOnServer = _isWaitingOnServer | waitOn;
        }

        if (_isWaitingOnServer) {
            return false;
        }

        Cascade(this);
        UpdateEnemyHealthUI(0);
        UpdateEnemyShieldUI(0);

        return true;
    }


    public void TakeDmg(int dmg) {
        //int lastHealth = _health;
        int shieldDmg = 0;
        if (_shield > 0) {
            if (_shield > dmg) {
                shieldDmg = -dmg;
                _shield = _shield - dmg;
                dmg = 0;
            }
            else {
                dmg = dmg - _shield;
                shieldDmg = -_shield;
                _shield = 0;
            }
            enemyDamageBlocked?.Invoke();
            UpdateEnemyShieldUI(shieldDmg);
        }

        int healthDmg = 0;
        if (_health - dmg < 0) {
            healthDmg = -_health;
            _health = 0;
        }
        else {
            healthDmg = -dmg;
            _health -= dmg;
        }

        //if (_health < lastHealth)
        //    enemyDamageReceived?.Invoke();

        UpdateEnemyShieldUI(healthDmg);

        if (_health <= 0) {
            EnemyDeath();
        }
    }


    public override void Clear() {
        base.Clear();
        _attack = 0;
        _element = ElementEnum.none;
        _sprite = null;
        _skillList = new Dictionary<int, EnemySkill>();
    }

    public void EnemyDeath() {
        // Invoke Win State or spawn next enemy
        // hand out exp whatever

        string callfunction = ClientFunctions.GetRandomEnemyDatabase();
        _requestEnemyId = HandleRequests.Instance.HandleRequest(callfunction, typeof(EnemyDatabase));
        _isWaitingOnServer = true;

    }

    public void EnemyTurn() {
        ClientFunctions.SendMessageToDatabase("Enemy Turn Started");
        //GlobalGameInfos.Instance.SendDataToServer("Enemy Turn Started");
        bool usedSkill = false;

        foreach (EnemySkill skill in _skillList.Values) {


            if (skill.CurrentCooldown == 0 && usedSkill == false) {
                usedSkill = true;

                int dmg = Random.Range(skill.MinAttack, skill.MaxAttack);
                GlobalGameInfos.Instance.PlayerObject.Player.TakeDmg(dmg);

                Debug.Log("Enemy Attacks Player!");

                int schildValue = Random.Range(skill.MinShield, skill.MaxSchield);
                _shield = _shield + schildValue;

                int healthValue = Random.Range(skill.MinHealth, skill.MaxHealth);
                _health = _health + healthValue;

                skill.StartCooldown();


                // server
                //EnemySkill logskill = new EnemySkill();
                //logskill.MinAttack = skill.MinAttack;
                //logskill.MinHealth = skill.MinHealth;
                //logskill.MinShield = skill.MinShield;

                //GlobalGameInfos.Instance.SendDataToServer(logskill);

            }
            else {
                skill.CooldownTick();
            }


        }
        ClientFunctions.SendMessageToDatabase("Enemy Turn End");
        //GlobalGameInfos.Instance.SendDataToServer("Enemy Turn End");
        EndEnemyTurn();
    }

    public void EndEnemyTurn() {
        TurnEnded?.Invoke();
    }

    private void UpdateEnemyShieldUI(int deltaValue) {
        ShieldChange?.Invoke(deltaValue);
    }

    private void UpdateEnemyHealthUI(int deltaValue) {
        HealthChange?.Invoke(deltaValue);
    }

    public bool GetUpdateFromServer() {
        return ConvertEnemyData();
    }

    int ICharacter.MaxHealth() {
        return _maxHealth;
    }

    public int CurrentHealth() {
        return _health;
    }

    public int CurrentShield() {
        return _shield;
    }

    public void EndTurn() {
        TurnEnded?.Invoke();
    }

    public void StartTurn() {
        EnemyTurn();
    }
}
