public class DebuffAttack : Effect, IDebuff {
    public DebuffAttack(Effect statusEffect) : base(statusEffect) {
    }

    public override bool ActivateEffectBase(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (_isOver == true) {
            return false;
        }

        if (ActivationTimeEnum.onCast == activation) {

            target.AddAttackModifier(-_maxValue);

        }
        if (ActivationTimeEnum.actionFinished == activation) {
            if (_durationType == (int)DurationTypeEnum.uses) {
                ReduceDuration();
                SetIsOver();
            }
        }

        if (ActivationTimeEnum.turnEnd == activation) {
            if (_durationType == (int)DurationTypeEnum.turns) {
                ReduceDuration();
                SetIsOver();
            }
        }

        if (_isOver == true) {
            target.AddAttackModifier(_maxValue);
            return false;
        }
        return true;
    }

}