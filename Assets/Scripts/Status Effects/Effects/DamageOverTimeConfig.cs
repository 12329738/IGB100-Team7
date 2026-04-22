using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class DamageOverTimeConfig : EffectNodeConfig
{
    public float damage;
    public bool isHit;
    public override EffectType Type => EffectType.DamageOverTime;
    public override void Execute(EffectContext ctx)
    {
        ctx.intent = EffectIntent.DealDamage;
        ctx.isHit = isHit;
        ctx.value = damage;
        if (ctx.stacks > 1)
            ctx.value *= (float)ctx.stacks;

        var receiver = ctx.source.GetComponent<IModifierReceiver>();
        if (receiver != null)
            ctx.value *= receiver.stats.GetStat(StatType.Damage);


        Debug.Log($"{ctx.target} took {ctx.value} dmg from {ctx.stacks} stacks of {ctx.origin}");
    }
}