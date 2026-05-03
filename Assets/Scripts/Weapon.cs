using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Item/Weapon")]
public class Weapon : Item, IModifierReceiver
{
    [HideInInspector]

    public StatsPreset baseStats;
    public GameObject prefab;
    [UnityEngine.Range(0f, 360f)]
    public float direction;
    public float hitInterval = 1f;
    public bool isPiercing;
    public bool trackEnemy;
    public bool aimAtEnemy;
    public bool spawnOnTarget;
    public HitMode hitMode;
    [HideInInspector]
    public ItemUpgrade baseUpgrade;
    [HideInInspector]
    public Entity owner;
    


    float cooldownTimer;
    public bool isHit;
    float projectileRemainder;
    public bool stickToPlayer;
    public bool randomDirection;
    public WeaponBehaviour behaviour;
    public ProjectilePattern pattern;
    [SerializeField]
    private StatsPreset _statPreset;
    public Guid Id = Guid.NewGuid();
    bool isWeapon = true;
    
    public DamageSourceDefinition definition;
    public List<Weapon> subWeaponData;
    [HideInInspector]
    public List<Weapon> subWeaponInstances = new List<Weapon>();
    public virtual StatsPreset statPreset { get => _statPreset; set => _statPreset = value; }

    private Stats _stats;
    [HideInInspector]
    public Stats stats { get => _stats; set => _stats = value; }

    public List<EffectEntryNode> effects;
    float attackInterval;
    public void Initialize(Entity e)
    {
        owner = e;
        stats = new Stats();
        stats.Initialize(statPreset, definition);
        stats.AddModifierProvider(e.provider);
        stats.AddModifierProvider(provider);

        foreach(Weapon weapon in subWeaponData)
        {
            Weapon subWeapon = Instantiate(weapon);
            subWeapon.Initialize(this, e);
            subWeaponInstances.Add(subWeapon);
        }
        attackInterval = 1f / stats.GetStat(StatType.AttackSpeed);
        cooldownTimer = attackInterval;
    }

    public void Initialize(Weapon weapon, Entity e)
    {
        owner = e;
        if (stats != null) return;
        stats = new Stats();
        stats.Initialize(statPreset,definition);
        stats.AddModifierProvider(e.provider);
        stats.AddModifierProvider(weapon);
        stats.AddModifierProvider(provider);
        attackInterval = 1f / stats.GetStat(StatType.AttackSpeed);
        cooldownTimer = attackInterval;
    }

    public void CreateBaseUpgrade()
    {
        baseUpgrade = CreateInstance<ItemUpgrade>();
        baseUpgrade.itemType = itemType;
        baseUpgrade.name = name;
        baseUpgrade.description = description;

    }


    public void Tick(float deltaTime)
    {

        attackInterval = 1f/stats.GetStat(StatType.AttackSpeed);
        cooldownTimer += deltaTime;

        if (cooldownTimer >= attackInterval)
        {
            cooldownTimer -= attackInterval;
            SpawnProjectiles();

            

        }
    }

    internal void SpawnProjectiles()
    {
        ProjectileData data = BuildProjectileData();


        float total = stats.GetStat(StatType.ProjectileCount) + projectileRemainder;
        int count = Mathf.FloorToInt(total);
        projectileRemainder = total - count;
        if (count < 1)
            count = 1;
        GameManager.instance.projectileSpawner.CreateProjectile(data, count);

    }

    public void SpawnProjectiles(EffectContext context, Vector3 position)
    {
        ProjectileData data = BuildProjectileData();
        data.owner = owner;

        float total = stats.GetStat(StatType.ProjectileCount) + projectileRemainder;
        int count = Mathf.FloorToInt(total);
        projectileRemainder = total - count;
        if (count < 1)
            count = 1;
        if (context.damageSource is Component comp)
        {
            GameManager.instance.projectileSpawner.CreateProjectile(data, count, position);
        }
        

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



