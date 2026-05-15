using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectContext
{
    public IDamageSource damageSource;
    public IDamageSource target;
    public CombatEvent trigger;

    public float value;
    public bool isHit = true;

    public bool isCrit;
    public int? stacks;

    public List<IIntentModifier> modifiers = new();
    public EffectPayload payload = new();

    public EffectContext Clone()
    {
        return (EffectContext)this.MemberwiseClone();
    }

    public void Reset()
    {
        damageSource = null;
        target = null;
        value = 0;
        isHit = true;
        isCrit = false;
        stacks = 0;
        modifiers = null;
        payload.Reset();


    }
}