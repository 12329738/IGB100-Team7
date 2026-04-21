public interface IModifierReceiver
{
    void AddModifier(StatModifier mod);
    void RemoveModifier(StatModifier mod);
    public Stats stats { get; set; }
}