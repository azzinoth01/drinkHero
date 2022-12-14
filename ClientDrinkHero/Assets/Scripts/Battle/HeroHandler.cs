using System;
using System.Collections.Generic;

public class HeroHandler {

    private int _requestId;
    private List<HeroDatabase> _heros;

    private bool _dataIsLoading;

    public List<HeroDatabase> Heros {
        get {
            return _heros;
        }

    }

    public event Action LoadingFinished;

    public HeroHandler() {

        _dataIsLoading = false;
        _heros = new List<HeroDatabase>();
        _requestId = 0;

    }


    public void RequestData() {

        string request = ClientFunctions.GetHeroDatabase();

        _requestId = HandleRequests.Instance.HandleRequest(request, typeof(HeroDatabase));
        _heros = null;
        _dataIsLoading = true;
    }

    private bool LoadHeroData() {
        bool check = true;
        foreach (HeroDatabase hero in HeroDatabase._cachedData.Values) {
            hero.RequestLoadReferenzData();
            check = check & hero.WaitingOnDataCount == 0;
            if (hero.WaitingOnDataCount == 0) {
                foreach (CardToHero cardToHero in hero.CardList) {
                    cardToHero.RequestLoadReferenzData();
                    check = check & cardToHero.WaitingOnDataCount == 0;
                    if (cardToHero.WaitingOnDataCount == 0) {
                        cardToHero.Card.RequestLoadReferenzData();
                        check = check & cardToHero.Card.WaitingOnDataCount == 0;
                        if (cardToHero.Card.WaitingOnDataCount == 0) {
                            foreach (CardToEffect cardToEffect in cardToHero.Card.CardEffectList) {
                                cardToEffect.RequestLoadReferenzData();
                                check = check & cardToEffect.WaitingOnDataCount == 0;
                            }
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

            _heros = list;
            HandleRequests.Instance.RequestDataStatus[_requestId] = DataRequestStatusEnum.RecievedAccepted;
        }
        if (_heros != null) {
            if (LoadHeroData()) {
                _dataIsLoading = false;
                LoadingFinished?.Invoke();
            }
        }



    }
}
