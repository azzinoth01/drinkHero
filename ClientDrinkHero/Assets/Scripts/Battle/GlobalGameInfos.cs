using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;



public class GlobalGameInfos : MonoBehaviour {
    private static GlobalGameInfos _instance;

    [SerializeField] private PlayerObject _playerObject;
    [SerializeField] private EnemyObject _enemyObject;
    [SerializeField] private UserObject _userObject;
    [SerializeField] private GameObject _turnManager;
    [SerializeField] public int port = 6969;
    [SerializeField] public string host = "markusdullnig.de";

    private CachedServerData _cachedServerData;

    private List<IWaitingOnServer> _waitOnServerObjects;


    private TcpClient _client;
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;


    private bool _waitOnStartData;

    private Thread _thread;

    private int _herodataWriteBackId;



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



        _waitOnServerObjects = new List<IWaitingOnServer>();

        StartServerConnection();
        _cachedServerData = new CachedServerData();

        string request = ClientFunctions.GetHeroDatabase();

        _herodataWriteBackId = HandleRequests.Instance.HandleRequest(request, typeof(HeroDatabase));


        //WriteBackData writeBack = new WriteBackData(_cachedServerData, _cachedServerData.GetType().GetMethod(nameof(_cachedServerData.SetHeroData)), typeof(HeroDatabase));
        //writeServerDataTo.Enqueue(writeBack);


        _enemyObject.enemy.EnemyDeath();

        //ClientFunctions.GetRandomEnemyDatabase();
        //writeBack = new WriteBackData(_enemyObject.enemy, _enemyObject.enemy.GetType().GetMethod(nameof(_enemyObject.enemy.SetEnemyData)), typeof(EnemyDatabase));
        //writeServerDataTo.Enqueue(writeBack);

        //ReadServerDataThread serverDataReadThread = new ReadServerDataThread(_reader, _stream);

        //_thread = new Thread(serverDataReadThread.ThreadLoop);

        //_thread.Start();
        //StartCoroutine(SendHeartbeat(3));
    }

    private void Update() {


        if (_waitOnStartData == true && ServerRequests.checkUpdates.Count == 0) {
            return;
        }

        if (ServerRequests.checkUpdates.Count != 0) {
            ServerRequests.checkUpdates.Dequeue();

            if (_waitOnStartData == false) {
                if (HandleRequests.Instance.RequestDataStatus[_herodataWriteBackId] == DataRequestStatusEnum.Recieved) {

                    List<HeroDatabase> list = TransmissionControl.GetObjectData<HeroDatabase>(HandleRequests.Instance.RequestData[_herodataWriteBackId]);

                    _cachedServerData.SetHeroData(list);
                    HandleRequests.Instance.RequestDataStatus[_herodataWriteBackId] = DataRequestStatusEnum.RecievedAccepted;
                }
            }

            for (int i = _waitOnServerObjects.Count - 1; i >= 0;) {

                if (_waitOnServerObjects[i].GetUpdateFromServer()) {
                    _waitOnServerObjects.RemoveAt(i);
                }

                i = i - 1;

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
        _turnManager.SetActive(true);
    }



    private void OnDisable() {
        ReadServerDataThread.KeepRunning = false;
        //StopServerConnection();
    }

    private void StartServerConnection() {


        ReadServerDataThread serverDataReadThread = new ReadServerDataThread(host, port, 3000);

        _thread = new Thread(serverDataReadThread.ThreadLoop);

        _thread.Start();

    }



}
