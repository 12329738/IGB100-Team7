using System.Collections.Generic;
using UnityEngine;


public abstract class Upgrade : ScriptableObject
{
    [HideInInspector]
    public string name;
    [HideInInspector]
    public string description;
    public List<EffectNodeConfig> effects;
    public List<StatModifier> modifiers;
    [HideInInspector]
    public RarityEnum rarity;
}
