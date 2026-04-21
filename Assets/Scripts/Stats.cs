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
    public List<StatModifier> modifiers;
    private Dictionary<object, List<StatModifier>> sources = new Dictionary<object, List<StatModifier>>();

    public Dictionary<StatType, float> cachedStats = new Dictionary<StatType, float>();
    bool isDirty;
    private bool isRecalculating = false;


    public void Initialize(StatsPreset preset)
    {
        baseStats = preset;
        foreach (StatConfig config in baseStats.statPresets)
        {
            cachedStats[config.statType] = config.value;
        }
    }


    public void AddSource(object source, List<StatModifier> modifiers)
    {
        if (isRecalculating)
        {
            pendingChanges.Add(() => AddSource(source, modifiers));
            return;
        }

        sources[source] = modifiers;
        MarkDirty();
    }

    public void RemoveSource(object source)
    {
        if (sources.Remove(source))
        {
            MarkDirty();
        }
    }


    public void ClearSources()
    {
        sources.Clear();
        MarkDirty();
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


        foreach (var source in sources.Values)
        {
            if (source == null) continue;

            foreach (var mod in source)
            {
                if (!cachedStats.ContainsKey(mod.statType))
                    continue;

                switch (mod.type)
                {
                    case ModifierType.Flat:
                        cachedStats[mod.statType] += mod.value;
                        break;

                    case ModifierType.Additive:
                        additive[mod.statType] += mod.value;
                        break;

                    case ModifierType.Multiplicative:
                        multiplicative[mod.statType] *= (1 + mod.value);
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


