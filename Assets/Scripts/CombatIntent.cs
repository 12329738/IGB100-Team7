using UnityEngine;

public struct CombatIntent
{
    public EffectIntent type;
    public IDamageSource source;
    public IDamageSource target;
    public float value;
    public int stacks;
    public EffectContext context;
    public int wave;
}