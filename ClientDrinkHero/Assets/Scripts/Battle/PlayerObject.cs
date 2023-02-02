using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour {
    [SerializeField] private Player _player;

    private List<HeroHandler> _heroHandlerList;

    public Player Player => _player;

    //remove after prototype
    public UserObjectOld testUserField;
    private int _loadcounter;


    private void Awake() {
        _player = new Player();
        _player.Clear();


        _heroHandlerList = new List<HeroHandler>();

        _loadcounter = 0;
        for (int i = 0; i < 4;) {
            HeroHandler heroHandler = new HeroHandler();
            heroHandler.RequestData(UIDataContainer.TeamIds[i]);

            //heroHandler.RequestData((int)UserSingelton.Instance.UserObject.User.HeroDatabasesList[i].RefHero);
            heroHandler.LoadingFinished += HerosLoaded;

            _heroHandlerList.Add(heroHandler);

            i = i + 1;
        }


    }

    private void Update() {
        foreach (HeroHandler heroHandler in _heroHandlerList) {
            heroHandler.Update();
        }

    }
    private void HerosLoaded() {
        _loadcounter = _loadcounter + 1;
        if (_loadcounter != 4) {
            return;
        }

        GameDeck gameDeck = new GameDeck();
        Deck deck = new Deck();
        int i = 0;
        foreach (HeroHandler heroHandler in _heroHandlerList) {

            HeroSlot slot = new HeroSlot();
            slot.Hero = heroHandler.Heros;
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

    [ContextMenu("play Card 0")]
    public void PlayCardZero() {
        _player.PlayHandCard(0);
    }

    [ContextMenu("play Card 1")]
    public void PlayCardOne() {
        _player.PlayHandCard(1);
    }

    [ContextMenu("play Card 2")]
    public void PlayCardTwo() {
        _player.PlayHandCard(2);
    }

    [ContextMenu("play Card 3")]
    public void PlayCardThree() {
        _player.PlayHandCard(3);
    }

    [ContextMenu("play Card 4")]
    public void PlayCardfour() {
        _player.PlayHandCard(4);
    }

    [ContextMenu("start turn")]
    public void DrawCard() {
        _player.StartTurn();
    }

    [ContextMenu("test")]
    public void Testing() {
        Debug.Log("Testing");
    }
}