public class ShieldAttack : Effect, ISkill
{
    public ShieldAttack(CardEffectData statusEffect) : base(statusEffect) {
    }

    public override bool ActivateEffectBase(ICharacterAction target,ActivationTimeEnum activation,int? value = null) {
        if(ActivationTimeEnum.onCast == activation) {
            target.ShieldAttack();
        }
        return false;
    }
}

