using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class HealConfig : EffectOperation
{
    public float amount;
    public override EffectIntent Type => EffectIntent.Heal;
    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        intents.Add(new CombatIntent
        {
            type = Type,
            source = ctx.damageSource,
            target = GameManager.instance.player,
            value = amount,
            context = ctx
        });
    }
}