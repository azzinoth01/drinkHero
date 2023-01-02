using UnityEngine;

public class BuffHealForEachDiscard : Effect, IBuff {
    public BuffHealForEachDiscard(Effect statusEffect) : base(statusEffect) {
    }
    public override bool ActivateEffect(IPlayerAction target, ActivationTimeEnum activation, int? value = null) {
        if (_isOver == true) {
            return false;
        }


        if (ActivationTimeEnum.actionFinished == activation) {

            value = Random.Range(_minValue, _maxValue + 1);
            target.Heal((int)value * target.GetDiscadHandCardsCount());


            if (_durationType == (int)DurationTypeEnum.uses) {
                ReduceDuration();
                SetIsOver();
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
