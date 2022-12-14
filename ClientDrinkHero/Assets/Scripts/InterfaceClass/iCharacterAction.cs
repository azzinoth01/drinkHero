public interface ICharacterAction {

    public void Heal(int value);
    public void TakeDmg(int value);
    public void TakeShieldDmg(int value);
    public void Shield(int value);
    public void AddAttackModifier(int value);
    public void AddDefenceModifier(int value);
    public void AddFixedAttackModifier(int value);
    public void AddFixedDefenceModifier(int value);
    public void SwapShieldWithEnemy();
    public void AttackEnemy(int value);
    public void RemoveShield();
    public void SkipTurn(int value);
    public void SetBuffMultihit(int value);
    public void ShieldAttack();
    public void DiscardHandCards(int value);
    public int GetDiscadHandCardsCount();
    public void Mana(int value);
    public void RemoveDebuff(int value);

}
