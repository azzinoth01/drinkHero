using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

public class ConnectedClient {


    private Socket _connection;
    private float _timeoutCheck;
    private DateTime _lastTime;
    private double _deltaTime;
    public string _recievedData;




    private NetworkStream _stream;
    private StreamReader _streamReader;
    private StreamWriter _streamWriter;

    public ConnectedClient(Socket socket) {

        _connection = socket;
        _recievedData = "";


        _stream = new NetworkStream(_connection);
        _streamReader = new StreamReader(_stream, Encoding.UTF8, false, 1024);
        _streamWriter = new StreamWriter(_stream, Encoding.UTF8, 1024);

        _streamWriter.AutoFlush = true;
    }

    public Socket Connection {
        get {
            return _connection;
        }


    }

    public void CloseConnection() {
        _streamWriter.Close();
        _streamReader.Close();
        _stream.Close();
        _connection.Close();
    }

    public void ReadConnection() {

        char[] buffer = new char[1024];
        int readCount = _streamReader.Read(buffer, 0, 1024);

        if (readCount > 0) {

            _recievedData = _recievedData + new string(buffer, 0, readCount);
        }

        if (_recievedData == "" || _recievedData == null) {
            return;
        }
        string message = TransmissionControl.GetMessageObject(_recievedData, out _recievedData);

        if (message == "" || message == null) {
            return;
        }

        bool? isCommand = TransmissionControl.IsCommandMessage(message);

        if (isCommand == null) {
            return;
        }
        else if (isCommand == true) {
            TransmissionControl.CommandMessage(_streamWriter, message);
        }
        else {
            //check what data type is to be recieved

            //TransmissionControl.GetObjectData<HeroDatabase>(message);
        }



        //Byte[] receive = new byte[1024];

        //int bytesReceived = _connection.Receive(receive, 0, receive.Length, 0);



        //if (bytesReceived == 0) {

        //}
        //else {
        //    _readPos = 0;
        //    while (_readPos < bytesReceived) {
        //        if (_byteSizeOfLastData <= 0) {
        //            byte[] toint = new byte[4];
        //            Buffer.BlockCopy(receive, _readPos, toint, 0, 4);
        //            _readPos = _readPos + 4;
        //            _byteSizeOfLastData = BinaryPrimitives.ReadInt32BigEndian(toint);

        //            _readData = new byte[_byteSizeOfLastData];
        //            _copyPos = 0;
        //        }

        //        int readbytes = 0;
        //        if (_byteSizeOfLastData < (bytesReceived - _readPos)) {
        //            readbytes = _byteSizeOfLastData;
        //        }
        //        else {
        //            readbytes = bytesReceived - _readPos;
        //        }
        //        Buffer.BlockCopy(receive, _readPos, _readData, _copyPos, readbytes);
        //        _byteSizeOfLastData = _byteSizeOfLastData - readbytes;
        //        _copyPos = _copyPos + readbytes;

        //        _readPos = _readPos + readbytes;


        //        if (_byteSizeOfLastData <= 0) {

        //            string sBuffer = Encoding.UTF8.GetString(_readData, 0, _readData.Length);

        //            Packet packet = JsonUtility.FromJson<Packet>(sBuffer);

        //            if (packet.ClassName == typeof(Card).ToString()) {
        //                Card card = JsonUtility.FromJson<Card>(packet.Data);
        //                string cardText = "Card played \r\n" + "Costs " + card.Costs + "\r\n" + "Attack " + card.Attack + "\r\n" + "Schild " + card.Shield + "\r\n" + "Health " + card.Health + "\r\n";
        //                Debug.LogError(cardText);
        //            }
        //            else if (packet.ClassName == typeof(EnemySkill).ToString()) {
        //                EnemySkill skill = JsonUtility.FromJson<EnemySkill>(packet.Data);
        //                string enemySkill = "Enemy Turn \r\n" + "Attack " + skill.MinAttack + "\r\n" + "Schild " + skill.MinShield + "\r\n" + "Health " + skill.MinHealth + "\r\n";
        //                Debug.LogError(enemySkill);
        //            }
        //            else if (packet.ClassName == typeof(string).ToString()) {
        //                string s = packet.Data;
        //                Debug.LogError(s);
        //            }
        //        }
        //    }



        //}
    }

}