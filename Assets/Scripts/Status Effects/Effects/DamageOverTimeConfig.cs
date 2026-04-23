using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class DamageOverTimeConfig : EffectOperation
{
    public float damage;
    public bool isHit;
    public override EffectIntent Type => EffectIntent.DamageOverTime;
    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        float value = damage;
        ctx.isHit = isHit;
        
        if (ctx.stacks > 1)
            value *= (float)ctx.stacks;

        var receiver = ctx.source.GetComponent<IModifierReceiver>();
        if (receiver != null)
            value*= receiver.stats.GetStat(StatType.Damage);
        intents.Add(new CombatIntent
        {
            source = ctx.source,
            target = ctx.target,
            type = EffectIntent.DealDamage,
            value = value,
            context = ctx
        });


        Debug.Log($"{ctx.target} took {value} dmg from {ctx.stacks} stacks of {ctx.origin}");
    }
}