using UnityEngine;

public static class EffectConverter {



    public static Effect ConvertEffectIntoEffectType(Effect item) {

        Effect converted = null;

        //buffs
        if (item.ClassType == "AttackBuff") {
            converted = new BuffAttackBuff(item);
        }
        else if (item.ClassType == "HealForEachDiscard") {
            converted = new BuffHealForEachDiscard(item);
        }
        else if (item.ClassType == "HealOverTime") {
            converted = new BuffHealOverTime(item);
        }
        else if (item.ClassType == "LeachDmg") {
            converted = new BuffLeachDmg(item);
        }
        else if (item.ClassType == "Multihit") {
            converted = new BuffMultihit(item);
        }
        else if (item.ClassType == "ShieldLeach") {
            converted = new BuffShieldLeach(item);
        }
        else if (item.ClassType == "StunImmunity") {
            converted = new BuffStunImmunity(item);
        }

        //debuffs
        else if (item.ClassType == "DebuffAttack") {
            converted = new DebuffAttack(item);
        }
        else if (item.ClassType == "DebuffFixedAttack") {
            converted = new DebuffFixedAttack(item);
        }
        else if (item.ClassType == "Poison") {
            converted = new DebuffPoison(item);
        }
        else if (item.ClassType == "RemoveShield") {
            converted = new DebuffRemoveShield(item);
        }
        else if (item.ClassType == "Stun") {
            converted = new DebuffStun(item);
        }


        //skills
        else if (item.ClassType == "Attack") {
            converted = new Attack(item);
        }
        else if (item.ClassType == "DiscardHandCard") {
            converted = new DiscardHandCards(item);
        }
        else if (item.ClassType == "Heal") {
            converted = new Heal(item);
        }
        else if (item.ClassType == "RemoveDebuff") {
            converted = new RemoveDebuff(item);
        }
        else if (item.ClassType == "SelfAttack") {
            converted = new SelfAttack(item);
        }
        else if (item.ClassType == "Shield") {
            converted = new Shield(item);
        }
        else if (item.ClassType == "ShieldAttack") {
            converted = new ShieldAttack(item);
        }
        else if (item.ClassType == "ShieldDmgOnly") {
            converted = new ShieldDmgOnly(item);
        }
        else if (item.ClassType == "SwapShieldWithEnemy") {
            converted = new SwapShieldWithEnemy(item);
        }
        else {
            Debug.Log("failed to convert Item ClassType: " + item.ClassType);
        }


        return converted;
    }

}
