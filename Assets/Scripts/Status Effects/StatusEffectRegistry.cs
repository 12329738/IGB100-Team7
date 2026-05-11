using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectRegistry : MonoBehaviour
{
    public static StatusEffectRegistry instance;

    private Dictionary<DamageSourceDefinition, List<StatusEffectInstance>> totalInstances = new();

    private void Awake()
    {
        instance = this;
    }

    public void AddInstance(StatusEffectInstance instance)
    {
        if (!totalInstances.ContainsKey(instance.definition))
            totalInstances[instance.definition] = new List<StatusEffectInstance> { instance };
        else
            totalInstances[instance.definition].Add(instance);
    }

    public void RemoveInstance(StatusEffectInstance instance)
    {
        if (!totalInstances.ContainsKey(instance.definition))
            return;

        totalInstances[instance.definition].Remove(instance);
    }

    public int GetTotalStacks(DamageSourceDefinition type)
    {
        totalInstances.TryGetValue(type, out var value);
        int total = 0;
        foreach (StatusEffectInstance instance in value)
            total += (int)instance.context.stacks;
        return total;
    }

    internal int GetStacksFromSource(DamageSourceDefinition effectData)
    {
        totalInstances.TryGetValue(effectData, out var value);
        int total = 0;
        foreach (StatusEffectInstance instance in value)
            total += (int)instance.context.stacks;
        return total;
    }

    internal int GetInstancesFromSource(DamageSourceDefinition effectData)
    {
        totalInstances.TryGetValue(effectData, out var value);
        return value.Count;
    }
}