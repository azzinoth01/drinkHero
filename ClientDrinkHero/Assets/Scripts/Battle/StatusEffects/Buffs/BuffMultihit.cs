using UnityEngine;

public class BuffMultihit : Effect, IBuff {
    public BuffMultihit(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffect(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {

        if (_isOver == true) {
            return false;
        }

        if (ActivationTimeEnum.actionStart == activation) {
            value = Random.Range(_minValue, _maxValue + 1);
            target.SetBuffMultihit(value.Value);

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