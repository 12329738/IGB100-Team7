using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class HealConfig : EffectNodeConfig
{
    public float amount;
    public override EffectNodeType Type => EffectNodeType.Heal;
    public override void Execute(EffectContext ctx)
    {

        ctx.target.GetComponent<Entity>().combat.Heal(ctx,amount);
    }
}