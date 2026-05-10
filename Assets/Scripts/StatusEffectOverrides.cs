using System;
using System.Collections.Generic;
using System.Linq;

public static class StatusEffectOverrides
{
    private static Dictionary<StatusEffectData, int> maxStackOverrides = new();
    private static Dictionary<StatusEffectData, List<StatModifier>> modifierOverrides = new();
    private static Dictionary<StatusEffectData, List<EffectEntryNode>> effectOverrides = new();

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
            : new List<StatModifier>();
    }

    internal static void AddEffects(StatusEffectData data, List<EffectEntryNode> effects)
    {
        if (!effectOverrides.ContainsKey(data))
        {
            effectOverrides[data] = new List<EffectEntryNode>();
        }

        var existing = effectOverrides[data];


        bool sameValues = existing.Count == effects.Count && !existing.Except(effects).Any();

        if (!sameValues)
        {

            existing.AddRange(effects);
        }
    }

    public static List<EffectEntryNode> GetEffects(StatusEffectData data)
    {
        return effectOverrides.TryGetValue(data, out List<EffectEntryNode> mod)
            ? mod
            : new List<EffectEntryNode>();
    }
}