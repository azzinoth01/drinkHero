using System.Collections.Generic;
using UnityEngine;

public class GameDataInstance
{

    private static GameDataInstance _instance;
    private UserObject _userObject;
    private UserSave _userSave;
    private Dictionary<string,ScriptableObject> _gameDatabase;
    private List<HeroObject> _ownedHeroes;
    private List<EnemyData> _bossEnemies;
    private List<EnemyData> _normalEnemies;
    private GameDataDatabase _gameDataDatabase;

    public static GameDataInstance Instance {
        get {
            if(_instance == null) {
                _instance = new GameDataInstance();
            }
            return _instance;
        }

    }

    public UserObject UserObject {
        get {
            return _userObject;
        }

        set {
            _userObject = value;
        }
    }

    public UserSave UserSave {
        get {
            return _userSave;
        }

        set {
            _userSave = value;
        }
    }

    public Dictionary<string,ScriptableObject> GameDatabase {
        get {
            if(_gameDatabase == null) {
                _gameDatabase = new Dictionary<string,ScriptableObject>();
            }
            return _gameDatabase;
        }

        set {
            _gameDatabase = value;
        }
    }

    public List<HeroObject> OwnedHeroes {
        get {
            if(_ownedHeroes == null) {
                _ownedHeroes = new List<HeroObject>();
            }
            return _ownedHeroes;
        }

        set {
            _ownedHeroes = value;
        }
    }

    public GameDataDatabase GameDataDatabase {
        get {
            return _gameDataDatabase;
        }

        set {
            _gameDataDatabase = value;
        }
    }

    public List<EnemyData> BossEnemies {
        get {
            if(_bossEnemies == null) {
                _bossEnemies = new List<EnemyData>();
            }
            return _bossEnemies;
        }

        set {
            _bossEnemies = value;
        }
    }

    public List<EnemyData> NormalEnemies {
        get {
            if(_normalEnemies == null) {
                _normalEnemies = new List<EnemyData>();
            }
            return _normalEnemies;
        }

        set {
            _normalEnemies = value;
        }
    }
}
