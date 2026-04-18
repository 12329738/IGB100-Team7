using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class KnockBackConfig : EffectNodeConfig
{
    public float magnitude;
    public override EffectNodeType Type => EffectNodeType.Knockback;
    public override void Execute(EffectContext ctx)
    {
        ctx.target.GetComponent<Entity>().combat.KnockBack(ctx,magnitude);
    }
}