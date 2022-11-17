using System;

using System.IO;
using System.Net.Sockets;
using System.Text;

public class ConnectedClient {


    private Socket _connection;
    private double _timeoutCheck;
    private double _timeoutCheckIntervall;
    private double _timeoutReached;
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

        _deltaTime = 0;
        _lastTime = DateTime.Now;

        _timeoutCheckIntervall = 3000;
        _timeoutCheck = _timeoutCheckIntervall;
        _timeoutReached = _timeoutCheckIntervall * 2;
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

        DateTime now = DateTime.Now;

        _deltaTime = (now - _lastTime).TotalMilliseconds;
        _lastTime = now;

        _timeoutCheck = _timeoutCheck - _deltaTime;
        _timeoutReached = _timeoutReached - _deltaTime;
        if (_timeoutCheck <= 0) {
            _timeoutCheck = _timeoutCheckIntervall;
            try {
                _streamWriter.Write("KEEPALIVE ");
            }
            catch {
                CloseConnection();
                return;
            }
            Console.WriteLine("Write KEEPALIVE");
        }

        if (_timeoutReached <= 0) {
            CloseConnection();
            return;
        }


        char[] buffer = new char[1024];
        int readCount = 0;

        if (_stream.DataAvailable == false) {
            //Console.Write("Can not Read\r\n");
        }
        else {
            try {
                readCount = _streamReader.Read(buffer, 0, 1024);
            }
            catch {
                CloseConnection();
                return;
            }
        }



        if (readCount > 0) {

            _recievedData = _recievedData + new string(buffer, 0, readCount);

        }

        if (_recievedData == "" || _recievedData == null) {
            return;
        }

        if (TransmissionControl.CheckHeartBeat(_recievedData, out _recievedData)) {
            _timeoutReached = _timeoutCheckIntervall * 2;
            Console.WriteLine("Recieved KEEPALIVE");
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
            Console.Write(message + "\r\n");
            TransmissionControl.CommandMessage(_streamWriter, message);

        }
        else {
            //check what data type is to be recieved

            //TransmissionControl.GetObjectData<HeroDatabase>(message);
        }


    }

}