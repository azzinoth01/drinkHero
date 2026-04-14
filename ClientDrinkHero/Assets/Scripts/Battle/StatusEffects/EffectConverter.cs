using UnityEngine;

public static class EffectConverter
{

    public static Effect ConvertEffectIntoEffectType(CardEffectData item) {

        Effect converted = null;

        //buffs
        if(item.ClassType == EffectTypeEnum.AttackBuff) {
            converted = new BuffAttackBuff(item);
        }
        else if(item.ClassType == EffectTypeEnum.HealForEachDiscard) {
            converted = new BuffHealForEachDiscard(item);
        }
        else if(item.ClassType == EffectTypeEnum.HealOverTime) {
            converted = new BuffHealOverTime(item);
        }
        else if(item.ClassType == EffectTypeEnum.LeachDmg) {
            converted = new BuffLeachDmg(item);
        }
        else if(item.ClassType == EffectTypeEnum.Multihit) {
            converted = new BuffMultihit(item);
        }
        else if(item.ClassType == EffectTypeEnum.ShieldLeach) {
            converted = new BuffShieldLeach(item);
        }
        else if(item.ClassType == EffectTypeEnum.StunImmunity) {
            converted = new BuffStunImmunity(item);
        }

        //debuffs
        else if(item.ClassType == EffectTypeEnum.DebuffAttack) {
            converted = new DebuffAttack(item);
        }
        else if(item.ClassType == EffectTypeEnum.DebuffFixedAttack) {
            converted = new DebuffFixedAttack(item);
        }
        else if(item.ClassType == EffectTypeEnum.Poison) {
            converted = new DebuffPoison(item);
        }
        else if(item.ClassType == EffectTypeEnum.RemoveShield) {
            converted = new DebuffRemoveShield(item);
        }
        else if(item.ClassType == EffectTypeEnum.Stun) {
            converted = new DebuffStun(item);
        }


        //skills
        else if(item.ClassType == EffectTypeEnum.Attack) {
            converted = new Attack(item);
        }
        else if(item.ClassType == EffectTypeEnum.DiscardHandCard) {
            converted = new DiscardHandCards(item);
        }
        else if(item.ClassType == EffectTypeEnum.DrawCard) {
            converted = new DrawCard(item);
        }
        else if(item.ClassType == EffectTypeEnum.Heal) {
            converted = new Heal(item);
        }
        else if(item.ClassType == EffectTypeEnum.Mana) {
            converted = new Mana(item);
        }
        else if(item.ClassType == EffectTypeEnum.RemoveDebuff) {
            converted = new RemoveDebuff(item);
        }
        else if(item.ClassType == EffectTypeEnum.SelfAttack) {
            converted = new SelfAttack(item);
        }
        else if(item.ClassType == EffectTypeEnum.Shield) {
            converted = new Shield(item);
        }
        else if(item.ClassType == EffectTypeEnum.ShieldAttack) {
            converted = new ShieldAttack(item);
        }
        else if(item.ClassType == EffectTypeEnum.ShieldDmgOnly) {
            converted = new ShieldDmgOnly(item);
        }
        else if(item.ClassType == EffectTypeEnum.ShieldSwap) {
            converted = new SwapShieldWithEnemy(item);
        }
        else {
            Debug.Log("failed to convert Item ClassType: " + item.ClassType);
        }
        return converted;
    }

}
