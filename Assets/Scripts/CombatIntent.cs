using UnityEngine;

public struct CombatIntent
{
    public EffectIntent intent;
    public IDamageSource source;
    public IDamageSource target;
    public float value;
    public EffectContext context;
}