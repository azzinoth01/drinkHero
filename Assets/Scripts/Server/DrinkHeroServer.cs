using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class DrinkHeroServer {


    private TcpListener _listener;
    private int _port = 6969;

    private List<ConnectedClient> _connectionList;
    private Thread _connectionThread;
    private Thread _listenThread;

    private static bool _isListen;
    private static bool _isConnected;

    public void StartServer() {

        string connectionString = Application.dataPath;

        int pos = connectionString.LastIndexOf("/");

        connectionString = connectionString.Substring(0, pos);

        connectionString = connectionString + "/" + "DrinkHeroDatabase.db";

        connectionString = "URI=file:" + connectionString;

        SqliteConnection databaseConnection = new SqliteConnection(connectionString);

        databaseConnection.Open();
        DatabaseManager.Db = databaseConnection;




        _connectionList = new List<ConnectedClient>();
        _listener = new TcpListener(IPAddress.Any, _port);
        _isListen = true;
        _listener.Start();



        _listenThread = new Thread(new ThreadStart(StartListen));
        _isConnected = true;

        _connectionThread = new Thread(new ThreadStart(ListenToConnections));

        _listenThread.Start();
        _connectionThread.Start();

        Debug.LogError("Server Startet");
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
        Debug.LogError("Server Closed");
    }


    public void StartListen() {

        while (_isListen) {
            if (_listener.Pending()) {
                Socket socket = _listener.AcceptSocket();
                //socket.ReceiveTimeout = 10000;
                _connectionList.Add(new ConnectedClient(socket));

                Debug.LogError("Socket Type" + socket.SocketType);

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
                    _connectionList.RemoveAt(i);
                    Debug.LogError("Socket disconnected\r\n");
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

