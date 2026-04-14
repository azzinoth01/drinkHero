using UnityEngine;

public class Attack : Effect, ISkill
{
    public Attack(CardEffectData statusEffect) : base(statusEffect) {
    }

    public override bool ActivateEffectBase(ICharacterAction target,ActivationTimeEnum activation,int? value = null) {
        if(ActivationTimeEnum.onCast == activation) {
            value = Random.Range(_minValue,_maxValue + 1);
            target.AttackEnemy(value.Value);
        }
        return false;
    }


}