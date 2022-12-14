using UnityEngine;

public class SelfAttack : Effect, ISkill {
    public SelfAttack(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffect(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (ActivationTimeEnum.onCast == activation) {
            value = Random.Range(_minValue, _maxValue + 1);
            target.TakeDmg(_maxValue);
        }
        return false;
    }
}
