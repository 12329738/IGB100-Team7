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
        ctx.damage = damage;
        ctx.hitInterval = damageInterval;
        ctx.target.GetComponent<Entity>().combat.DealDamage(ctx);
        Debug.Log($"{ctx.target} took {damage} dmg");
    }
}