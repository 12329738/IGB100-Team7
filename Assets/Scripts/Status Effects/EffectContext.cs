using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectContext
{
    public IDamageSource damageSource;
    //public IDamageSource damageSourceOwner;
    public IDamageSource target;
    public EffectIntent intent;
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

}