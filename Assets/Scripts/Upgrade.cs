using System.Collections.Generic;
using UnityEngine;


public abstract class Upgrade : ScriptableObject
{

    public string name;

    public string description;

    public List<EffectEntryNode> effects;
    public List<StatModifier> modifiers;
    [HideInInspector]
    public RarityEnum rarity;

    private void OnValidate()
    {
        if (effects == null) return;

        foreach (var entry in effects)
        {
            entry?.Validate();
        }
    }
}

