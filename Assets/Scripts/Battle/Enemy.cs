using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Enemy {
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _shield;
    [SerializeField] private int _attack;
    [SerializeField] private ElementEnum _element;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private List<EnemySkill> _skillList;

    public static event Action<int> updateEnemyShieldUI;

    public int EnemyHealth => _health;
    public int EnemyMaxHealth => _maxHealth;

    public int EnemyShield => _shield;

    public static event Action enemyTurnDone;
    public static event Action<float, float> updateEnemyHealthUI;

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


    public void EnemyDeath() {
        // Invoke Win State or spawn next enemy
        // hand out exp whatever
    }

    public void EnemyTurn() {
        GlobalGameInfos.Instance.SendDataToServer("Enemy Turn Started");
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
                EnemySkill logskill = new EnemySkill();
                logskill.MinAttack = skill.MinAttack;
                logskill.MinHealth = skill.MinHealth;
                logskill.MinShield = skill.MinShield;

                GlobalGameInfos.Instance.SendDataToServer(logskill);

            }
            else {
                skill.CooldownTick();
            }

            i = i + 1;
        }
        GlobalGameInfos.Instance.SendDataToServer("Enemy Turn End");
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
}
