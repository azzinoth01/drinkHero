public class DebuffFixedAttack : Effect, IDebuff {
    public DebuffFixedAttack(Effect statusEffect) : base(statusEffect) {
    }

    public override bool ActivateEffect(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (_isOver == true) {
            return false;
        }

        if (ActivationTimeEnum.onCast == activation) {

            target.AddFixedAttackModifier(-_maxValue);

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
            target.AddFixedAttackModifier(_maxValue);
            return false;
        }
        return true;
    }
}
