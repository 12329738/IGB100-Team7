using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileData 
{
    public Stats stats;
    public GameObject prefab;
    [UnityEngine.Range(0f, 360f)]
    public float randomAngleVariance;
    [UnityEngine.Range(0f, 360f)]
    public float baseDirection;
    public Vector3 finalDirection;
    public bool isPiercing;
    public WeaponBehaviour behaviour;
    public ProjectilePattern pattern;
    public Team team;
    public GameObject owner;
    public float hitInterval;
    public List<EffectEntryNode> effects;

    public ProjectileData(Weapon original)
    {
        stats = original.stats; 
        prefab = original.prefab;
        pattern = original.pattern;
        behaviour = original.behaviour;
        randomAngleVariance = original.randomAngleVariance;
        isPiercing = original.isPiercing;
        owner = original.owner;
        hitInterval = original.hitInterval;
        effects = original.effects;
    }

    public ProjectileData()
    {

    }

}
