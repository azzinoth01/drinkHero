using UnityEngine;

public class Heal : Effect, ISkill {
    public Heal(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffect(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (ActivationTimeEnum.onCast == activation) {
            value = Random.Range(_minValue, _maxValue + 1);
            target.Heal(value.Value);


        }

        return false;

    }



}
