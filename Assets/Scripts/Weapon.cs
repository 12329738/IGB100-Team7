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
    public float angleVariance;
    [UnityEngine.Range(0f, 360f)]
    public float direction;
    public bool isPiercing;
    public WeaponBehaviour behaviour;
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

        for (int i = 0; i < stats.GetStat(StatType.ProjectileCount).currentValue; i++)
        {
            GameObject obj = ObjectPool.instance.GetObject(prefab);

            obj.transform.position = GameManager.instance.player.transform.position;
            obj.transform.rotation = Quaternion.identity;


            Projectile proj = obj.GetComponent<Projectile>();
            proj.Activate(this, Team.Player);
        }

    }

}
