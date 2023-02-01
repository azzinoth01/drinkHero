using System;
using System.Collections.Generic;
using UnityEngine;

public class AllHeroesPreviewHandler
{
    private bool _dataIsLoading;
    private int _requestId;
    public event Action LoadingFinished;
    public List<HeroDatabase> Heros { get; private set; }

    public AllHeroesPreviewHandler()
    {
        _dataIsLoading = false;
        Heros = new List<HeroDatabase>();
        _requestId = 0;
    }
    
    public void RequestData()
    {
        var request = ClientFunctions.GetHeroDatabase();

        _requestId = HandleRequests.Instance.HandleRequest(request, typeof(HeroDatabase));
        Heros = null;
        _dataIsLoading = true;
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
    }
    
    public void Update()
    {
        if (_dataIsLoading == false) return;

        if (HandleRequests.Instance.RequestDataStatus[_requestId] == DataRequestStatusEnum.Recieved)
        {
            var list = HeroDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_requestId]);

            Heros = list;
            HandleRequests.Instance.RequestDataStatus[_requestId] = DataRequestStatusEnum.RecievedAccepted;

            _dataIsLoading = false;
            LoadingFinished?.Invoke();
            Debug.Log("AllHerosPreviewHandler finished!");
            NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();
        }
    }
}