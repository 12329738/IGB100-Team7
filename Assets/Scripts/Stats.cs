using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
[Serializable]

public class Stats 
{

    public StatsPreset preset;
    [HideInInspector]
    public List<StatModifier> modifiers;

    public Dictionary<StatType, Stat> statDictionary;

    public void Initialize()
    {
        statDictionary = new Dictionary<StatType, Stat>();
        foreach (StatConfig statConfig in preset.statConfigs)
        {
            Stat newStat = new Stat();
            newStat.Initialize(statConfig.value);
            statDictionary[statConfig.stat] = newStat;
        }
    }

    public Stat GetStat(StatType type)
    {
        return statDictionary[type];
    }

    public void ApplyModifiers(List<StatModifier> modifiers)
    {
        foreach (StatModifier modifier in modifiers)
        {
            GetStat(modifier.stat).ApplyModifiers(modifiers);
        }
    }
}
