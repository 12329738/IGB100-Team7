using UnityEngine;

public struct CombatIntent
{
    public EffectIntent type;
    public GameObject source;
    public GameObject target;
    public float value;
    public int stacks;
    public EffectContext context;
    public int wave;
}