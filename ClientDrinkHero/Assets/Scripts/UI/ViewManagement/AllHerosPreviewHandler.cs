using System;
using System.Collections.Generic;
using UnityEngine;

public class AllHerosPreviewHandler
{
       
    
    private int _requestId;
    private List<HeroDatabase> _heros;

    private bool _dataIsLoading;

    public List<HeroDatabase> Heros {
        get {
            return _heros;
        }

    }

    public event Action LoadingFinished;

    public AllHerosPreviewHandler() {

        _dataIsLoading = false;
        _heros = new List<HeroDatabase>();
        _requestId = 0;

    }


    public void RequestData() {

        string request = ClientFunctions.GetHeroDatabase();

        _requestId = HandleRequests.Instance.HandleRequest(request, typeof(HeroDatabase));
        _heros = null;
        _dataIsLoading = true;
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
    }
    

    public void Update() {
        if (_dataIsLoading == false) {
            return;
        }
        
        if (HandleRequests.Instance.RequestDataStatus[_requestId] == DataRequestStatusEnum.Recieved) {

            List<HeroDatabase> list = HeroDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_requestId]);

            _heros = list;
            HandleRequests.Instance.RequestDataStatus[_requestId] = DataRequestStatusEnum.RecievedAccepted;
            
            _dataIsLoading = false;
            LoadingFinished?.Invoke();
            Debug.Log("AllHerosPreviewHandler finished!");
            NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();
        }
    }
}
