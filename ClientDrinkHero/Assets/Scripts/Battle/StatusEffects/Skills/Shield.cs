using UnityEngine;

public class Shield : Effect, ISkill {
    public Shield(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffectBase(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (ActivationTimeEnum.onCast == activation) {
            value = Random.Range(_minValue, _maxValue + 1);
            target.Shield(value.Value);


        }


        return false;
    }



}