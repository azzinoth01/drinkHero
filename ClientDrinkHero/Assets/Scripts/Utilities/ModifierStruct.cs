using System;

[Serializable]
public struct ModifierStruct {

    private int _modifierValue;
    private int _fixedModifierValue;

    public ModifierStruct(int modifierValue, int fixedModifierValue) {
        _modifierValue = modifierValue;
        _fixedModifierValue = fixedModifierValue;
    }
    public ModifierStruct(ModifierStruct copy) {
        _modifierValue = copy._modifierValue;
        _fixedModifierValue = copy._fixedModifierValue;
    }

    public int ModifierValue {
        get {
            int returnValue = _modifierValue;
            if (returnValue < -100) {
                returnValue = -100;
            }
            return _modifierValue;
        }
    }
    public int FixedModifierValue {

        get {
            return _fixedModifierValue;
        }
    }
    public void AddModifier(int value) {
        _modifierValue = _modifierValue + value;
    }
    public void addFixedModifier(int value) {
        _fixedModifierValue = _fixedModifierValue + value;
    }

    public int CalcValue(int value) {
        float modifier = ModifierValue;
        modifier = modifier / 100;

        float tempValue = (value * (1 + modifier)) + _fixedModifierValue;

        value = (int)tempValue;

        //value = value * (1 + (ModifierValue / 100)) + _fixedModifierValue;

        if (value < 0) {
            value = 0;
        }

        return value;
    }


}
