using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileData 
{
    public Dictionary<StatType, float> stats;
    public StatsPreset preset;
    public GameObject prefab;
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
    public bool isHit;
    public bool trackEnemy;
    public bool aimAtEnemy;
    public bool randomDirection;


    public ProjectileData(Weapon original)
    {
        stats = new Dictionary<StatType, float>();
        preset = original.statPreset;

        foreach (var kvp in original.stats.cachedStats)
        {
            stats.Add(kvp.Key, kvp.Value);
        }

        prefab = original.prefab;
        pattern = original.pattern;
        behaviour = original.behaviour;
        isPiercing = original.isPiercing;
        owner = original.owner;
        hitInterval = original.hitInterval;
        effects = original.effects;
        isHit = original.isHit;
        trackEnemy = original.trackEnemy;
        aimAtEnemy = original.aimAtEnemy;
        randomDirection = original.randomDirection;

    }

    public ProjectileData(ProjectileData original)
    {
        stats = original.stats;
        prefab = original.prefab;
        pattern = original.pattern;
        behaviour = original.behaviour;
        isPiercing = original.isPiercing;
        owner = original.owner;
        hitInterval = original.hitInterval;
        effects = original.effects;
        isHit = original.isHit;
        trackEnemy = original.trackEnemy;   
        aimAtEnemy = original.aimAtEnemy;
        randomDirection = original.randomDirection;
    }

    public ProjectileData Clone()
    {
        return (ProjectileData)this.MemberwiseClone();
    }

}
