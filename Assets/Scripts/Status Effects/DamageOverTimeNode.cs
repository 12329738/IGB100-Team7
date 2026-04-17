using System;
using UnityEngine;

[Serializable]
public class DamageOverTimeConfig : EffectNodeConfig
{
    public float value;
    public override void Execute(EffectContext ctx)
    {
        ctx.target.GetComponent<IDamageable>()
            ?.TakeDamage(value * ctx.deltaTime);
        Debug.Log($"{ctx.target} took {value} dmg");
    }
}