using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class KnockBackConfig : EffectNodeConfig
{
    public float magnitude;
    public override EffectType Type => EffectType.Knockback;
    public override void Execute(EffectContext ctx)
    {
        ctx.baseValue = magnitude;
        ctx.value = magnitude;
        ctx.intent = EffectIntent.Knockback;
    }
}