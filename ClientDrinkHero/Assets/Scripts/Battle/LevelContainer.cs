using System.Collections.Generic;
using UnityEngine;

public class LevelContainer
{

    private List<EnemyData> _enemies;
    private EnemyData _boss;

    private EnemyBattle _currentEnemy;

    private bool _bossSpawned;


    private ModifierStruct _dmgModificator;
    private ModifierStruct _healthModificator;

    public int levelCount;


    public void NextEnemy() {
        if(_bossSpawned == true) {
            LoadNextLevel();
            return;
        }

        if(_enemies.Count == 0) {
            _currentEnemy.ResetEnemy(_boss);
            _bossSpawned = true;
        }
        else {
            _currentEnemy.ResetEnemy(_enemies[_enemies.Count - 1]);
            _enemies.RemoveAt(_enemies.Count - 1);

        }
        _currentEnemy.SetBaseModificator(_healthModificator,_dmgModificator);

    }
    public LevelContainer(EnemyBattle _enemyObject) {
        _bossSpawned = false;
        levelCount = 0;

        _currentEnemy = _enemyObject;

        _dmgModificator = new ModifierStruct(0,0);
        _healthModificator = new ModifierStruct(0,0);

        LoadNextLevel();
    }



    public void LoadNextLevel() {
        _bossSpawned = false;

        levelCount = levelCount + 1;
        LoadEnemies();
        AddModificators();
        NextEnemy();
    }

    private void AddModificators() {
        if(levelCount != 1) {
            _dmgModificator.AddModifier(20);
            _healthModificator.AddModifier(20);
        }
    }

    private void LoadEnemies() {
        int normalEnemiesCount = GameDataInstance.Instance.NormalEnemies.Count;
        _enemies = new List<EnemyData>();
        for(int i = 0; i < 3; i++) {
            int index = Random.Range(0,normalEnemiesCount);
            _enemies.Add(GameDataInstance.Instance.NormalEnemies[index]);
        }
        int bossEnemiesCount = GameDataInstance.Instance.BossEnemies.Count;
        int bossIndex = Random.Range(0,bossEnemiesCount);
        _boss = GameDataInstance.Instance.BossEnemies[bossIndex];
    }
}
