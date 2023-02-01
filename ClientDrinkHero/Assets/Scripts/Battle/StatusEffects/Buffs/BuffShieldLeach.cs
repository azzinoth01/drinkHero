public class BuffShieldLeach : Effect, IBuff {
    public BuffShieldLeach(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffectBase(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {


        if (_isOver == true) {
            return false;
        }

        if (ActivationTimeEnum.actionFinished == activation) {
            if (value != null) {
                target.Shield((int)value);
            }
            else {
                target.Shield(0);
            }
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
