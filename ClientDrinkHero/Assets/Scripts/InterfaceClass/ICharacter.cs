using System;

public interface ICharacter {


    public int MaxHealth();
    public int CurrentHealth();
    public int CurrentShield();
    public event Action<int> HealthChange;
    public event Action<int> ShieldChange;
    public event Action TurnEnded;

    public void EndTurn();
    public void StartTurn();

}
