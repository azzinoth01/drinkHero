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
    
    public override void Initialize()
    {
        backButton.onClick.AddListener(ViewTweener.ButtonClickTween(backButton, 
            backButtonClicked, () => ViewManager.ShowLast()));

        _backButtonInitial = backButton.image.sprite;
        
        _loadSprite = characterPortraitImage.GetComponent<LoadSprite>();
    }

    public void LoadCharacterData(int id)
    {
        var character = CharacterSelectView.UnlockedHeroes[id-1].Hero;
        var cardList = character.CardList;

        var cards = new List<CardData>();

        foreach (var card in cardList)
        {
            var data = new CardData();
            data.cost = card.Cost;
            data.description = card.Text;
            data.name = card.Name;
            cards.Add(data);
        }

        for (var i = 0; i < cardPreviews.Length; i++) cardPreviews[i].SetData(cards[i]);

        _loadSprite.LoadNewSprite(character.SpritePath);
        characterNameLabel.SetText(character.Name);
    }
    
    public override void Show()
    {
        base.Show();
        backButton.image.sprite = _backButtonInitial;
    }

    public override void Hide()
    {
        base.Hide();
        OnZoomReset?.Invoke();
    }
}