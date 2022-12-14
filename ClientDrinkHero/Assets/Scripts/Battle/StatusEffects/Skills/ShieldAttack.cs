public class ShieldAttack : Effect, ISkill {
    public ShieldAttack(Effect statusEffect) : base(statusEffect) {
    }

    public override bool ActivateEffect(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (ActivationTimeEnum.onCast == activation) {
            target.ShieldAttack();
        }
        return false;
    }
}

