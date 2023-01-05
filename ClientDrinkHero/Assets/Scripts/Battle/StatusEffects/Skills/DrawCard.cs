using UnityEngine;

public class DrawCard : Effect, ISkill {
    public DrawCard(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffect(IPlayerAction target, ActivationTimeEnum activation, int? value = null) {
        if (ActivationTimeEnum.onCast == activation) {
            value = Random.Range(_minValue, _maxValue + 1);
            target.DrawCard(value.Value);
        }
        return false;
    }
}
