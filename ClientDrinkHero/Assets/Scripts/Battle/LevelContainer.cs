using System.Collections.Generic;

public class LevelContainer {

    private List<EnemyDatabase> _enemies;
    private EnemyDatabase _boss;

    private EnemyBattle _currentEnemy;

    private EnemyListHandler _enemyListHandler;
    private EnemyBossHandler _enemyBossHandler;

    private bool _boosLoadFinished;
    private bool _enemyLoadFinished;
    private bool _bossSpawned;


    private ModifierStruct _dmgModificator;
    private ModifierStruct _healthModificator;

    public int levelCount;


    public void NextEnemy() {
        if (_bossSpawned == true) {
            LoadNextLevel();
            return;
        }

        if (_enemies.Count == 0) {
            _currentEnemy.ResetEnemy(_boss);
            _bossSpawned = true;
        }
        else {
            _currentEnemy.ResetEnemy(_enemies[_enemies.Count - 1]);
            _enemies.RemoveAt(_enemies.Count - 1);

        }
        _currentEnemy.SetBaseModificator(_healthModificator, _dmgModificator);

    }
    public LevelContainer(EnemyBattle _enemyObject) {
        _bossSpawned = false;
        levelCount = 0;

        _currentEnemy = _enemyObject;

        _dmgModificator = new ModifierStruct(0, 0);
        _healthModificator = new ModifierStruct(0, 0);

        _enemyBossHandler = new EnemyBossHandler();
        _enemyListHandler = new EnemyListHandler();

        _enemyListHandler.LoadingFinished += SetEnemyLoadFinish;
        _enemyBossHandler.LoadingFinished += SetBossLoadFinish;

        LoadNextLevel();
    }

    public void Update() {
        _enemyBossHandler.Update();
        _enemyListHandler.Update();
    }


    public void LoadNextLevel() {
        _enemyLoadFinished = false;
        _boosLoadFinished = false;
        _bossSpawned = false;
        _enemyListHandler.RequestData();
        _enemyBossHandler.RequestData();

        levelCount = levelCount + 1;
    }
    private void SetEnemyLoadFinish() {
        _enemyLoadFinished = true;
        CheckFinishedLoading();
    }
    private void SetBossLoadFinish() {
        _boosLoadFinished = true;
        CheckFinishedLoading();
    }

    private void CheckFinishedLoading() {
        if (_enemyLoadFinished && _boosLoadFinished) {
            if (levelCount != 1) {
                _dmgModificator.AddModifier(20);
                _healthModificator.AddModifier(20);
            }


            _enemies = _enemyListHandler._enemyList;
            _boss = _enemyBossHandler._enemy;
            NextEnemy();
        }
    }

}
