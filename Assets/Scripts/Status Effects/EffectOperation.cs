using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EffectOperation
{
    public abstract EffectIntent Type { get; }

    public abstract void Generate(EffectContext ctx, List<CombatIntent> intents);


}