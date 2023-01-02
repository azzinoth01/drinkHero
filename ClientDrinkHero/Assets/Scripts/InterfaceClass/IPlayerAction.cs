public interface IPlayerAction : ICharacterAction {

    public void DiscardHandCards(int value);
    public int GetDiscadHandCardsCount();
    public void Mana(int value);
    public void DrawCard(int value);
}
