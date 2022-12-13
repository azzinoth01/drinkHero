using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Enemy : Character, IGetUpdateFromServer, ICharacter {

    [SerializeField] private int _attack;
    [SerializeField] private ElementEnum _element;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Dictionary<int, EnemySkill> _skillList;

    //public static event Action<int> updateEnemyShieldUI;
    //public static event Action enemyTurnDone;
    public static event Action enemyDamageReceived, enemyDamageBlocked, enemyHealed, enemyShieldUp;
    //public static event Action<float, float> updateEnemyHealthUI;



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
        if (_enemyData == null) {
            return false;
        }
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

            }
            if (skill.WaitingOnDataCount != 0) {
                waitOn = true;
            }
            _isWaitingOnServer = _isWaitingOnServer | waitOn;
        }

        if (_isWaitingOnServer) {
            return false;
        }




        return true;
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
        GlobalGameInfos.Instance.WaitOnServerObjects.Add(this);

    }

    public void EnemyTurn() {
        ClientFunctions.SendMessageToDatabase("Enemy Turn Started");

        CheckDebuffsAndBuffs(ActivationTimeEnum.turnStart);


        bool usedSkill = false;

        if (_skipTurn == true) {
            _skipTurn = false;
            ClientFunctions.SendMessageToDatabase("Enemy Turn End");
            EndTurn();
            return;
        }

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



            }
            else {
                skill.CooldownTick();
            }


        }
        ClientFunctions.SendMessageToDatabase("Enemy Turn End");
        EndTurn();
    }





    public bool GetUpdateFromServer() {
        if (HandleRequests.Instance.RequestDataStatus[_requestEnemyId] == DataRequestStatusEnum.Recieved) {

            List<EnemyDatabase> list = EnemyDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_requestEnemyId]);

            _enemyData = list[0];
            HandleRequests.Instance.RequestDataStatus[_requestEnemyId] = DataRequestStatusEnum.RecievedAccepted;

        }

        return ConvertEnemyData();
    }

    protected override void Death() {
        EnemyDeath();
    }

    public override void EndTurn() {
        CheckDebuffsAndBuffs(ActivationTimeEnum.turnEnd);
        InvokeTurnEnd();
    }

    public override void StartTurn() {
        EnemyTurn();
    }

    public override void SwapShieldWithEnemy() {
        int tempShield = GlobalGameInfos.Instance.PlayerObject.Player.Shield;
        GlobalGameInfos.Instance.PlayerObject.Player.Shield = Shield;
        Shield = tempShield;
    }
    public override void AttackEnemy(int value) {
        GlobalGameInfos.Instance.PlayerObject.Player.TakeDmg(value);
        _dmgCausedThisAction = _dmgCausedThisAction + value;
    }
}
