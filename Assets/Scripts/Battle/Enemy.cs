using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Enemy {
    [SerializeField] private int _health;
    [SerializeField] private int _schild;
    [SerializeField] private int _attack;
    [SerializeField] private ElementEnum _element;
    [SerializeField] private Sprite _sprite;

    public Player player;

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

        if (_health <= 0) {
            EnemyDeath();
        }
    }


    public void EnemyDeath() {

    }

    public void EnemyTurn() {
        int dmg = Random.Range(_attack, _attack + 3);
        player.TakeDmg(dmg);

        EndEnemyTurn();
    }

    public void EndEnemyTurn() {

    }
}
