using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.Video.VideoPlayer;
[Serializable]
[CreateAssetMenu(menuName = "Item/Weapon")]
public class Weapon : Item, IEventHandler
{
    [HideInInspector]
    public Stats stats;
    public StatsPreset baseStats;
    public GameObject prefab;
    [UnityEngine.Range(0f, 360f)]
    public float randomAngleVariance;
    [UnityEngine.Range(0f, 360f)]
    public float direction;
    public float hitInterval = 1f;
    public bool isPiercing;
    public WeaponBehaviour behaviour;
    public ProjectilePattern pattern;
    [HideInInspector]
    public Upgrade baseUpgrade;
    public GameObject owner; 
    public List<EffectEntryNode> effects;
    float projectileRemainder = 0f;
    public virtual List<StatModifier> modifiers { get; set; }
    public EventHandler eventHandler {  get; set; }
    private EffectHandler effectHandler;
    float cooldownTimer;
    public bool isHit;

    public void Initialize()
    {
        owner = GameManager.instance.player.gameObject;
        stats.preset = baseStats;
        stats.Initialize();      
        modifiers = new List<StatModifier>();
        eventHandler = new EventHandler();
        effectHandler = new EffectHandler(eventHandler);

        foreach (EffectEntryNode node in effects)
        {
            effectHandler.AddToMap(node);
        }

        if (stats.GetStat(StatType.Duration).currentValue <= 0)
        {
            Attack();
        }
    }

    public void CreateBaseUpgrade()
    {
        baseUpgrade = CreateInstance<Upgrade>();
        baseUpgrade.itemType = itemType;
        baseUpgrade.name = name;
        baseUpgrade.description = description;

    }

    public void AddModifier(StatModifier modifier)
    {
        modifiers.Add(modifier);
        ApplyModifiers();
    }


    private void ApplyModifiers()
    {
        stats.ApplyModifiers(modifiers);
    }



    public void Tick(float deltaTime)
    {
        if (stats.GetStat(StatType.Duration).currentValue <= 0)
        {
            return;
        }

        cooldownTimer -= deltaTime;

        if (cooldownTimer <= 0f)
        {
            Attack();
            cooldownTimer = stats.GetStat(StatType.Cooldown).currentValue;
        }
    }

    internal void Attack()
    {
        ProjectileData data = BuildProjectileData();

        float total = stats.GetStat(StatType.ProjectileCount).currentValue + projectileRemainder;
        int count = Mathf.FloorToInt(total);
        projectileRemainder = total - count;
        Debug.Log($"{this} has a projectile count of {stats.GetStat(StatType.ProjectileCount).currentValue}, spawning {count} projectiles");
        GameManager.instance.projectileSpawner.CreateProjectile(data, count);
    }


    public ProjectileData BuildProjectileData()
    {
        return new ProjectileData(this);        
    }

    private void OnValidate()
    {
        if (effects == null) return;

        foreach (var entry in effects)
        {
            if (entry == null) continue;

            entry.Validate();
        }
    }
}



