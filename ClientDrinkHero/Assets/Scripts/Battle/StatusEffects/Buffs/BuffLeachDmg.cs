public class BuffLeachDmg : Effect, IBuff {
    public BuffLeachDmg(Effect statusEffect) : base(statusEffect) {
    }

    public override bool ActivateEffectBase(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {

        if (_isOver == true) {
            return false;
        }

        if (ActivationTimeEnum.actionFinished == activation) {
            if (value != null) {
                target.Heal((int)value);
            }
            else {
                target.Heal(0);
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
