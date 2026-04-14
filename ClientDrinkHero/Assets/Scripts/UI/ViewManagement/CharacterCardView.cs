using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardView : View
{
    [SerializeField] private TextMeshProUGUI characterNameLabel;
    [SerializeField] private Image characterFactionImage;
    [SerializeField] private Image characterPortraitImage;
    [SerializeField] private CharacterCardPreview[] cardPreviews;
    [SerializeField] private Button backButton;
    [SerializeField] private Sprite backButtonClicked;
    private Sprite _backButtonInitial;

    private LoadSprite _loadSprite;

    public static event Action OnZoomReset;

    public override void Initialize() {
        backButton.onClick.AddListener(ViewTweener.ButtonClickTween(backButton,
            backButtonClicked,() => ViewManager.ShowLast()));

        _backButtonInitial = backButton.image.sprite;

        _loadSprite = characterPortraitImage.GetComponent<LoadSprite>();
    }

    public void LoadCharacterData(string id) {
        HeroObject character = null;
        List<HeroObject> ownedHero = GameDataInstance.Instance.OwnedHeroes;

        foreach(HeroObject hero in ownedHero) {
            if(hero.Id == id) {
                character = hero;
            }
        }
        if(character == null) {
            return;
        }

        List<CardData> cardList = character.CardList;

        List<CardDataView> cards = new List<CardDataView>();

        foreach(CardData card in cardList) {
            var data = new CardDataView();
            data.cost = card.Cost;
            data.description = card.Text;
            data.name = card.Name;
            data.spritePath = card.IconPath;
            cards.Add(data);
        }

        for(int i = 0; i < cardPreviews.Length; i++) {
            cardPreviews[i].SetData(cards[i]);
        }

        _loadSprite.LoadNewSprite(character.SpritePath);
        characterNameLabel.SetText(character.Name);
    }

    public override void Show() {
        base.Show();
        backButton.image.sprite = _backButtonInitial;
    }

    public override void Hide() {
        base.Hide();
        OnZoomReset?.Invoke();
    }
}