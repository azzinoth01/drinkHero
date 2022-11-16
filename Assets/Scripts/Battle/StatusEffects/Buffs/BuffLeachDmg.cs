public class BuffLeachDmg : Buff {
    public BuffLeachDmg(BuffLeachDmg statusEffect) : base(statusEffect) {
    }

    public override void ActivateStatusEffect(Character target, ActivationTimeEnum activation, int? value = 0) {

        if (ActivationTimeEnum.afterDmgCausedActive == activation) {
            target.HealCharacter((int)value);
            if (_durationType == DurationTypeEnum.uses) {
                ReduceDuration();
            }
        }
        if (ActivationTimeEnum.turnEndActive == activation) {
            if (_durationType == DurationTypeEnum.turns) {
                ReduceDuration();
            }
        }

    }


    public override object Clone() {
        return new BuffLeachDmg(this);
    }
}
