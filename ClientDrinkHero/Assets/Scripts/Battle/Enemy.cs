using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Enemy : Character, IWaitingOnServer {

    [SerializeField] private int _attack;
    [SerializeField] private ElementEnum _element;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Dictionary<long, EnemySkill> _skillList;

    public static event Action<int> updateEnemyShieldUI;
    public static event Action enemyTurnDone;
    public static event Action<float, float> updateEnemyHealthUI;

    private EnemyDatabase _enemyData;
    private bool _isWaitingOnServer;

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

    public bool IsWaitingOnServer {
        get {
            return _isWaitingOnServer;
        }

        set {
            _isWaitingOnServer = value;
        }
    }

    public Dictionary<long, EnemySkill> SkillList {
        get {
            return _skillList;
        }


    }

    public Enemy() : base() {
        _skillList = new Dictionary<long, EnemySkill>();
    }

    private bool ConvertEnemyData() {
        _id = _enemyData.Id;
        _health = _enemyData.MaxHealth;
        _maxHealth = _enemyData.MaxHealth;
        _shield = _enemyData.Shield;

        List<EnemyToEnemySkill> enemyToEnemySkills = _enemyData.GetEnemySkillList(out _isWaitingOnServer);

        if (_isWaitingOnServer) {
            return false;
        }


        foreach (EnemyToEnemySkill skill in enemyToEnemySkills) {
            long id = long.Parse(skill.RefEnemySkill);
            bool waitOn = false;

            if (_skillList.TryGetValue(id, out EnemySkill enemySkill)) {

                enemySkill.EnemySkillData = skill.GetEnemySkill(out waitOn);

            }
            else {
                enemySkill = new EnemySkill();
                enemySkill.Id = id;
                enemySkill.EnemySkillData = skill.GetEnemySkill(out waitOn);
                _skillList.AddWithCascading(id, enemySkill, this);
            }

            _isWaitingOnServer = _isWaitingOnServer | waitOn;
        }

        if (_isWaitingOnServer) {
            return false;
        }

        Cascade(this);

        return true;
    }


    public void TakeDmg(int dmg) {

        if (_shield > 0) {
            if (_shield > dmg) {
                _shield = _shield - dmg;
                dmg = 0;
            }
            else {
                dmg = dmg - _shield;
                _shield = 0;
            }

            UpdateEnemyShieldUI();
        }

        if (_health - dmg < 0) {
            _health = 0;
        }
        else {
            _health -= dmg;
        }

        UpdateEnemyHealthUI();

        if (_health <= 0) {
            EnemyDeath();
        }
    }


    public override void Clear() {
        base.Clear();
        _attack = 0;
        _element = ElementEnum.none;
        _sprite = null;
        _skillList = new Dictionary<long, EnemySkill>();
    }

    public void EnemyDeath() {
        // Invoke Win State or spawn next enemy
        // hand out exp whatever
    }

    public void EnemyTurn() {
        ClientFunctions.SendMessageToDatabase("Enemy Turn Started");
        //GlobalGameInfos.Instance.SendDataToServer("Enemy Turn Started");
        bool usedSkill = false;
        for (int i = 0; i < _skillList.Count;) {
            EnemySkill skill = _skillList[i];

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

            i = i + 1;
        }
        ClientFunctions.SendMessageToDatabase("Enemy Turn End");
        //GlobalGameInfos.Instance.SendDataToServer("Enemy Turn End");
        EndEnemyTurn();
    }

    public void EndEnemyTurn() {
        enemyTurnDone?.Invoke();
    }

    private void UpdateEnemyShieldUI() {
        updateEnemyShieldUI?.Invoke(_shield);
    }

    private void UpdateEnemyHealthUI() {
        updateEnemyHealthUI?.Invoke(_health, _maxHealth);
    }

    public bool GetUpdateFromServer() {
        return ConvertEnemyData();
    }
}
