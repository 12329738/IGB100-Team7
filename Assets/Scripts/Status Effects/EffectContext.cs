using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectContext
{
    public GameObject source;
    public GameObject target;


    public object effectInstanceId;
    public int sourceInstanceId;
    public float hitInterval;
    public EffectType effectType;

    public bool isHit = true;
    public float baseValue;
    public float value;

    public ValueSource valueSource;
    public float percent;

    public EffectIntent intent;
    public CombatEvent trigger;

    public List<ContextModifier> modifiers = new();
    public EffectPayload payload = new();
    public bool isCrit;
    public int? stacks;

    public DamageSourceDefinition origin;
    public DamageFlags flags;

    public EffectContext Clone()
    {
        return (EffectContext)this.MemberwiseClone();
    }

}