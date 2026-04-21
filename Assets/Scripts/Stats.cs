using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
[Serializable]

public class Stats 
{

    public StatsPreset baseStats;
    private List<Action> pendingChanges = new List<Action>();
    [HideInInspector]
    public Dictionary<object, List<StatModifier>> modifierSources = new Dictionary<object, List<StatModifier>>();

    public Dictionary<StatType, float> cachedStats = new Dictionary<StatType, float>();
    bool isDirty;
    private bool isRecalculating = false;


    public void Initialize(StatsPreset preset)
    {
        if (preset == null)
        preset = new StatsPreset(); 
        baseStats = preset;
        foreach (StatConfig config in baseStats.statPresets)
        {
            cachedStats[config.statType] = config.value;
        }

        Recalculate();
    }

    public void AddModifierSource(object source, List<StatModifier> modifiers)
    {
        if (isRecalculating)
        {
            pendingChanges.Add(() => AddModifierSource(source, modifiers));
            return;
        }

        if (!modifierSources.ContainsKey(source))
        {
            modifierSources.Add(source, modifiers);
        }

        else
        {
            modifierSources[source].AddRange(modifiers);
        }


         MarkDirty();
    }


    public void RemoveModifier(object source)
    {
        if (modifierSources.Remove(source))
        {
            MarkDirty();
        }
    }

    public void MarkDirty()
    {
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


        foreach (List<StatModifier> modifiers in modifierSources.Values)
        {
            if (modifiers == null) continue;
        
            foreach(StatModifier mod in modifiers)
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


