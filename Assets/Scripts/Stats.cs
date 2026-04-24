using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.VersionControl;
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
            provider.OnDirty -= MarkDirty;
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
        if (isInitializing || isRecalculating) return;

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


        foreach (var stat in baseStats.statPresets)
        {
            cachedStats[stat.statType] = stat.value;
        }

        var flat = new Dictionary<StatType, float>();
        var multiplicative = new Dictionary<StatType, float>();

        foreach (var stat in baseStats.statPresets)
        {
            flat[stat.statType] = 0f;
            multiplicative[stat.statType] = 1f;
        }


        HashSet<IModifierProvider> visited = new();

        foreach (IModifierProvider provider in modifierProviders)
        {
            foreach (var mod in GetModifiersSafe(provider, visited))
            {
                if (!cachedStats.ContainsKey(mod.stat))
                    continue;

                switch (mod.type)
                {

                    case ModifierType.Flat:
                        flat[mod.stat] += mod.amount;
                        break;

                    case ModifierType.Percentage:
                        multiplicative[mod.stat] += mod.amount/100;
                        break;
                }
            }
        }

        foreach (var stat in cachedStats.Keys.ToList())
        {
            float baseValue = cachedStats[stat];

            float value = baseValue + flat[stat];
            value *=  multiplicative[stat];                  

            cachedStats[stat] = value;
        }
        isDirty = false;
        isRecalculating = false;
    }

    private IEnumerable<StatModifier> GetModifiersSafe(
    IModifierProvider provider,
    HashSet<IModifierProvider> visited)
    {
        if (!visited.Add(provider))
            yield break; 

        if (provider is ModifierProvider mp)
        {
            foreach (var mod in mp.Modifiers)
                yield return mod;

            foreach (var child in mp.children)
            {
                foreach (var mod in GetModifiersSafe(child, visited))
                    yield return mod;
            }
        }
        else
        {
            foreach (var mod in provider.Modifiers)
                yield return mod;
        }
    }

    public float GetMultiplierFor(StatType stat)
    {
        float mult = 1f;

        foreach (var provider in modifierProviders)
        {
            foreach (var mod in GetModifiersSafe(provider, new HashSet<IModifierProvider>()))
            {
                if (mod.stat == stat && mod.type == ModifierType.Percentage)
                    mult *= (1 + mod.amount / 100f);
            }
        }

        return mult;
    }
    public IEnumerable<StatModifier> GetAllModifiers()
    {
        HashSet<IModifierProvider> visited = new();

        foreach (var provider in modifierProviders)
        {
            foreach (var mod in GetModifiersSafe(provider, visited))
            {
                yield return mod;
            }
        }
    }

    public IEnumerable<StatModifier> GetModifiers(StatType type)
    {
        foreach (var mod in GetAllModifiers())
        {
            if (mod.stat == type)
                yield return mod;
        }
    }


}


