using System.Collections.Generic;
using UnityEngine;

public class ModifyMaxStacks : EffectOperation
{
    public override EffectIntent Type => EffectIntent.ModifyMaxStacks;
    public StatusEffectData effect;
    public int newMaxStacks;
    public override void Generate(EffectContext ctx, List<CombatIntent> intents)
    {
        StatusEffectOverrides.SetMaxStacks(effect, newMaxStacks);
    }
}
