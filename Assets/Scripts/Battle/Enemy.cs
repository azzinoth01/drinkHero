using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Enemy {
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _schild;
    [SerializeField] private int _attack;
    [SerializeField] private ElementEnum _element;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private List<EnemySkill> _skillList;

    //private Player _player;

    public int EnemyHealth => _health;
    public int EnemyMaxHealth => _maxHealth;

    public static event Action enemyTurnDone;
    public static event Action<float, float> updateEnemyHealthUI;

    public void TakeDmg(int dmg) {

        if (_schild > 0) {
            if (_schild > dmg) {
                _schild = _schild - dmg;
                dmg = 0;
            }
            else {
                dmg = dmg - _schild;
                _schild = 0;
            }
        }

        _health = _health - dmg;

        updateEnemyHealthUI?.Invoke(_health, _maxHealth);

        if (_health <= 0) {
            EnemyDeath();
        }
    }


    public void EnemyDeath() {

    }

    public void EnemyTurn() {
        bool usedSkill = false;
        for (int i = 0; i < _skillList.Count;) {
            EnemySkill skill = _skillList[i];

            if (skill.CurrentCooldown == 0 && usedSkill == false) {
                usedSkill = true;

                int dmg = Random.Range(skill.MinAttack, skill.MaxAttack);
                GlobalGameInfos.Instance.PlayerObject.PlayerReference.TakeDmg(dmg);

                Debug.Log("Enemy Attacks Player!");

                int schildValue = Random.Range(skill.MinSchild, skill.MaxSchild);
                _schild = _schild + schildValue;

                int healthValue = Random.Range(skill.MinHealth, skill.MaxHealth);
                _health = _health + healthValue;

                skill.StartCooldown();
            }
            else {
                skill.CooldownTick();
            }

            i = i + 1;
        }

        EndEnemyTurn();
    }

    public void EndEnemyTurn() {
        enemyTurnDone?.Invoke();
    }
}
