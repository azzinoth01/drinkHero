using System;

[Serializable]
public class BuffHealOverTime : Buff {
    public BuffHealOverTime(BuffHealOverTime statusEffect) : base(statusEffect) {
    }

    public override void ActivateStatusEffect(Character target, ActivationTimeEnum activation, int? value = null) {

        if (ActivationTimeEnum.turnStartActive == activation || ActivationTimeEnum.onCast == activation) {

            target.HealCharacter(_value);


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
        return new BuffHealOverTime(this);
    }
}
