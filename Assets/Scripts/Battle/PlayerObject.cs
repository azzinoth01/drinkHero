using UnityEngine;

public class PlayerObject : MonoBehaviour {

    [SerializeField] private Player _player;
    public Player PlayerReference => _player;

    //remove after prototype
    public UserObject testUserField;
    public EnemyObject testEnemyField;

    
    // Start is called before the first frame update
    void Start() {

        Deck deck = testUserField.Users.DeckList[0];

        GameDeck gameDeck = new GameDeck(deck);
        _player = new Player(gameDeck);
        _player.enemy = testEnemyField.enemy;

    }

    // Update is called once per frame
    void Update() {

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


}
