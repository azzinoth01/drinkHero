using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharacterUpgradeTab : MonoBehaviour
{
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


    private void Awake() {
        _cardIndex = 0;
        _nextButton.onClick.AddListener(NextCardData);
        _previousButton.onClick.AddListener(PreviousCardData);
        _upgradeButton.onClick.AddListener(UpgradeCard);

    }

    private void OnEnable() {

        LoadCardData();
    }

    private void LoadCardData() {
        HeroObject hero = _tabView.Hero;

        CardDataView data = new CardDataView();

        data.cost = hero.CardList[_cardIndex].Cost;
        data.description = hero.CardList[_cardIndex].Text;
        data.name = hero.CardList[_cardIndex].Name;
        data.spritePath = hero.CardList[_cardIndex].IconPath;

        _currentCard.SetData(data);

        _upgradeButton.interactable = false;
        _costValue.SetText("0");
        _hasValue.SetText("0");

        if(hero.CardList[_cardIndex].UpgradeTo != null) {
            data = new CardDataView();

            data.cost = hero.CardList[_cardIndex].UpgradeTo.Cost;
            data.description = hero.CardList[_cardIndex].UpgradeTo.Text;
            data.name = hero.CardList[_cardIndex].UpgradeTo.Name;
            data.spritePath = hero.CardList[_cardIndex].IconPath;

            _upgradeButton.interactable = true;

            _costValue.SetText(hero.CardList[_cardIndex].UpgradeItemAmount.ToString());
        }

        foreach(LoadSprite loadSprite in _upgradeItemList) {

            loadSprite.LoadNewSprite(hero.CardList[_cardIndex].UpgradeItem.SpritePath);
        }

        List<SavedItem> ownedItems = GameDataInstance.Instance.UserSave.CollectedItems;
        foreach(SavedItem item in ownedItems) {
            if(item.Id == hero.CardList[_cardIndex].UpgradeItem.Id) {
                _hasValue.SetText(item.Amount.ToString());
            }
        }
        _uppgradeCard.SetData(data);
    }

    private void NextCardData() {
        _cardIndex = _cardIndex + 1;

        if(_cardIndex >= _tabView.Hero.CardList.Count) {
            _cardIndex = 0;
        }
        LoadCardData();
    }
    private void PreviousCardData() {
        _cardIndex = _cardIndex - 1;

        if(_cardIndex < 0) {
            _cardIndex = _tabView.Hero.CardList.Count - 1;
        }
        LoadCardData();
    }
    private void UpgradeCard() {
        HeroObject hero = _tabView.Hero;
        int cost = hero.CardList[_cardIndex].UpgradeItemAmount;
        UpgradeItemData upgradeItem = hero.CardList[_cardIndex].UpgradeItem;
        SavedItem ownedItem = null;
        foreach(SavedItem savedItem in GameDataInstance.Instance.UserSave.CollectedItems) {
            if(upgradeItem.Id == savedItem.Id) {
                ownedItem = savedItem;
                break;
            }
        }
        if(ownedItem == null || ownedItem.Amount < cost) {
            return;
        }
        ownedItem.Amount = ownedItem.Amount - cost;
        hero.UpgradeCardLevel(_cardIndex);
        GameDataInstance.Instance.UserSave.UpgradeHeroCardLevel(hero,_cardIndex);
        GameDataInstance.Instance.UserSave.Save();
        LoadCardData();
    }
}
