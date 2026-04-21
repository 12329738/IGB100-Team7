using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Item/Weapon")]
public class Weapon : Item, IEventHandler, IStats
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
    public WeaponBehaviour behaviour;
    public ProjectilePattern pattern;
    [HideInInspector]
    public Upgrade baseUpgrade;
    public GameObject owner; 
    public List<EffectEntryNode> effects;

    public EventHandler eventHandler {  get; set; }
    private EffectHandler effectHandler;
    float cooldownTimer;
    public bool isHit;
    float projectileRemainder;
    public bool stickToPlayer;
    public bool randomDirection;

    [SerializeField]
    private StatsPreset _statPreset;

    public virtual StatsPreset statPreset { get => _statPreset; set => _statPreset = value; }

    private Stats _stats;
    [HideInInspector]
    public Stats stats { get => _stats; set => _stats = value; }
    public void Initialize()
    {
        owner = GameManager.instance.player.gameObject;
        stats = new Stats();
        stats.Initialize(baseStats);
        stats.modifierSources = GameManager.instance.player.stats.modifierSources;
        Dictionary<object, List<StatModifier>> dict = new Dictionary<object, List<StatModifier>>();
        stats.AddModifierSource(this, dict);
        
        
  
        eventHandler = new EventHandler();
        effectHandler = new EffectHandler(eventHandler);

        foreach (EffectEntryNode node in effects)
        {
            effectHandler.AddToMap(node);
        }


        if (stats.GetStat(StatType.Duration) <= 0)

        {
            Spawnprojectiles();
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

        if (stats.GetStat(StatType.Duration) <= 0)

        {
            return;
        }

        cooldownTimer -= deltaTime;

        if (cooldownTimer <= 0f)
        {
            Spawnprojectiles();

            cooldownTimer = stats.GetStat(StatType.Cooldown);

        }
    }

    internal void Spawnprojectiles()
    {
        ProjectileData data = BuildProjectileData();


        float total = stats.GetStat(StatType.ProjectileCount) + projectileRemainder;
        int count = Mathf.FloorToInt(total);
        projectileRemainder = total - count;
        Debug.Log($"{this} has a projectile count of {stats.GetStat(StatType.ProjectileCount)}, spawning {count} projectiles");
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

    internal void AddModifier(Guid id, StatModifier modifier)
    {
        modifiers.Add(id, modifier);
        stats.MarkDirty();
    }
}



