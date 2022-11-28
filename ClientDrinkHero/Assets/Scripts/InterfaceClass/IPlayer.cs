using System;

public interface IPlayer : ICharacter {

    public int MaxRessource();
    public int CurrentRessource();
    public IHandCards GetHandCards();
    public event Action<int> RessourceChange;
    public event Action UpdateHandCards;
    public event Action GameOverEvent;


}
