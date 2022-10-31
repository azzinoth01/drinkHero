using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class DrinkHeroServer {


    private TcpListener _listener;
    private int _port = 6969;

    private List<ConnectedClient> _connectionList;



    public void StartServer() {
        _connectionList = new List<ConnectedClient>();
        _listener = new TcpListener(IPAddress.Any, _port);
        _listener.Start();



        Thread thread = new Thread(new ThreadStart(StartListen));
        thread.Start();
        Thread listenThread = new Thread(new ThreadStart(ListenToConnections));
        listenThread.Start();
        Debug.LogError("Server Startet");
    }
    public DrinkHeroServer() {

    }




    public void StartListen() {

        while (true) {

            Socket socket = _listener.AcceptSocket();
            //socket.ReceiveTimeout = 10000;
            _connectionList.Add(new ConnectedClient(socket));

            Debug.LogError("Socket Type" + socket.SocketType);



            Thread.Sleep(1);
        }


    }

    public void ListenToConnections() {
        while (true) {
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

