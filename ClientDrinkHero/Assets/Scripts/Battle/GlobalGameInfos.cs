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


    private List<IGetUpdateFromServer> _waitOnServerObjects;


    private TcpClient _client;
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;


    private bool _waitOnStartData;

    private Thread _thread;

    private int _herodataWriteBackId;
    private int _enemyWriteBackId;

    public EnemyHandler enemyHandler;



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




    public StreamWriter Writer {
        get {
            return _writer;
        }


    }

    public List<IGetUpdateFromServer> WaitOnServerObjects {
        get {
            return _waitOnServerObjects;
        }


    }

    private void Awake() {
        _instance = this;

        _playerObject.Player.Clear();
        _waitOnStartData = false;



        _waitOnServerObjects = new List<IGetUpdateFromServer>();

        StartServerConnection();


        //string request = ClientFunctions.GetHeroDatabase();

        //_herodataWriteBackId = HandleRequests.Instance.HandleRequest(request, typeof(HeroDatabase));



    }

    private void Update() {


        if (ServerRequests.checkUpdates.Count == 0) {
            return;
        }

        ServerRequests.checkUpdates.Dequeue();
        HandleRequests.Instance.CheckUpdateList();

        if (_turnManager.activeSelf == false) {
            _turnManager.SetActive(true);
        }

        return;
    }









    private void OnDisable() {
        ReadServerDataThread.KeepRunning = false;

    }

    private void StartServerConnection() {


        ReadServerDataThread serverDataReadThread = new ReadServerDataThread(host, port, 3000);

        _thread = new Thread(serverDataReadThread.ThreadLoop);

        _thread.Start();

    }



}
