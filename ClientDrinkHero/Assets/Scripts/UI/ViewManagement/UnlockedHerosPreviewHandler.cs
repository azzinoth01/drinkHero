
    
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedHerosPreviewHandler
{
       
           
    
    private int _requestId;
    private List<HeroToUserDatabase> _unlockedHeros;

    private bool _dataIsLoading;

    public List<HeroToUserDatabase> UnlockedHeros {
        get {
            return _unlockedHeros;
        }

    }

    public event Action LoadingFinished;

    public UnlockedHerosPreviewHandler() {

        _dataIsLoading = false;
        _unlockedHeros = new List<HeroToUserDatabase>();
        _requestId = 0;

    }


    public void RequestData()
    {
        string request = ClientFunctions.GetUserToHeroByLoggedInUser();
        
        _requestId = HandleRequests.Instance.HandleRequest(request, typeof(HeroToUserDatabase));
        _unlockedHeros = null;
        _dataIsLoading = true;
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
    }


    private bool LoadHeroData() {
        bool check = true;

        if (_unlockedHeros.Count != 0)
        {
            foreach (var unlockedHero in _unlockedHeros)
            {
                unlockedHero.RequestLoadReferenzData();
                check = check & unlockedHero.WaitingOnDataCount == 0;

                if (unlockedHero.WaitingOnDataCount == 0)
                {
                    unlockedHero.Hero.RequestLoadReferenzData();
                    check = check & unlockedHero.Hero.WaitingOnDataCount == 0;
                    if (unlockedHero.Hero.WaitingOnDataCount == 0) {
                        foreach (CardDatabase card in unlockedHero.Hero.CardList) {
                            card.RequestLoadReferenzData();
                            check = check & card.WaitingOnDataCount == 0;
                            if (card.WaitingOnDataCount == 0) {
                                foreach (CardToEffect cardToEffect in card.CardEffectList) {
                                    cardToEffect.RequestLoadReferenzData();
                                    check = check & cardToEffect.WaitingOnDataCount == 0;
                                }
                            }
                        }
                    }
                }
            }
        }
        else check = false;

        return check;
    }

    public void Update() {
        if (_dataIsLoading == false) {
            return;
        }


        if (HandleRequests.Instance.RequestDataStatus[_requestId] == DataRequestStatusEnum.Recieved) {
            Debug.LogError("UnlockedHerosPreviewHandler Received List!");
            List<HeroToUserDatabase> list = HeroToUserDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_requestId]);

            _unlockedHeros = list;
            HandleRequests.Instance.RequestDataStatus[_requestId] = DataRequestStatusEnum.RecievedAccepted;
        }
        if (_unlockedHeros != null) {
            if (LoadHeroData()) {
                _dataIsLoading = false;
                LoadingFinished?.Invoke();
                Debug.Log("UnlockedHerosPreviewHandler finished!");
                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();
            }
        }



    }
}
