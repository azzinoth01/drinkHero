using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    [SerializeField] private Player _player;
    public Player Player => _player;

    private void Start() {
        _player = new Player();
        _player.Clear();

        HerosLoaded();
    }

    private void HerosLoaded() {

        GameDeck gameDeck = new GameDeck();
        Deck deck = new Deck();

        List<HeroObject> ownedHero = GameDataInstance.Instance.OwnedHeroes;
        List<HeroObject> teamHeroes = new List<HeroObject>();
        foreach(string heroID in UIDataContainer.TeamIds) {
            foreach(HeroObject hero in ownedHero) {
                if(hero.Id == heroID) {
                    teamHeroes.Add(hero);
                }
            }
        }
        int i = 0;
        foreach(HeroObject hero in teamHeroes) {
            HeroSlot slot = new HeroSlot();
            slot.Hero = hero;
            slot.SlotID = i;
            deck.HeroSlotList.Add(slot);

            i = i + 1;
        }
        gameDeck.Deck = deck;

        _player.GameDeck = gameDeck;
    }


    private void OnEnable() {
        Player.playerDamageReceived += PlayerDamageFeedback;
        Player.playerDamageBlocked += PlayerDamageBlockedFeedback;
        Player.playerHealed += PlayerHealedFeedback;
        Player.playerShieldUp += PlayerShieldUpFeedback;
    }

    private void OnDisable() {
        Player.playerDamageReceived -= PlayerDamageFeedback;
        Player.playerDamageBlocked -= PlayerDamageBlockedFeedback;
        Player.playerHealed -= PlayerHealedFeedback;
        Player.playerShieldUp -= PlayerShieldUpFeedback;
    }

    // Start is called before the first frame update
    //void Start() {

    //    Deck deck = testUserField.Users.DeckList[0];

    private void PlayerDamageFeedback() {
        //GlobalAudioManager.Instance.Play(_playerDamageSound);
    }

    private void PlayerDamageBlockedFeedback() {
        //GlobalAudioManager.Instance.Play(_playerDamageBlockedSound);
    }

    private void PlayerHealedFeedback() {
        //GlobalAudioManager.Instance.Play(_playerhealedSound);
    }

    private void PlayerShieldUpFeedback() {
        //GlobalAudioManager.Instance.Play(_playerShieldUpSound);
    }
}