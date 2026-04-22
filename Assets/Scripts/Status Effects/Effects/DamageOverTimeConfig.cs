using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class DamageOverTimeConfig : EffectNodeConfig
{
    public float damage;
    public float damageInterval;
    public bool isHit;
    public override EffectNodeType Type => EffectNodeType.DamageOverTime;
    public override void Execute(EffectContext ctx)
    {
        ctx.isHit = isHit;
        ctx.value = damage;
        IModifierReceiver receiver = ctx.source.GetComponent<IModifierReceiver>();
        if (receiver != null)
        {
            ctx.value = (float)(receiver.stats.GetMultiplierFor(StatType.Damage) * damage * ctx.stacks);
        }

        ctx.hitInterval = damageInterval;
        ctx.target.GetComponent<Entity>().combat.DealDamage(ctx);
        Debug.Log($"{ctx.target} took {damage} dmg from {ctx.stacks} stacks of {ctx.source}");
    }
}