using System;
using System.Collections.Generic;

public class UnlockedHeroesPreviewHandler
{
    private bool _dataIsLoading;
    private int _requestId;
    
    public List<HeroToUserDatabase> UnlockedHeros { get; private set; }
    public event Action LoadingFinished;

    public UnlockedHeroesPreviewHandler()
    {
        _dataIsLoading = false;
        UnlockedHeros = new List<HeroToUserDatabase>();
        _requestId = 0;
    }

    public void RequestData()
    {
        var request = ClientFunctions.GetUserToHeroByLoggedInUser();

        _requestId = HandleRequests.Instance.HandleRequest(request, typeof(HeroToUserDatabase));
        UnlockedHeros = null;
        _dataIsLoading = true;
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
    }
    
    private bool LoadHeroData()
    {
        var check = true;

        if (UnlockedHeros.Count != 0)
            foreach (var unlockedHero in UnlockedHeros)
            {
                unlockedHero.RequestLoadReferenzData();
                check = check & (unlockedHero.WaitingOnDataCount == 0);

                if (unlockedHero.WaitingOnDataCount == 0)
                {
                    unlockedHero.Hero.RequestLoadReferenzData();
                    check = check & (unlockedHero.Hero.WaitingOnDataCount == 0);
                    if (unlockedHero.Hero.WaitingOnDataCount == 0)
                        foreach (var card in unlockedHero.Hero.CardList)
                        {
                            card.RequestLoadReferenzData();
                            check = check & (card.WaitingOnDataCount == 0);
                            if (card.WaitingOnDataCount == 0)
                                foreach (var cardToEffect in card.CardEffectList)
                                {
                                    cardToEffect.RequestLoadReferenzData();
                                    check = check & (cardToEffect.WaitingOnDataCount == 0);
                                }
                        }
                }
            }
        else check = false;

        return check;
    }

    public void Update()
    {
        if (_dataIsLoading == false) return;


        if (HandleRequests.Instance.RequestDataStatus[_requestId] == DataRequestStatusEnum.Recieved)
        {
            var list = HeroToUserDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_requestId]);
            UnlockedHeros = list;
            HandleRequests.Instance.RequestDataStatus[_requestId] = DataRequestStatusEnum.RecievedAccepted;
        }

        if (UnlockedHeros != null)
            if (LoadHeroData())
            {
                _dataIsLoading = false;
                LoadingFinished?.Invoke();
                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();
            }
    }
}