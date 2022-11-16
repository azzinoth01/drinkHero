using System;
using System.Buffers.Binary;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class GlobalGameInfos : MonoBehaviour {
    private static GlobalGameInfos _instance;

    [SerializeField] private PlayerObject _playerObject;
    [SerializeField] private EnemyObject _enemyObject;
    [SerializeField] private UserObject _userObject;
    [SerializeField] public int port = 6969;
    [SerializeField] public string host = "localhost";


    private TcpClient _client;
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;

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

    public StreamReader Reader {
        get {
            return _reader;
        }


    }

    public StreamWriter Writer {
        get {
            return _writer;
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
        _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024);
        _writer = new StreamWriter(_stream, Encoding.UTF8, 1024);

        SendDataToServer("Start");


        StartCoroutine(testing());
    }


    public void SendDataToServer<T>(T item) {

        if (_client == null || _client.Connected == false) {
            return;
        }

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
        if (_client != null) {
            _stream.Close();
            _client.Close();
        }

    }

}
