using System;
using System.Collections.Generic;

public class HeroHandler {

    private int _requestId;
    private HeroDatabase _heros;

    private bool _dataIsLoading;

    public HeroDatabase Heros {
        get {
            return _heros;
        }

    }

    public event Action LoadingFinished;

    public HeroHandler() {

        _dataIsLoading = false;
        _heros = null;
        _requestId = 0;

    }


    public void RequestData(int index) {

        string request = ClientFunctions.GetHeroDatabaseByKeyPair("ID\"" + index + "\"");

        _requestId = HandleRequests.Instance.HandleRequest(request, typeof(HeroDatabase));
        _heros = null;
        _dataIsLoading = true;
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
    }

    private bool LoadHeroData() {
        bool check = true;
        foreach (HeroDatabase hero in HeroDatabase._cachedData.Values) {
            hero.RequestLoadReferenzData();
            check = check & hero.WaitingOnDataCount == 0;
            if (hero.WaitingOnDataCount == 0) {
                foreach (CardDatabase card in hero.CardList) {
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

        return check;
    }

    public void Update() {
        if (_dataIsLoading == false) {
            return;
        }


        if (HandleRequests.Instance.RequestDataStatus[_requestId] == DataRequestStatusEnum.Recieved) {

            List<HeroDatabase> list = HeroDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_requestId]);

            _heros = list[0];
            HandleRequests.Instance.RequestDataStatus[_requestId] = DataRequestStatusEnum.RecievedAccepted;
        }
        if (_heros != null) {
            if (LoadHeroData()) {
                _dataIsLoading = false;
                LoadingFinished?.Invoke();
                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();
            }
        }



    }
}
