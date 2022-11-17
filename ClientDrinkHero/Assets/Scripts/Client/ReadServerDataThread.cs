using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using UnityEngine;

public class ReadServerDataThread {

    public static bool KeepRunning;

    private StreamReader _reader;
    private NetworkStream _stream;

    public ReadServerDataThread(StreamReader reader, NetworkStream stream) {
        KeepRunning = true;

        _reader = reader;
        _stream = stream;


    }


    public void ThreadLoop() {

        string readData = "";
        while (KeepRunning) {



            if (GlobalGameInfos.writeServerDataTo.Count == 0 || _stream.DataAvailable == false) {
                Thread.Sleep(10);
                continue;
            }


            char[] buffer = new char[1024];
            int readCount;
            try {
                readCount = _reader.Read(buffer, 0, 1024);
            }
            catch {
                KeepRunning = false;
                GlobalGameInfos.Instance.StopServerConnection();
                return;
            }


            if (readCount > 0) {
                readData = readData + new string(buffer, 0, readCount);
            }
            if (TransmissionControl.CheckHeartBeat(readData, out readData)) {
                GlobalGameInfos._TimeoutTime = GlobalGameInfos._TimeoutBaseTime;
            }

            string message = TransmissionControl.GetMessageObject(readData, out readData);
            if (message == "" || message == null) {
                if (TransmissionControl.CheckIfDataIsEmpty(readData, out readData)) {
                    break;
                }
            }
            else {
                bool? isCommand = TransmissionControl.IsCommandMessage(message);

                if (isCommand != null && isCommand == false) {
                    WriteBackData writeBackData = GlobalGameInfos.writeServerDataTo.Dequeue();

                    GetDataByTypeAndSendBack(writeBackData, message);

                    //List<object> list = TransmissionControl.GetObjectData(message, writeBackData.type);
                }


            }

            Thread.Sleep(10);
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
