using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class KnockBackConfig : EffectOperation
{
    public float magnitude;
    public KnockBack knockback;
    public float damageMultiplier;
    public override EffectIntent Type => EffectIntent.Knockback;
    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        knockback = new KnockBack
        {
            magnitude = magnitude,
            knockBackDamage = ctx.value * damageMultiplier, 
            originPoint = ctx.damageSource.owner.gameObject.transform.position
        };

        intents.Add(new CombatIntent
        {
            source = ctx.damageSource,
            target = ctx.target,
            intent = Type,
            context = ctx,
            behaviour = this,
        });
    }
}