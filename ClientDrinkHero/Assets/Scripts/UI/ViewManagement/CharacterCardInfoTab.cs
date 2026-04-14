using System.Collections.Generic;
using UnityEngine;

public class CharacterCardInfoTab : MonoBehaviour
{
    [SerializeField] private CharacterCardTabView _tabView;
    [SerializeField] private List<CharacterCardPreview> _cardInfoDisplay;

    private void OnEnable() {

        HeroObject hero = _tabView.Hero;

        int i = 0;
        foreach(CharacterCardPreview cardPreview in _cardInfoDisplay) {
            CardDataView data = new CardDataView();

            data.cost = hero.CardList[i].Cost;
            data.description = hero.CardList[i].Text;
            data.name = hero.CardList[i].Name;
            data.spritePath = hero.CardList[i].IconPath;
            cardPreview.SetData(data);

            i = i + 1;
        }

    }
}
