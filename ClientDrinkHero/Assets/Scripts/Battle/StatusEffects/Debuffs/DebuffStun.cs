using UnityEngine;

public class DebuffStun : Effect, IDebuff {
    public DebuffStun(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffectBase(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (_isOver == true) {
            return false;
        }

        if (ActivationTimeEnum.turnStart == activation) {
            value = Random.Range(_minValue, _maxValue + 1);
            target.SkipTurn((int)value);
            target.CallEffectText("STUNNED");
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
