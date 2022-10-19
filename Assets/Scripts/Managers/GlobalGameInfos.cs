using System.Net.Sockets;
using UnityEngine;


public class GlobalGameInfos : MonoBehaviour {
    private static GlobalGameInfos _instance;

    [SerializeField] private PlayerObject _playerObject;
    [SerializeField] private EnemyObject _enemyObject;
    [SerializeField] private UserObject _userObject;
    [SerializeField] public int port = 6969;
    [SerializeField] public string host = "localhost";


    public TcpClient _client;
    public NetworkStream _stream;

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





    private void Awake() {
        _instance = this;
        StartServerConnection();

    }

    private void OnDisable() {
        StopServerConnection();
    }

    private void StartServerConnection() {
        _client = new TcpClient("localhost", port);
        _stream = _client.GetStream();

        byte[] data = System.Text.Encoding.UTF8.GetBytes("Start");
        _stream.Write(data, 0, data.Length);

    }

    public void SendDataToServer(Card card) {
        string text = JsonUtility.ToJson(card);



        byte[] data = System.Text.Encoding.UTF8.GetBytes(text);

        _stream.Write(data, 0, data.Length);

    }

    private void StopServerConnection() {

        _stream.Close();
        _client.Close();
    }

}
