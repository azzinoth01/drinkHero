using UnityEngine;

public class DiscardHandCards : Effect, ISkill {

    public DiscardHandCards(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffect(IPlayerAction target, ActivationTimeEnum activation, int? value = null) {
        if (ActivationTimeEnum.onCast == activation) {
            value = Random.Range(_minValue, _maxValue + 1);
            target.DiscardHandCards((int)value);
        }
        return false;
    }
}
