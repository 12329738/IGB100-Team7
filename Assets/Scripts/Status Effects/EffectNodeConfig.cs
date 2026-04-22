using UnityEngine;

[System.Serializable]
public abstract class EffectNodeConfig
{
    public abstract EffectType Type { get; }
    public abstract void Execute(EffectContext ctx);


}