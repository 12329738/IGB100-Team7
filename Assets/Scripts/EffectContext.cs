using UnityEngine;

public class EffectContext
{
    public GameObject source;
    public GameObject target;
    public object damageId;
    public float hitInterval;
    public float deltaTime;

    public bool isHit = true;
    public float damage;
    public bool isCrit;

}