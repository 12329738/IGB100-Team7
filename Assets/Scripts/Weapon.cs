using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Item/Weapon")]
public class Weapon : Item, IEventHandler
{
    [HideInInspector]
    public Stats weaponStats;
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
    public virtual List<StatModifier> weaponModifiers { get; set; }
    public EventHandler eventHandler {  get; set; }
    private EffectHandler effectHandler;
    float cooldownTimer;
    public bool isHit;
    float projectileRemainder;

    public void Initialize()
    {
        owner = GameManager.instance.player.gameObject;

        weaponStats.Initialize(baseStats);
        weaponModifiers = new List<StatModifier>();

        eventHandler = new EventHandler();
        effectHandler = new EffectHandler(eventHandler);

        foreach (EffectEntryNode node in effects)
        {
            effectHandler.AddToMap(node);
        }


        if (weaponStats.GetStat(StatType.Duration) <= 0)

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


    public void Tick(float deltaTime)
    {

        if (weaponStats.GetStat(StatType.Duration) <= 0)

        {
            return;
        }

        cooldownTimer -= deltaTime;

        if (cooldownTimer <= 0f)
        {
            Attack();

            cooldownTimer = weaponStats.GetStat(StatType.Cooldown);

        }
    }

    internal void Attack()
    {
        ProjectileData data = BuildProjectileData();


        float total = weaponStats.GetStat(StatType.ProjectileCount) + projectileRemainder;
        int count = Mathf.FloorToInt(total);
        projectileRemainder = total - count;
        Debug.Log($"{this} has a projectile count of {weaponStats.GetStat(StatType.ProjectileCount)}, spawning {count} projectiles");
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



