using System;
using System.Collections.Generic;

public class EnemyListHandler {
    public int requestId;
    public List<EnemyDatabase> _enemyList;
    public bool loadData;


    public event Action LoadingFinished;

    public EnemyListHandler() {

        loadData = false;
        _enemyList = null;
        requestId = 0;

    }


    public void RequestData() {

        string request = ClientFunctions.GetRandomNormalEnemyDatabase(3);

        requestId = HandleRequests.Instance.HandleRequest(request, typeof(EnemyDatabase));
        _enemyList = null;
        loadData = true;
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
    }

    private bool LoadEnemyData() {
        bool check = true;
        foreach (EnemyDatabase enemy in _enemyList) {
            enemy.RequestLoadReferenzData();
            check = check & enemy.WaitingOnDataCount == 0;
            if (enemy.WaitingOnDataCount == 0) {
                foreach (EnemyToEnemySkill enemyToEnemySkill in enemy.EnemyToEnemySkills) {
                    enemyToEnemySkill.RequestLoadReferenzData();
                    check = check & enemyToEnemySkill.WaitingOnDataCount == 0;
                }
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

            _enemyList = list;
            HandleRequests.Instance.RequestDataStatus[requestId] = DataRequestStatusEnum.RecievedAccepted;
        }
        if (_enemyList != null) {
            if (LoadEnemyData()) {
                loadData = false;
                LoadingFinished?.Invoke();
                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();
            }
        }



    }

}
