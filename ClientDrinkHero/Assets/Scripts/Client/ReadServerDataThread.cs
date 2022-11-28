using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ReadServerDataThread {

    public static bool KeepRunning;
    private TcpClient _client;
    private StreamReader _reader;
    private NetworkStream _stream;
    private StreamWriter _writer;

    private double _timeoutBaseTime;

    private double _timeoutCheck;
    private double _deltaTime;
    private DateTime _lastTime;

    public ReadServerDataThread(string host, int port, int timeout) {

        ServerRequests.checkUpdates = new Queue<bool>();
        ServerRequests.writeServerDataToHandleRequests = new Queue<(int, Type)>();
        ServerRequests.serverRequestQueue = new Queue<string>();

        KeepRunning = true;


        int connectTrys = 5;



        for (int i = 0; i < connectTrys;) {
            try {
                _client = new TcpClient(host, port);
                _stream = _client.GetStream();
                _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024);
                _writer = new StreamWriter(_stream, Encoding.UTF8, 1024);
                _writer.AutoFlush = true;
                if (_client.Connected) {
                    Debug.Log("Connection success");
                    break;
                }
            }
            catch {

            }
            Debug.Log("Connection failed trying again");
            i = i + 1;
        }




        string request = ClientFunctions.SendMessageToDatabase("Start");

        ServerRequests.serverRequestQueue.Enqueue(request);

        while (ServerRequests.serverRequestQueue.Count != 0) {
            string call = ServerRequests.serverRequestQueue.Dequeue();
            Debug.Log("write to server " + call);
            try {
                _writer.Write(call);
            }
            catch (Exception e) {
                Debug.LogError(e.Message);
                KeepRunning = false;
                CloseConnection();
                return;
            }

        }

        _timeoutBaseTime = timeout;
        _timeoutCheck = timeout;

        _lastTime = DateTime.Now;
    }
    private void CloseConnection() {

        try {
            _writer.Close();
        }
        catch {

        }
        try {
            _reader.Close();
        }
        catch {

        }
        try {
            _stream.Close();
        }
        catch {

        }
        try {
            _client.Close();
        }
        catch {

        }
    }

    public void ThreadLoop() {
        Debug.Log("Data Reading started");
        string readData = "";
        while (KeepRunning) {

            DateTime now = DateTime.Now;

            _deltaTime = (now - _lastTime).TotalMilliseconds;
            _lastTime = now;

            _timeoutCheck = _timeoutCheck - _deltaTime;


            if (_timeoutCheck <= 0) {
                _timeoutCheck = _timeoutBaseTime;
                ClientFunctions.SendHeartbeat();
            }



            while (ServerRequests.serverRequestQueue.Count != 0) {
                string call = ServerRequests.serverRequestQueue.Dequeue();
                Debug.Log("write to server " + call);
                try {
                    _writer.Write(call);
                    //Debug.Log(call);
                }
                catch (Exception e) {
                    Debug.LogError(e.Message);
                    KeepRunning = false;
                    CloseConnection();
                    return;
                }

            }


            try {
                if (_stream.DataAvailable == false) {
                    Thread.Sleep(10);
                    continue;
                }
            }
            catch (Exception e) {
                KeepRunning = false;
                CloseConnection();
                Debug.Log(e.Message);
                return;
            }




            char[] buffer = new char[1024];
            int readCount;
            try {
                readCount = _reader.Read(buffer, 0, 1024);
            }
            catch (Exception e) {
                KeepRunning = false;
                CloseConnection();
                Debug.Log(e.Message);
                return;
            }


            if (readCount > 0) {
                readData = readData + new string(buffer, 0, readCount);
            }


            while (readData != "") {

                TransmissionControl.CheckHeartBeat(readData, out readData);
                string message = TransmissionControl.GetMessageObject(readData, out readData);
                Debug.Log(message);
                if (message == "" || message == null) {
                    if (TransmissionControl.CheckIfDataIsEmpty(readData, out readData)) {

                        continue;
                    }
                    else {
                        break;
                    }

                }
                else {

                    bool? isCommand = TransmissionControl.IsCommandMessage(message);

                    if (isCommand != null && isCommand == false) {
                        (int, Type) writeBackData = ServerRequests.writeServerDataToHandleRequests.Dequeue();

                        GetDataByTypeAndSendBack(writeBackData.Item1, writeBackData.Item2, message);


                    }


                }

            }

            Thread.Sleep(1);
        }
    }


    private void GetDataByTypeAndSendBack(int id, Type type, string message) {


        HandleRequests.Instance.RequestData[id] = message;
        HandleRequests.Instance.RequestDataStatus[id] = DataRequestStatusEnum.Recieved;
        ServerRequests.checkUpdates.Enqueue(true);

        //if (type == typeof(HeroDatabase)) {
        //    List<HeroDatabase> list = TransmissionControl.GetObjectData<HeroDatabase>(message);
        //    List<IRequestDataFromServer> writeBackList = new List<IRequestDataFromServer>();

        //    foreach (HeroDatabase item in list) {
        //        writeBackList.Add(item);
        //    }
        //    HandleRequests.Instance.RequestData[id] = writeBackList;
        //    HandleRequests.Instance.RequestDataStatus[id] = DataRequestStatusEnum.Recieved;
        //    ServerRequests.checkUpdates.Enqueue(true);
        //}
        //else if (type == typeof(CardDatabase)) {
        //    List<CardDatabase> list = TransmissionControl.GetObjectData<CardDatabase>(message);
        //    List<IRequestDataFromServer> writeBackList = new List<IRequestDataFromServer>();

        //    foreach (CardDatabase item in list) {
        //        writeBackList.Add(item);
        //    }
        //    HandleRequests.Instance.RequestData[id] = writeBackList;
        //    HandleRequests.Instance.RequestDataStatus[id] = DataRequestStatusEnum.Recieved;
        //    ServerRequests.checkUpdates.Enqueue(true);
        //}
        //else if (type == typeof(CardToHero)) {
        //    List<CardToHero> list = TransmissionControl.GetObjectData<CardToHero>(message);
        //    List<IRequestDataFromServer> writeBackList = new List<IRequestDataFromServer>();

        //    foreach (CardToHero item in list) {
        //        writeBackList.Add(item);
        //    }
        //    HandleRequests.Instance.RequestData[id] = writeBackList;
        //    HandleRequests.Instance.RequestDataStatus[id] = DataRequestStatusEnum.Recieved;
        //    ServerRequests.checkUpdates.Enqueue(true);
        //}
        //else if (type == typeof(EnemyDatabase)) {
        //    List<EnemyDatabase> list = TransmissionControl.GetObjectData<EnemyDatabase>(message);
        //    List<IRequestDataFromServer> writeBackList = new List<IRequestDataFromServer>();

        //    foreach (EnemyDatabase item in list) {
        //        writeBackList.Add(item);
        //    }
        //    HandleRequests.Instance.RequestData[id] = writeBackList;
        //    HandleRequests.Instance.RequestDataStatus[id] = DataRequestStatusEnum.Recieved;
        //    ServerRequests.checkUpdates.Enqueue(true);
        //}
        //else if (type == typeof(EnemyToEnemySkill)) {
        //    List<EnemyToEnemySkill> list = TransmissionControl.GetObjectData<EnemyToEnemySkill>(message);
        //    List<IRequestDataFromServer> writeBackList = new List<IRequestDataFromServer>();

        //    foreach (EnemyToEnemySkill item in list) {
        //        writeBackList.Add(item);
        //    }
        //    HandleRequests.Instance.RequestData[id] = writeBackList;
        //    HandleRequests.Instance.RequestDataStatus[id] = DataRequestStatusEnum.Recieved;
        //    ServerRequests.checkUpdates.Enqueue(true);
        //}
        //else if (type == typeof(EnemySkillDatabase)) {
        //    List<EnemySkillDatabase> list = TransmissionControl.GetObjectData<EnemySkillDatabase>(message);
        //    List<IRequestDataFromServer> writeBackList = new List<IRequestDataFromServer>();

        //    foreach (EnemySkillDatabase item in list) {
        //        writeBackList.Add(item);
        //    }
        //    HandleRequests.Instance.RequestData[id] = writeBackList;
        //    HandleRequests.Instance.RequestDataStatus[id] = DataRequestStatusEnum.Recieved;
        //    ServerRequests.checkUpdates.Enqueue(true);
        //}



    }
}
