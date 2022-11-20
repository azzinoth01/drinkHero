



using MySql.Data.MySqlClient;

using System.Net;
using System.Net.Sockets;


public class DrinkHeroServer {


    private TcpListener _listener;
    private int _port = 6969;

    private List<ConnectedClient> _connectionList;
    private Thread _connectionThread;
    private Thread _listenThread;

    private static bool _isListen;
    private static bool _isConnected;

    public void StartServer() {



        string connector = @"server=localhost;userid=dbOwner;password=DrinkHero2022!;database=DrinkHeroDatenbank";

        MySqlConnection con = new MySqlConnection(connector);

        con.Open();


        DatabaseManager.Db = con;



        _connectionList = new List<ConnectedClient>();
        _listener = new TcpListener(IPAddress.Any, _port);
        _isListen = true;
        _listener.Start();



        _listenThread = new Thread(new ThreadStart(StartListen));
        _isConnected = true;

        _connectionThread = new Thread(new ThreadStart(ListenToConnections));

        _listenThread.Start();
        _connectionThread.Start();

        LogManager.LogQueue.Enqueue("[" + DateTime.Now.ToString() + "] Server Startet\r\n");
    }
    public DrinkHeroServer() {

    }

    public void CloseServer() {

        _isListen = false;
        _isConnected = false;

        while (_listenThread.IsAlive || _connectionThread.IsAlive) {
            Thread.Sleep(1);
        }

        foreach (ConnectedClient client in _connectionList) {
            client.CloseConnection();
        }
        _listener.Stop();
        DatabaseManager.Db.Close();
        LogManager.LogQueue.Enqueue("[" + DateTime.Now.ToString() + "] Server Closed\r\n");
    }


    public void StartListen() {

        while (_isListen) {
            if (_listener.Pending()) {
                Socket socket = _listener.AcceptSocket();
                socket.ReceiveTimeout = 10000;

                _connectionList.Add(new ConnectedClient(socket));

                LogManager.LogQueue.Enqueue("[" + DateTime.Now.ToString() + "] (" + socket.RemoteEndPoint.ToString() + ") Connected\r\n");

            }

            if (_isListen == false) {
                break;
            }

            Thread.Sleep(1);
        }


    }

    public void ListenToConnections() {
        while (_isConnected) {
            if (_connectionList.Count == 0) {
                Thread.Sleep(1);
                continue;
            }


            for (int i = 0; i < _connectionList.Count;) {
                if (_connectionList[i].Connection.Connected == false) {


                    LogManager.LogQueue.Enqueue("[" + DateTime.Now.ToString() + "] (" + _connectionList[i].RemoteIp + ") Disconnected\r\n");
                    _connectionList.RemoveAt(i);
                    continue;

                }
                else {

                    _connectionList[i].ReadConnection();

                    i = i + 1;
                }
            }


            Thread.Sleep(1);
        }

    }
}

