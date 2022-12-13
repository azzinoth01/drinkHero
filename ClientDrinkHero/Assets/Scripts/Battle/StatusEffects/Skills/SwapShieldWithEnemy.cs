public class SwapShieldWithEnemy : Effect, ISkill {
    public SwapShieldWithEnemy(Effect statusEffect) : base(statusEffect) {
    }


    public override bool ActivateEffect(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        if (ActivationTimeEnum.onCast == activation) {
            target.SwapShieldWithEnemy();
            _durationValue = 0;
            SetIsOver();

        }

        return false;

    }



}
