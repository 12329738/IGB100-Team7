using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectContext
{
    public IDamageSource damageSource;
    public IDamageSource damageSourceOwner;
    public IDamageSource target;



    public EffectInstance effectInstance;
    public int sourceInstanceId;
    public float hitInterval;
    //public EffectIntent effectType;

    public bool isHit = true;
    public float baseValue;
    public float value;

    public ValueSource valueSource;
    public float percent;

    public EffectIntent intent;
    public CombatEvent trigger;

    public List<IIntentModifier> modifiers = new();
    public EffectPayload payload = new();
    public bool isCrit;
    public int? stacks;

    public DamageSourceDefinition definition;
    public DamageFlags flags;

    public EventHandler eventHandler;
    public EffectContext Clone()
    {
        return (EffectContext)this.MemberwiseClone();
    }

}