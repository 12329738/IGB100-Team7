using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class KnockBackConfig : EffectOperation
{
    public float magnitude;
    public override EffectIntent Type => EffectIntent.Knockback;
    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        intents.Add(new CombatIntent
        {
            source = ctx.damageSource,
            target = ctx.target,
            value = magnitude,
            type = Type
        });
    }
}