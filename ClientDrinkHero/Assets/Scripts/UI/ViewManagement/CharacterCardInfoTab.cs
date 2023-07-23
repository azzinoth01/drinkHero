using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCardInfoTab : MonoBehaviour {
    [SerializeField] private CharacterCardTabView _tabView;
    [SerializeField] private List<CharacterCardPreview> _cardInfoDisplay;

    private void OnEnable() {

        HeroDatabase hero = _tabView.Character;


        int i = 0;
        foreach (CharacterCardPreview cardPreview in _cardInfoDisplay) {
            CardData data = new CardData();

            data.cost = hero.CardList[i].Cost;
            data.description = hero.CardList[i].Text;
            data.name = hero.CardList[i].Name;
            data.spritePath = hero.CardList[i].GetSpritePath();
            cardPreview.SetData(data);

            i = i + 1;
        }






    }
}
