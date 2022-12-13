public interface ICharacterAction {

    public void Heal(int value);
    public void TakeDmg(int value);
    public void Shield(int value);

    public void AddAttackModifier(int value);
    public void AddDefenceModifier(int value);

    public void SwapShieldWithEnemy();
    public void AttackEnemy(int value);

    public void RemoveShield();

    public void SkipTurn();

    public void SetBaseMultihit(int value);
    public void SetBuffMultihit(int value);

}
