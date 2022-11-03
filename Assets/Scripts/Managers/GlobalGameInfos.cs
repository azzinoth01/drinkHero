using System;
using System.Buffers.Binary;
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

        SendDataToServer("Start");

        //byte[] data = System.Text.Encoding.UTF8.GetBytes("Start");
        //_stream.Write(data, 0, data.Length);

    }

    public void SendDataToServer<T>(T item) {

        Packet packet = new Packet();

        packet.SetData(item);

        string text = JsonUtility.ToJson(packet);

        int size = text.Length;



        Byte[] sizeByte = new byte[4];

        BinaryPrimitives.WriteInt32BigEndian(sizeByte, size);

        byte[] data = System.Text.Encoding.UTF8.GetBytes(text);

        byte[] combinedData = new byte[sizeByte.Length + data.Length];

        System.Buffer.BlockCopy(sizeByte, 0, combinedData, 0, sizeByte.Length);
        System.Buffer.BlockCopy(data, 0, combinedData, 4, data.Length);


        _stream.Write(combinedData, 0, combinedData.Length);

    }

    private void StopServerConnection() {

        _stream.Close();
        _client.Close();
    }

}
