using UnityEngine;

public class RemoveDebuff : Effect, ISkill {
    public RemoveDebuff(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffectBase(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (ActivationTimeEnum.onCast == activation) {
            value = Random.Range(_minValue, _maxValue + 1);
            target.RemoveDebuff(value.Value);
        }
        return false;
    }

}
