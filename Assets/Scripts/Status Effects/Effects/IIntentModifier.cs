public interface IIntentModifier
{
    void Modify(ref CombatIntent intent);

    public EffectIntent effectToModifiy { get; set; }
}