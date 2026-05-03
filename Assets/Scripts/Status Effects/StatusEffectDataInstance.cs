using System.Collections.Generic;
using UnityEngine;


public class StatusEffectDataInstance 
{
    public string name;
    public string description;
    public float duration;
    public int maxStacks;
    public DamageSourceDefinition definition;
    public List<StatModifier> modifiers = new();

    public bool hasTick = true;
    public float tickInterval = 0.5f;
    public List<EffectEntryNode> entries = new List<EffectEntryNode>();

    public StatusEffectDataInstance(StatusEffectData data)
    {
        name = data.name;
        description = data.description;
        duration = data.duration;
        maxStacks = StatusEffectOverrides.GetMaxStacks(data);
        definition = data.definition;
        hasTick  = data.hasTick;
        tickInterval = data.tickInterval;
        entries = new List<EffectEntryNode>();
        foreach (var entry in data.entries)
        {
            entries.Add(new EffectEntryNode(entry)); 
        }

        modifiers = new List<StatModifier>();
        foreach (var mod in data.modifiers)
        {
            modifiers.Add(new StatModifier(mod)); 
        }

    }

    
}

