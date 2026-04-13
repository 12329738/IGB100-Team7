using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class Weapon     
{
    public WeaponData baseData;
    //WeaponData currentData;
    public List<List<Upgrade>> upgradeList;
    public int currentLevel = 1;
    public Stats stats;
    public List<StatModifier> modifiers;

    float cooldownTimer;

    public void Initialize()
    {
        stats = baseData.stats;
        stats.Initialize();      
        modifiers = new List<StatModifier>();

        if (stats.GetStat(StatType.Duration).currentValue <= 0)
        {
            Attack();
        }
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

        for (int i = 0; i < stats.GetStat(StatType.Projectiles).currentValue; i++)
        {
            GameObject obj = ObjectPool.instance.GetObject(baseData.prefab);

            obj.transform.position = GameManager.instance.player.transform.position;
            obj.transform.rotation = Quaternion.identity;


            Projectile proj = obj.GetComponent<Projectile>();
            proj.Activate(baseData, stats, Team.Player);
        }

    }

}
