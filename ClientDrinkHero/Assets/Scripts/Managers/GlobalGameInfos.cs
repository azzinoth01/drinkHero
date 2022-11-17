using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;

public struct WriteBackData {
    public object instance;
    public MethodInfo method;
    public Type type;


    public WriteBackData(object instance, MethodInfo method, Type type) {
        this.instance = instance;
        this.method = method;
        this.type = type;

    }
}


public class GlobalGameInfos : MonoBehaviour {
    private static GlobalGameInfos _instance;

    [SerializeField] private PlayerObject _playerObject;
    [SerializeField] private EnemyObject _enemyObject;
    [SerializeField] private UserObject _userObject;
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] public int port = 6969;
    [SerializeField] public string host = "localhost";

    private CachedServerData _cachedServerData;
    public static Queue<WriteBackData> writeServerDataTo;

    public static Queue<bool> checkUpdates;
    private List<IWaitingOnServer> _waitOnServerObjects;


    private TcpClient _client;
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;


    private bool _waitOnStartData;

    private Thread _thread;

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


    public CachedServerData CachedServerData {
        get {
            return _cachedServerData;
        }


    }

    public StreamWriter Writer {
        get {
            return _writer;
        }


    }

    public List<IWaitingOnServer> WaitOnServerObjects {
        get {
            return _waitOnServerObjects;
        }


    }

    private void Awake() {
        _instance = this;
        _playerObject.Player.Clear();
        _waitOnStartData = false;
        writeServerDataTo = new Queue<WriteBackData>();
        checkUpdates = new Queue<bool>();
        _waitOnServerObjects = new List<IWaitingOnServer>();

        StartServerConnection();
        _cachedServerData = new CachedServerData();

        ClientFunctions.GetHeroDatabase();
        WriteBackData writeBack = new WriteBackData(_cachedServerData, _cachedServerData.GetType().GetMethod(nameof(_cachedServerData.SetHeroData)), typeof(HeroDatabase));
        writeServerDataTo.Enqueue(writeBack);

        ClientFunctions.GetRandomEnemyDatabase();
        writeBack = new WriteBackData(_enemyObject.enemy, _enemyObject.enemy.GetType().GetMethod(nameof(_enemyObject.enemy.SetEnemyData)), typeof(EnemyDatabase));
        writeServerDataTo.Enqueue(writeBack);

        ReadServerDataThread serverDataReadThread = new ReadServerDataThread(_reader);

        _thread = new Thread(serverDataReadThread.ThreadLoop);

        _thread.Start();

    }

    private void Update() {


        if (_waitOnStartData == true && checkUpdates.Count == 0) {
            return;
        }

        if (checkUpdates.Count != 0) {
            checkUpdates.Dequeue();

            for (int i = _waitOnServerObjects.Count - 1; i >= 0;) {

                if (_waitOnServerObjects[i].GetUpdateFromServer()) {
                    _waitOnServerObjects.RemoveAt(i);
                }

                i = i - 1;

            }
            foreach (IWaitingOnServer waiting in _waitOnServerObjects) {
                waiting.GetUpdateFromServer();
            }
        }

        if (_cachedServerData._heroData.Count != 0) {
            _waitOnStartData = true;
            CreatePlayer();
        }


        return;
    }

    public void CreatePlayer() {
        _playerObject.Player.Clear();
        GameDeck gameDeck = new GameDeck();
        Deck deck = new Deck();
        int i = 0;
        foreach (HeroDatabase heroDatabase in _cachedServerData._heroData.Values) {
            if (i > 3) {
                break;
            }
            Hero hero = new Hero();
            HeroSlot slot = new HeroSlot();
            slot.Hero = hero;
            deck.HeroSlotList.AddWithCascading(slot, deck);
            hero.HeroData = heroDatabase;
            i = i + 1;
        }
        gameDeck.Deck = deck;

        _playerObject.Player.GameDeck = gameDeck;
        _turnManager.enabled = true;
    }



    private void OnDisable() {
        ReadServerDataThread.KeepRunning = false;
        StopServerConnection();
    }

    private void StartServerConnection() {
        _client = new TcpClient("localhost", port);
        _stream = _client.GetStream();
        _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024);
        _writer = new StreamWriter(_stream, Encoding.UTF8, 1024);
        _writer.AutoFlush = true;

        ClientFunctions.SendMessageToDatabase("Start");



    }


    //public void SendDataToServer<T>(T item) {

    //    if (_client == null || _client.Connected == false) {
    //        return;
    //    }

    //    Packet packet = new Packet();

    //    packet.SetData(item);

    //    string text = JsonUtility.ToJson(packet);

    //    int size = text.Length;



    //    Byte[] sizeByte = new byte[4];

    //    BinaryPrimitives.WriteInt32BigEndian(sizeByte, size);

    //    byte[] data = System.Text.Encoding.UTF8.GetBytes(text);

    //    byte[] combinedData = new byte[sizeByte.Length + data.Length];

    //    System.Buffer.BlockCopy(sizeByte, 0, combinedData, 0, sizeByte.Length);
    //    System.Buffer.BlockCopy(data, 0, combinedData, 4, data.Length);


    //    _stream.Write(combinedData, 0, combinedData.Length);

    //}

    private void StopServerConnection() {
        if (_client != null) {
            _stream.Close();
            _client.Close();
        }

    }

}
