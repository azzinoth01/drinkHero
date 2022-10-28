using System;
using System.Buffers.Binary;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ConnectedClient {


    private Socket _connection;
    private float _timeoutCheck;
    private DateTime _lastTime;
    private double _deltaTime;
    public string _recievedData;
    private int _byteSizeOfLastData;
    public bool finisehdRecievData;
    private byte[] _readData;
    private int _readPos;
    private int _copyPos;

    public ConnectedClient(Socket socket) {

        _connection = socket;
        _recievedData = "";
        finisehdRecievData = true;
        _readPos = 0;
        _copyPos = 0;
        _byteSizeOfLastData = 0;
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
            _readPos = 0;
            while (_readPos < bytesReceived) {
                if (_byteSizeOfLastData <= 0) {
                    byte[] toint = new byte[4];
                    Buffer.BlockCopy(receive, _readPos, toint, 0, 4);
                    _readPos = _readPos + 4;
                    _byteSizeOfLastData = BinaryPrimitives.ReadInt32BigEndian(toint);

                    _readData = new byte[_byteSizeOfLastData];
                    _copyPos = 0;
                }

                int readbytes = 0;
                if (_byteSizeOfLastData < (bytesReceived - _readPos)) {
                    readbytes = _byteSizeOfLastData;
                }
                else {
                    readbytes = bytesReceived - _readPos;
                }
                Buffer.BlockCopy(receive, _readPos, _readData, _copyPos, readbytes);
                _byteSizeOfLastData = _byteSizeOfLastData - readbytes;
                _copyPos = _copyPos + readbytes;

                _readPos = _readPos + readbytes;


                if (_byteSizeOfLastData <= 0) {

                    string sBuffer = Encoding.UTF8.GetString(_readData, 0, _readData.Length);

                    Packet packet = JsonUtility.FromJson<Packet>(sBuffer);

                    if (packet.ClassName == typeof(Card).ToString()) {
                        Card card = JsonUtility.FromJson<Card>(packet.Data);
                        string cardText = "Card played \r\n" + "Costs " + card.Costs + "\r\n" + "Attack " + card.Attack + "\r\n" + "Schild " + card.Shield + "\r\n" + "Health " + card.Health + "\r\n";
                        Debug.LogError(cardText);
                    }
                    else if (packet.ClassName == typeof(EnemySkill).ToString()) {
                        EnemySkill skill = JsonUtility.FromJson<EnemySkill>(packet.Data);
                        string enemySkill = "Enemy Turn \r\n" + "Attack " + skill.MinAttack + "\r\n" + "Schild " + skill.MinSchield + "\r\n" + "Health " + skill.MinHealth + "\r\n";
                        Debug.LogError(enemySkill);
                    }
                    else if (packet.ClassName == typeof(string).ToString()) {
                        string s = packet.Data;
                        Debug.LogError(s);
                    }
                }
            }





            //while (bytesReceived > 0) {
            //    string sBuffer;
            //    int readpos = 0;
            //    if (_byteSizeOfLastData <= 0) {
            //        byte[] toint = new byte[4];
            //        System.Buffer.BlockCopy(receive, readpos, toint, 0, 4);
            //        _byteSizeOfLastData = BinaryPrimitives.ReadInt32BigEndian(toint);
            //        bytesReceived = bytesReceived - 4;
            //        readpos = readpos + 4;


            //        if (_byteSizeOfLastData >= bytesReceived) {
            //            sBuffer = Encoding.UTF8.GetString(receive, readpos, bytesReceived);
            //            _byteSizeOfLastData = _byteSizeOfLastData - bytesReceived;
            //            bytesReceived = bytesReceived - bytesReceived;
            //        }
            //        else if (_byteSizeOfLastData < bytesReceived) {
            //            sBuffer = Encoding.UTF8.GetString(receive, readpos, _byteSizeOfLastData);
            //            bytesReceived = bytesReceived - _byteSizeOfLastData;
            //            readpos = readpos + _byteSizeOfLastData;
            //            _byteSizeOfLastData = _byteSizeOfLastData - _byteSizeOfLastData;

            //        }
            //    }
            //}

            //if (finisehdRecievData == true) {
            //    byte[] toint = new byte[4];
            //    System.Buffer.BlockCopy(receive, 0, toint, 0, 4);
            //    _byteSizeOfLastData = BinaryPrimitives.ReadInt32BigEndian(toint);

            //    //byteSizeOfLastData = BitConverter.ToInt32(receive, 0);
            //    sBuffer = Encoding.UTF8.GetString(receive, 4, _byteSizeOfLastData);

            //    _byteSizeOfLastData = _byteSizeOfLastData - (bytesReceived - 4);
            //    if (0 >= _byteSizeOfLastData) {
            //        finisehdRecievData = true;
            //    }
            //    else {

            //        finisehdRecievData = false;
            //    }
            //}
            //else {
            //    sBuffer = Encoding.UTF8.GetString(receive, 0, bytesReceived);
            //    _byteSizeOfLastData = _byteSizeOfLastData - bytesReceived;
            //    if (0 >= _byteSizeOfLastData) {
            //        finisehdRecievData = true;
            //    }

            //}
            //_recievedData = _recievedData + sBuffer;

            //if (finisehdRecievData == true) {
            //    Packet packet = JsonUtility.FromJson<Packet>(_recievedData);
            //    _recievedData = "";

            //    if (packet.ClassName == typeof(Card).ToString()) {
            //        Card card = JsonUtility.FromJson<Card>(packet.Data);
            //        string cardText = "Card played \r\n" + "Costs " + card.Costs + "\r\n" + "Attack " + card.Attack + "\r\n" + "Schild " + card.Schild + "\r\n" + "Health " + card.Health + "\r\n";
            //        Debug.LogError(cardText);
            //    }
            //    else if (packet.ClassName == typeof(EnemySkill).ToString()) {
            //        EnemySkill skill = JsonUtility.FromJson<EnemySkill>(packet.Data);
            //        string enemySkill = "Enemy Turn \r\n" + "Attack " + skill.MinAttack + "\r\n" + "Schild " + skill.MinSchild + "\r\n" + "Health " + skill.MinHealth + "\r\n";
            //    }
            //    else if (packet.ClassName == typeof(string).ToString()) {
            //        string s = packet.Data;
            //        Debug.LogError(s);
            //    }
            //}

            //if (sBuffer == "Start") {
            //    Debug.LogError(sBuffer + "\r\n");
            //    return;
            //}













        }
    }

}