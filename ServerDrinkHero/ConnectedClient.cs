
using System.Net.Sockets;
using System.Text;

public class ConnectedClient {


    private Socket _connection;
    private double _timeoutCheck;
    private double _timeoutCheckIntervall;

    private DateTime _lastTime;
    private double _deltaTime;
    public string _recievedData;
    public string RemoteIp;



    private NetworkStream _stream;
    private StreamReader _streamReader;
    private StreamWriter _streamWriter;

    public ConnectedClient(Socket socket) {

        _connection = socket;
        RemoteIp = _connection.RemoteEndPoint.ToString();
        _recievedData = "";


        _stream = new NetworkStream(_connection);
        _streamReader = new StreamReader(_stream, Encoding.UTF8, false, 1024);
        _streamWriter = new StreamWriter(_stream, Encoding.UTF8, 1024);

        _streamWriter.AutoFlush = true;

        _deltaTime = 0;
        _lastTime = DateTime.Now;

        _timeoutCheckIntervall = 3000;
        _timeoutCheck = _timeoutCheckIntervall;


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

        if (_timeoutCheck <= 0) {
            _timeoutCheck = _timeoutCheckIntervall;
            try {
                _streamWriter.Write("KEEPALIVE ");
                LogManager.LogQueue.Enqueue("[" + DateTime.Now.ToString() + "] (" + RemoteIp + ") {SEND} KEEPALIVE\r\n");
            }
            catch {
                CloseConnection();
                return;
            }


        }


        char[] buffer = new char[1024];
        int readCount = 0;
        try {
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
        }
        catch {
            CloseConnection();
            return;
        }





        if (readCount > 0) {

            _recievedData = _recievedData + new string(buffer, 0, readCount);

        }

        if (_recievedData == "" || _recievedData == null) {
            return;
        }

        if (TransmissionControl.CheckHeartBeat(_recievedData, out _recievedData)) {

            LogManager.LogQueue.Enqueue("[" + DateTime.Now.ToString() + "] (" + RemoteIp + ") {RECIEVED} KEEPALIVE\r\n");
            Console.WriteLine("[" + DateTime.Now.ToString() + "] (" + RemoteIp + ") {RECIEVED} KEEPALIVE\r\n");
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
            //Console.Write(message);
            LogManager.LogQueue.Enqueue("[" + DateTime.Now.ToString() + "] (" + RemoteIp + ") {RECIEVED} " + message + "\r\n");
            Console.WriteLine("[" + DateTime.Now.ToString() + "] (" + RemoteIp + ") {RECIEVED} " + message + "\r\n");
            string log = TransmissionControl.CommandMessage(_streamWriter, message);
            //Console.Write(log);
            LogManager.LogQueue.Enqueue("[" + DateTime.Now.ToString() + "] (" + RemoteIp + ") {SEND} " + log + "\r\n");
            Console.WriteLine("[" + DateTime.Now.ToString() + "] (" + RemoteIp + ") {SEND} " + log + "\r\n");
        }
        else {
            //check what data type is to be recieved

            //TransmissionControl.GetObjectData<HeroDatabase>(message);
        }


    }

}