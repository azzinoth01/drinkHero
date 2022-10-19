using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ConnectedClient {


    private Socket _connection;
    private float _timeoutCheck;
    private DateTime _lastTime;
    private double _deltaTime;
    public string _recievedData;



    public ConnectedClient(Socket socket) {

        _connection = socket;
        _recievedData = "";

    }

    public Socket Connection {
        get {
            return _connection;
        }


    }

    public void ReadConnection() {
        //DateTime currentTime = DateTime.Now;
        //_deltaTime = (currentTime - _lastTime).TotalMilliseconds;
        //_lastTime = currentTime;

        Byte[] receive = new byte[1024];

        int bytesReceived = _connection.Receive(receive, 0, receive.Length, 0);




        if (bytesReceived == 0) {
            //_timeoutCheck = _timeoutCheck + (float)_deltaTime;
            //if (_connection.ReceiveTimeout < _timeoutCheck) {
            //    _connection.Close();
            //}
        }
        else {



            string sBuffer = Encoding.UTF8.GetString(receive, 0, bytesReceived);

            if (sBuffer == "Start") {
                Debug.LogError(sBuffer + "\r\n");
                return;
            }


            _recievedData = _recievedData + sBuffer;


            Card card = JsonUtility.FromJson<Card>(_recievedData);
            _recievedData = "";

            string cardText = "Card played \r\n" + "Costs " + card.Costs + "\r\n" + "Attack " + card.Attack + "\r\n" + "Schild " + card.Schild + "\r\n" + "Health " + card.Health + "\r\n";


            Debug.LogError(cardText);





        }
    }

}