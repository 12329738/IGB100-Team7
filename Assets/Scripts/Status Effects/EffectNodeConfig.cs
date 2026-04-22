using UnityEngine;

[System.Serializable]
public abstract class EffectNodeConfig
{
    public abstract EffectNodeType Type { get; }
    public abstract void Execute(EffectContext ctx);
    public bool onlyAffectEnemies = false;
    public bool onlyAffectAllies = false;
}