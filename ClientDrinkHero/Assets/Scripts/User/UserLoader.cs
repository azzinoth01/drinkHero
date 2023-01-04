using System;
using System.Collections.Generic;

public class UserLoader {
    public int requestId;
    public UserDatabase user;
    public bool loadData;


    public event Action LoadingFinished;

    public UserLoader() {

        loadData = false;
        user = null;
        requestId = 0;

    }
    public void RequestData() {
        UserSave save = UserSave.LoadSave();
        string request;
        if (save.Id == -1) {
            request = ClientFunctions.CreateNewUser();
        }
        else {
            request = ClientFunctions.GetUserByKeyPair("ID\"" + save.Id + "\"");
        }


        requestId = HandleRequests.Instance.HandleRequest(request, typeof(UserDatabase));
        user = null;
        loadData = true;

        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
    }

    private bool LoadUserData() {
        bool check = true;
        user.RequestLoadReferenzData();
        check = check & user.WaitingOnDataCount == 0;
        if (user.WaitingOnDataCount == 0) {
            foreach (HeroToUserDatabase heroToUser in user.HeroDatabasesList) {
                heroToUser.RequestLoadReferenzData();
                check = check & heroToUser.WaitingOnDataCount == 0;

            }
        }




        return check;
    }

    public void Update() {
        if (loadData == false) {
            return;
        }


        if (HandleRequests.Instance.RequestDataStatus[requestId] == DataRequestStatusEnum.Recieved) {

            List<UserDatabase> list = UserDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[requestId]);

            user = list[0];
            HandleRequests.Instance.RequestDataStatus[requestId] = DataRequestStatusEnum.RecievedAccepted;

            UserSave save = new UserSave();
            save.Id = user.Id;
            save.SaveUserID();
        }
        if (user != null) {
            if (LoadUserData()) {
                loadData = false;
                LoadingFinished?.Invoke();
                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();
            }
        }



    }
}
