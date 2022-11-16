using System;

[Serializable]
public class Buff : StatusEffect {
    public Buff(Buff statusEffect) : base(statusEffect) {
    }

    public override object Clone() {
        return new Buff(this);
    }
}
