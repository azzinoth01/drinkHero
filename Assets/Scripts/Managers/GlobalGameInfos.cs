using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameInfos : MonoBehaviour {
    private static GlobalGameInfos _instance;

    [SerializeField] private PlayerObject _playerObject;
    [SerializeField] private EnemyObject _enemyObject;
    [SerializeField] private UserObject _userObject;

    public static GlobalGameInfos Instance {
        get {
            return _instance;
        }
    }

    public PlayerObject PlayerObject {
        get {
            return _playerObject;
        }

        set {
            _playerObject = value;
        }
    }

    public EnemyObject EnemyObject {
        get {
            return _enemyObject;
        }

        set {
            _enemyObject = value;
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
    
    private void Awake()
    {
        _instance = this;
    }
}
