using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
[Serializable]
[CreateAssetMenu(menuName = "Item/Weapon")]
public class Weapon : Item
{
    [HideInInspector]
    public Stats stats;
    public StatsPreset baseStats;
    public GameObject prefab;
    [UnityEngine.Range(0f, 360f)]
    public float randomAngleVariance;
    [UnityEngine.Range(0f, 360f)]
    public float direction;

    public bool hasKnockback;
    public float knockbackMagnitute;
    public bool isPiercing;
    public WeaponBehaviour behaviour;
    public ProjectilePattern pattern;
    [HideInInspector]
    public Upgrade baseUpgrade;
    public virtual List<StatModifier> modifiers { get; set; }
    float cooldownTimer;

    public void Initialize()
    {
        stats.preset = baseStats;
        stats.Initialize();      
        modifiers = new List<StatModifier>();

        if (stats.GetStat(StatType.Duration).currentValue <= 0)
        {
            Attack();
        }
    }

    public void CreateBaseUpgrade()
    {
        baseUpgrade = new Upgrade();
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

    private IEnumerator AttackCoroutine(float cooldown)
    {
        while (true)
        {
            Attack();

            yield return new WaitForSeconds(cooldown);
        }
    }

    internal void Attack()
    {
        ProjectileData data = BuildProjectileData();

        GameManager.instance.projectileSpawner.CreateProjectile(data);
    }


    public ProjectileData BuildProjectileData()
    {
        return new ProjectileData
        {
            stats = stats,
            prefab = prefab,
            randomAngleVariance = randomAngleVariance,
            baseDirection = direction,
            hasKnockback = hasKnockback,
            knockbackMagnitude = knockbackMagnitute,
            isPiercing = isPiercing,
            behaviour = behaviour,
            pattern = pattern,
            team = Team.Player,
        };
        
    }
}



