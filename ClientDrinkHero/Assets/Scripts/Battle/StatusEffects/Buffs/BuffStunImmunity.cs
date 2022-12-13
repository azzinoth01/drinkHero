public class BuffStunImmunity : Effect, IBuff {



    public BuffStunImmunity(Effect statusEffect) : base(statusEffect) {
    }

    public override bool StatusEffectApplyCheck(IEffect statusEffect) {
        if (statusEffect is DebuffStun) {
            return false;
        }
        return true;
    }

    public override bool ActivateEffect(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (_isOver == true) {
            return false;
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
