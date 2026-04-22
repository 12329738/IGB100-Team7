using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectRegistry : MonoBehaviour
{
    public static StatusEffectRegistry instance;

    private Dictionary<StatusEffectData, int> totalStacks = new();

    private void Awake()
    {
        instance = this;
    }

    public void AddStacks(StatusEffectData type, int amount)
    {
        if (!totalStacks.ContainsKey(type))
            totalStacks[type] = 0;

        totalStacks[type] += amount;
    }

    public void RemoveStacks(StatusEffectData type, int amount)
    {
        if (!totalStacks.ContainsKey(type))
            return;

        totalStacks[type] -= amount;
        if (totalStacks[type] < 0)
            totalStacks[type] = 0;
    }

    public int GetTotalStacks(StatusEffectData type)
    {
        return totalStacks.TryGetValue(type, out var value) ? value : 0;
    }

    internal int GetStacksFromSource(StatusEffectData effectData, GameObject source)
    {
        return totalStacks.TryGetValue(effectData, out var value) ? value : 0;
    }
}