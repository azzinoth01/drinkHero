using System;
using System.Collections.Generic;

public class UserLoader {
    public int requestId;
    public UserDatabase user;
    public bool loadData;
    private bool _userDataOld;


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
            //request = ClientFunctions.GetUserByKeyPair("ID\"" + save.Id + "\"");
            request = ClientFunctions.LoginWithUser(save.Id.ToString());
        }


        requestId = HandleRequests.Instance.HandleRequest(request, typeof(UserDatabase));
        user = null;
        loadData = true;
        _userDataOld = true;
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
    }

    public void RequestData(string request) {
        requestId = HandleRequests.Instance.HandleRequest(request, typeof(UserDatabase));
        loadData = true;
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
        _userDataOld = true;
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
            _userDataOld = false;
        }
        if (user != null && _userDataOld == false) {
            if (LoadUserData()) {
                loadData = false;
                LoadingFinished?.Invoke();
                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();
            }
        }



    }
}
