using UnityEngine;

public class ShieldDmgOnly : Effect, ISkill {
    public ShieldDmgOnly(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffect(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (ActivationTimeEnum.onCast == activation) {
            value = Random.Range(_minValue, _maxValue + 1);
            target.AttackEnemy(value.Value);
        }
        return false;
    }

}
