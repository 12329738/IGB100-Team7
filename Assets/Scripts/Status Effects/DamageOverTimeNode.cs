using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class DamageOverTimeConfig : EffectNodeConfig
{
    public float value;
    public float damageInterval;
    float lastTimeDamaged;
    public override void Execute(EffectContext ctx)
    {
        ctx.damage = value * ctx.deltaTime;
        ctx.target.GetComponent<IDamageable>()
            ?.TakeDamage(ctx);
        Debug.Log($"{ctx.target} took {value} dmg");
    }
}