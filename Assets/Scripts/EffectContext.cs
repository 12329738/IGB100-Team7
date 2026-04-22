using UnityEngine;

public class EffectContext
{
    public GameObject source;
    public GameObject target;
    public object valueId;
    public float hitInterval;
    public float deltaTime;
    public EffectNodeType effectType;

    public bool isHit = true;
    public float value;
    public bool isCrit;
    public int? stacks;


}