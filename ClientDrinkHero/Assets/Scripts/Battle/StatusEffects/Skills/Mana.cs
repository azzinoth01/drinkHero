using UnityEngine;

public class Mana : Effect, ISkill {
    public Mana(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffect(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (ActivationTimeEnum.onCast == activation) {
            value = Random.Range(_minValue, _maxValue + 1);
            target.Mana(value.Value);


        }

        return false;

    }

}
