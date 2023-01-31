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

    private LoadSprite _loadSprite;

    public override void Initialize()
    {
        backButton.onClick.AddListener(() => ViewManager.ShowLast());
        _loadSprite = characterPortraitImage.GetComponent<LoadSprite>();
    }

    public void LoadCharacterData(int id)
    {
        var character = CharacterSelectView.UnlockedHeroes[id].Hero;
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
}