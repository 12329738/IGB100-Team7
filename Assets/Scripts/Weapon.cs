using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Item/Weapon")]
public class Weapon : Item, IModifierReceiver
{
    [HideInInspector]

    public StatsPreset baseStats;
    public GameObject prefab;
    [UnityEngine.Range(0f, 360f)]
    public float randomAngleVariance;
    [UnityEngine.Range(0f, 360f)]
    public float direction;
    public float hitInterval = 1f;
    public bool isPiercing;
    public bool trackEnemy;
    public bool aimAtEnemy;
    
    [HideInInspector]
    public ItemUpgrade baseUpgrade;
    [HideInInspector]
    public Entity owner => GameManager.instance.player;
    
    [HideInInspector]
    public EventHandler eventHandler {  get; set; }

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
    public List<Weapon> subWeaponData;
    [HideInInspector]
    public List<Weapon> subWeaponInstances = new List<Weapon>();
    public DamageSourceDefinition definition;

    public virtual StatsPreset statPreset { get => _statPreset; set => _statPreset = value; }

    private Stats _stats;
    [HideInInspector]
    public Stats stats { get => _stats; set => _stats = value; }

    public List<EffectEntryNode> effects;
    public void Initialize()
    {
        if (stats != null) return; 
        stats = new Stats();
        stats.Initialize(statPreset);
        stats.AddModifierProvider(GameManager.instance.player.provider);
        stats.AddModifierProvider(this.provider);

        foreach(Weapon weapon in subWeaponData)
        {
            Weapon subWeapon = Instantiate(weapon);
            subWeapon.Initialize(this);
            subWeaponInstances.Add(subWeapon);
        }

    }

    public void Initialize(Weapon weapon)
    {
        if (stats != null) return;
        stats = new Stats();
        stats.Initialize(statPreset);
        stats.AddModifierProvider(GameManager.instance.player.provider);
        stats.AddModifierProvider(weapon);
        stats.AddModifierProvider(this.provider);
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

        if (stats.GetStat(StatType.Duration) <= 0)

        {
            return;
        }

        cooldownTimer -= deltaTime;

        if (cooldownTimer <= 0f)
        {
            cooldownTimer = stats.GetStat(StatType.Cooldown);
            SpawnProjectiles();

            

        }
    }

    internal void SpawnProjectiles()
    {
        ProjectileData data = BuildProjectileData();


        float total = stats.GetStat(StatType.ProjectileCount) + projectileRemainder;
        int count = Mathf.FloorToInt(total);
        projectileRemainder = total - count;
        if (count == 0)
            count = 1;
        GameManager.instance.projectileSpawner.CreateProjectile(data, count);

    }

    public void SpawnProjectiles(EffectContext context)
    {
        ProjectileData data = BuildProjectileData();
        data.owner = owner;

        float total = stats.GetStat(StatType.ProjectileCount) + projectileRemainder;
        int count = Mathf.FloorToInt(total);
        projectileRemainder = total - count;
        if (count == 0)
            count = 1;
        if (context.damageInstanceSource is Component comp)
        {
            GameManager.instance.projectileSpawner.CreateProjectile(data, count, comp.gameObject.transform.position);
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



