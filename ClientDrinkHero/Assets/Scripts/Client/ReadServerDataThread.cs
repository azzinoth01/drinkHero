using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
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
    private double _timeoutTime;
    private double _timeoutCheck;
    private double _deltaTime;
    private DateTime _lastTime;

    public ReadServerDataThread(string host, int port, int timeout) {
        KeepRunning = true;

        _timeoutBaseTime = timeout;
        _timeoutCheck = timeout;
        _timeoutTime = timeout * 2;
        _lastTime = DateTime.Now;

        _client = new TcpClient(host, port);
        _stream = _client.GetStream();
        _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024);
        _writer = new StreamWriter(_stream, Encoding.UTF8, 1024);
        _writer.AutoFlush = true;

        ClientFunctions.SendMessageToDatabase("Start");

        while (GlobalGameInfos.serverRequestQueue.Count != 0) {
            string call = GlobalGameInfos.serverRequestQueue.Dequeue();

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

        string readData = "";
        while (KeepRunning) {

            DateTime now = DateTime.Now;

            _deltaTime = (now - _lastTime).TotalMilliseconds;
            _lastTime = now;

            _timeoutCheck = _timeoutCheck - _deltaTime;
            _timeoutTime = _timeoutTime - _deltaTime;

            if (_timeoutCheck <= 0) {
                _timeoutCheck = _timeoutBaseTime;
                ClientFunctions.SendHeartbeat();
            }
            if (_timeoutTime <= 0) {

                KeepRunning = false;
                CloseConnection();
                return;

            }

            while (GlobalGameInfos.serverRequestQueue.Count != 0) {
                string call = GlobalGameInfos.serverRequestQueue.Dequeue();

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


            try {
                if (GlobalGameInfos.writeServerDataTo.Count == 0 || _stream.DataAvailable == false) {
                    Thread.Sleep(10);
                    continue;
                }
            }
            catch {
                KeepRunning = false;
                CloseConnection();
                return;
            }




            char[] buffer = new char[1024];
            int readCount;
            try {
                readCount = _reader.Read(buffer, 0, 1024);
            }
            catch {
                KeepRunning = false;
                CloseConnection();
                return;
            }


            if (readCount > 0) {
                readData = readData + new string(buffer, 0, readCount);
            }
            //Debug.Log(readData);

            if (TransmissionControl.CheckHeartBeat(readData, out readData)) {
                _timeoutTime = _timeoutBaseTime * 2;
            }
            while (readData != "") {
                string message = TransmissionControl.GetMessageObject(readData, out readData);
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
                        WriteBackData writeBackData = GlobalGameInfos.writeServerDataTo.Dequeue();

                        GetDataByTypeAndSendBack(writeBackData, message);


                    }


                }

            }

            Thread.Sleep(1);
        }
    }


    private void GetDataByTypeAndSendBack(WriteBackData writeBackData, string message) {
        if (writeBackData.type == typeof(HeroDatabase)) {
            List<HeroDatabase> list = TransmissionControl.GetObjectData<HeroDatabase>(message);
            object[] obj = new object[1];
            obj[0] = list;
            writeBackData.method.Invoke(writeBackData.instance, obj);
            GlobalGameInfos.checkUpdates.Enqueue(true);
        }
        else if (writeBackData.type == typeof(CardDatabase)) {
            List<CardDatabase> list = TransmissionControl.GetObjectData<CardDatabase>(message);
            object[] obj = new object[1];
            obj[0] = list;
            writeBackData.method.Invoke(writeBackData.instance, obj);
            GlobalGameInfos.checkUpdates.Enqueue(true);
        }
        else if (writeBackData.type == typeof(CardToHero)) {
            List<CardToHero> list = TransmissionControl.GetObjectData<CardToHero>(message);
            object[] obj = new object[1];
            obj[0] = list;
            writeBackData.method.Invoke(writeBackData.instance, obj);
            GlobalGameInfos.checkUpdates.Enqueue(true);
        }
        else if (writeBackData.type == typeof(EnemyDatabase)) {
            List<EnemyDatabase> list = TransmissionControl.GetObjectData<EnemyDatabase>(message);
            object[] obj = new object[1];
            obj[0] = list;
            writeBackData.method.Invoke(writeBackData.instance, obj);
            GlobalGameInfos.checkUpdates.Enqueue(true);
        }
        else if (writeBackData.type == typeof(EnemyToEnemySkill)) {
            List<EnemyToEnemySkill> list = TransmissionControl.GetObjectData<EnemyToEnemySkill>(message);
            object[] obj = new object[1];
            obj[0] = list;
            writeBackData.method.Invoke(writeBackData.instance, obj);
            GlobalGameInfos.checkUpdates.Enqueue(true);
        }
        else if (writeBackData.type == typeof(EnemySkillDatabase)) {
            List<EnemySkillDatabase> list = TransmissionControl.GetObjectData<EnemySkillDatabase>(message);
            object[] obj = new object[1];
            obj[0] = list;
            writeBackData.method.Invoke(writeBackData.instance, obj);
            GlobalGameInfos.checkUpdates.Enqueue(true);
        }



    }
}
