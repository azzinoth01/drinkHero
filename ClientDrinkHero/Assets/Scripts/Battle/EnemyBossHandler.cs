using System;
using System.Collections.Generic;

public class EnemyBossHandler {



    public int requestId;
    public EnemyDatabase _enemy;

    public bool loadData;


    public event Action LoadingFinished;

    public EnemyBossHandler() {

        loadData = false;
        _enemy = null;
        requestId = 0;

    }


    public void RequestData() {

        string request = ClientFunctions.GetRandomBossEnemyDatabase();

        requestId = HandleRequests.Instance.HandleRequest(request, typeof(EnemyDatabase));
        _enemy = null;
        loadData = true;
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
    }

    private bool LoadEnemyData() {
        bool check = true;

        _enemy.RequestLoadReferenzData();
        check = check & _enemy.WaitingOnDataCount == 0;
        if (_enemy.WaitingOnDataCount == 0) {
            foreach (EnemyToEnemySkill enemyToEnemySkill in _enemy.EnemyToEnemySkills) {
                enemyToEnemySkill.RequestLoadReferenzData();
                check = check & enemyToEnemySkill.WaitingOnDataCount == 0;
            }
        }


        return check;
    }

    public void Update() {
        if (loadData == false) {
            return;
        }


        if (HandleRequests.Instance.RequestDataStatus[requestId] == DataRequestStatusEnum.Recieved) {

            List<EnemyDatabase> list = EnemyDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[requestId]);

            _enemy = list[0];
            HandleRequests.Instance.RequestDataStatus[requestId] = DataRequestStatusEnum.RecievedAccepted;
        }
        if (_enemy != null) {
            if (LoadEnemyData()) {
                loadData = false;
                LoadingFinished?.Invoke();
                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();
            }
        }



    }



}
