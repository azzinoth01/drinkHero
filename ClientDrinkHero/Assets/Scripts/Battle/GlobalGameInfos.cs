using UnityEngine;



public class GlobalGameInfos : MonoBehaviour {
    private static GlobalGameInfos _instance;

    [SerializeField] private PlayerObject _playerObject;
    [SerializeField] private EnemyObject _enemyObject;
    [SerializeField] private UserObjectOld _userObject;
    [SerializeField] private GameObject _turnManager;

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

    public UserObjectOld UserObject {
        get {
            return _userObject;
        }

        set {
            _userObject = value;
        }
    }

    private void Awake() {
        _instance = this;

        _playerObject.Player.Clear();

    }

    private void Update() {

        if (_turnManager.activeSelf == false) {
            _turnManager.SetActive(true);
        }

        return;
    }

    private void OnDisable() {
        ReadServerDataThread.KeepRunning = false;

    }

}
