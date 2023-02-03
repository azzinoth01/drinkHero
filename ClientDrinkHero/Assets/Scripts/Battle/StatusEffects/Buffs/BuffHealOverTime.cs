public class BuffHealOverTime : Effect, IBuff {
    public BuffHealOverTime(Effect statusEffect) : base(statusEffect) {
    }
    public override bool ActivateEffectBase(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (_isOver == true) {
            return false;
        }


        if (ActivationTimeEnum.turnStart == activation || ActivationTimeEnum.onCast == activation) {
            target.CallEffectText("HEAL OVER TIME");
            target.Heal(_maxValue);


            if (_durationType == (int)DurationTypeEnum.uses) {
                ReduceDuration();
            }
        }

        if (ActivationTimeEnum.turnEnd == activation) {
            if (_durationType == (int)DurationTypeEnum.turns) {
                ReduceDuration();
            }
            SetIsOver();
        }


        if (_isOver == true) {
            return false;
        }
        return true;
    }



}
