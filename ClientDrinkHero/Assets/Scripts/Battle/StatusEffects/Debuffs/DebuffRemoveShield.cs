public class DebuffRemoveShield : Effect, IDebuff {
    public DebuffRemoveShield(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffect(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (_isOver == true) {
            return false;
        }

        if (ActivationTimeEnum.onCast == activation || ActivationTimeEnum.turnStart == activation) {
            target.RemoveShield();
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
            return false;
        }
        return true;

    }



}
