using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CharacterUpgradeTab : MonoBehaviour {
    private int _cardIndex;
    [SerializeField] private CharacterCardTabView _tabView;
    [SerializeField] private CharacterCardPreview _currentCard;
    [SerializeField] private CharacterCardPreview _uppgradeCard;

    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private Button _upgradeButton;



    [SerializeField] private List<LoadSprite> _upgradeItemList;
    [SerializeField] private TextMeshProUGUI _hasValue;
    [SerializeField] private TextMeshProUGUI _costValue;


    private bool _isCardUpgrading;


    private void Awake() {
        _cardIndex = 0;
        _nextButton.onClick.AddListener(NextCardData);
        _previousButton.onClick.AddListener(PreviousCardData);
        _upgradeButton.onClick.AddListener(UpgradeCard);

    }


    private void OnEnable() {

        LoadCardData();

        _isCardUpgrading = false;
    }

    private void LoadCardData() {
        HeroDatabase hero = _tabView.Character;


        CardData data = new CardData();

        data.cost = hero.CardList[_cardIndex].Cost;
        data.description = hero.CardList[_cardIndex].Text;
        data.name = hero.CardList[_cardIndex].Name;

        _currentCard.SetData(data, hero.SpritePath);

        _upgradeButton.interactable = false;
        _costValue.SetText("0");
        _hasValue.SetText("0");

        if (hero.CardList[_cardIndex].RefUpgradeTo != null) {
            data = new CardData();

            data.cost = hero.CardList[_cardIndex].UpgradeTo.Cost;
            data.description = hero.CardList[_cardIndex].UpgradeTo.Text;
            data.name = hero.CardList[_cardIndex].UpgradeTo.Name;

            _upgradeButton.interactable = true;

            _costValue.SetText(hero.CardList[_cardIndex].UpgradeItemAmount.ToString());
        }

        foreach (LoadSprite loadSprite in _upgradeItemList) {
            loadSprite.LoadNewSprite(hero.CardList[_cardIndex].UpgradeItem.SpritePath);
        }
        foreach (UserToUpradeItemDatabase item in UserSingelton.Instance.UserObject.User.UserToUpgradeItemDatabaseList) {
            if (item.RefItem == hero.CardList[_cardIndex].RefUpgradeItem) {
                _hasValue.SetText(item.Amount.ToString());
            }
        }


        _uppgradeCard.SetData(data, hero.SpritePath);
    }

    private void NextCardData() {
        _cardIndex = _cardIndex + 1;

        if (_cardIndex >= _tabView.Character.CardList.Count) {
            _cardIndex = 0;
        }
        LoadCardData();
    }
    private void PreviousCardData() {
        _cardIndex = _cardIndex - 1;

        if (_cardIndex < 0) {
            _cardIndex = _tabView.Character.CardList.Count - 1;
        }
        LoadCardData();
    }
    private void UpgradeCard() {
        string request = ClientFunctions.UpgradeCard("RefHero\"" + _tabView.Character.Id + "\";RefCard\"" + _tabView.Character.CardList[_cardIndex].Id + "\"");
        int _requestId = HandleRequests.Instance.HandleRequest(request, typeof(ResponsMessageObject));
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
        _upgradeButton.interactable = false;

        _nextButton.interactable = false;
        _previousButton.interactable = false;


        StartCoroutine(WaitForUpgradeResponse(_requestId));
    }

    private IEnumerator WaitForUpgradeResponse(int requestId) {

        while (HandleRequests.Instance.RequestDataStatus[requestId] != DataRequestStatusEnum.RecievedAccepted) {
            if (HandleRequests.Instance.RequestDataStatus[requestId] == DataRequestStatusEnum.Recieved) {

                ResponsMessageObject message = ResponsMessageObject.CreateObjectDataFromString(HandleRequests.Instance.RequestData[requestId])[0];

                Debug.Log(message.Message);

                if (message.Message == "SUCCESS") {
                    _tabView.Character.CardList[_cardIndex] = _tabView.Character.CardList[_cardIndex].UpgradeTo;
                    _isCardUpgrading = true;
                }


                HandleRequests.Instance.RequestDataStatus[requestId] = DataRequestStatusEnum.RecievedAccepted;
            }
            yield return null;
        }



    }

    private void Update() {

        if (_isCardUpgrading == true) {
            if (CheckUpgradeLoadCardData() == true) {

                _isCardUpgrading = false;
                _upgradeButton.interactable = true;
                _nextButton.interactable = true;
                _previousButton.interactable = true;
                LoadCardData();
            }
        }


    }
    private bool CheckUpgradeLoadCardData() {
        bool check = true;
        _tabView.Character.CardList[_cardIndex].RequestLoadReferenzData();

        check = check & (_tabView.Character.CardList[_cardIndex].WaitingOnDataCount == 0);

        return check;
    }



}
