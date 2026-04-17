using UnityEngine;

[System.Serializable]
public abstract class EffectNodeConfig
{
    public abstract void Execute(EffectContext ctx);

}