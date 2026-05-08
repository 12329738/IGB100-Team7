using System.Collections.Generic;
using System.Linq;

public static class StatusEffectOverrides
{
    private static Dictionary<StatusEffectData, int> maxStackOverrides = new();
    private static Dictionary<StatusEffectData, List<StatModifier>> modifierOverrides = new();

    public static void SetMaxStacks(StatusEffectData data, int value)
    {
        maxStackOverrides[data] = value;
    }

    public static int GetMaxStacks(StatusEffectData data)
    {
        return maxStackOverrides.TryGetValue(data, out int v)
            ? v
            : data.maxStacks;
    }

    public static void AddModifier(StatusEffectData data, List<StatModifier> modifier)
    {
        if (!modifierOverrides.ContainsKey(data))
        {
            modifierOverrides[data] = new List<StatModifier>();
        }

        var existing = modifierOverrides[data];


        bool sameValues = existing.Count == modifier.Count && !existing.Except(modifier).Any();

        if (!sameValues)
        {

            existing.AddRange(modifier);
        }
    }

    public static List<StatModifier> GetModifier(StatusEffectData data)
    {
        return modifierOverrides.TryGetValue(data, out List<StatModifier> mod)
            ? mod
            : data.modifiers;
    }
}