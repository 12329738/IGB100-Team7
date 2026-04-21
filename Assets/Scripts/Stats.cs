using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
[Serializable]

public class Stats 
{

    public StatsPreset baseStats;
    private List<Action> pendingChanges = new List<Action>();

    public HashSet<IModifierProvider> modifierProviders = new HashSet<IModifierProvider>();

    public Dictionary<StatType, float> cachedStats = new Dictionary<StatType, float>();
    bool isDirty;
    private bool isRecalculating = false;
    private bool isInitializing = false;

    public void Initialize(StatsPreset preset)
    {
        isInitializing = true;

        baseStats = preset ?? new StatsPreset();

        cachedStats.Clear();

        foreach (var config in baseStats.statPresets)
        {
            cachedStats[config.statType] = config.value;
        }

        MarkDirty();

        isInitializing = false;
    }


    public void AddModifierProvider(IModifierProvider provider)
    {
        if (modifierProviders.Add(provider))
        {
            provider.OnDirty += MarkDirty;
            MarkDirty();
        }
    }



    public void RemoveModifierProvider(IModifierProvider provider)
    {
        if (modifierProviders.Remove(provider))
        {
            provider.OnDirty -= MarkDirty;
            MarkDirty();
        }
    }

    public void MarkDirty()
    {
        if (isInitializing) return;

        if (isDirty) return;

        isDirty = true;
    }



    public float GetStat(StatType type)
    {
        if (isDirty)
        {
            Recalculate();
        }

        return cachedStats.TryGetValue(type, out var value) ? value : 0f;
    }

    private void Recalculate()
    {
        if (isRecalculating)
            return;
        isRecalculating = true;

        while (pendingChanges.Count > 0)
        {
            var changesToApply = new List<Action>(pendingChanges);
            pendingChanges.Clear();

            foreach (var action in changesToApply)
            {
                action();
            }
        }

        foreach (StatConfig stat in baseStats.statPresets)
        {
            cachedStats[stat.statType] = stat.value;
        }

        Dictionary<StatType, float> additive = new Dictionary<StatType, float>();
        Dictionary<StatType, float> multiplicative = new Dictionary<StatType, float>();


        foreach (StatConfig stat in baseStats.statPresets)
        {
            additive[stat.statType] = 0f;
            multiplicative[stat.statType] = 1f;
        }


        foreach (var provider in modifierProviders)
        {
            IEnumerable<StatModifier> mods;

            if (provider is ModifierProvider mp)
                mods = mp.GetAllModifiers();
            else
                mods = provider.Modifiers;

            foreach (var mod in mods)
            {
                if (!cachedStats.ContainsKey(mod.stat))
                    continue;

                switch (mod.type)
                {
                    case ModifierType.Flat:
                        cachedStats[mod.stat] += mod.amount;
                        break;

                    case ModifierType.Additive:
                        additive[mod.stat] += mod.amount;
                        break;

                    case ModifierType.Multiplicative:
                        multiplicative[mod.stat] *= (1 + mod.amount);
                        break;
                }
            }
        }




        foreach (var stat in new List<StatType>(cachedStats.Keys))
        {
            cachedStats[stat] *= (1 + additive[stat]);
            cachedStats[stat] *= multiplicative[stat];
        }

        isDirty = false;
        isRecalculating = false;
    }

}


