public interface IHandCards {


    public int HandCardCount();
    public ICardDisplay GetHandCard(int index);
    public bool PlayHandCard(int index);

}
