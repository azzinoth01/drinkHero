public interface IEffect {
    public int Id {
        get;
        set;
    }
    public string Name {
        get;
        set;
    }
    public DurationTypeEnum Duration {
        get;
    }
    public int DurationValue {
        get;
        set;
    }
    public bool Stackable {
        get;
        set;
    }
    public bool RefreshOnStack {
        get;
        set;
    }
    public bool IgnoreStatusLimit {
        get;
        set;
    }

    public int MaxValue {
        get;
        set;
    }
    public int MinValue {
        get;
        set;
    }


    public bool ActivateEffectBase(ICharacterAction target, ActivationTimeEnum activation, int? value = null);

    public bool ActivateEffect(IPlayerAction target, ActivationTimeEnum activation, int? value = null);


    public bool StatusEffectApplyCheck(IEffect statusEffect);


}
