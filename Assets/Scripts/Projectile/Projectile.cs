using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Projectile : MonoBehaviour
{
    public IProjectileState state;
    public ProjectileData projectileData;
    private Combat ownerCombat;
    private IDamageable ownerDamageable;
    private Dictionary<GameObject, float> lastHitTimes = new();
    private EventHandler eventHandler;
    private EffectHandler effectHandler;

    public void Update()
    {      
        if (projectileData.behaviour != null)
        {
            projectileData.behaviour.Move(this, state);
        }
    }


    public void Initialize(ProjectileData data)
    {
        eventHandler = GetComponent<EventHandler>();
        effectHandler = GetComponent<EffectHandler>();

        foreach (EffectEntryNode node in data.effects)
        {
            effectHandler.AddToMap(node);
        }

        this.projectileData = data;

        ownerCombat = this.projectileData.owner.GetComponent<Combat>();
        ownerDamageable = this.projectileData.owner.GetComponent<IDamageable>();

        StopAllCoroutines();

        if (this.projectileData.stats.GetStat(StatType.Duration).currentValue > 0)
        {
            StartCoroutine(LifetimeRoutine());
        }

    }



    IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(projectileData.stats.GetStat(StatType.Duration).currentValue);
        Deactivate();
    }

    void Deactivate()
    {
        EffectContext context = new EffectContext
        {
            source = this.gameObject
        };
        eventHandler.RaiseEvent(CombatEvent.OnExpire, context);
        ObjectPool.instance.ReturnObject(projectileData.prefab, gameObject);

    }

    private void OnTriggerStay(Collider other)
    {
        TryHit(other.gameObject);         
    }

    private void TryHit(GameObject target)
    {
        if (target == projectileData.owner)
            return;

        if (!target.TryGetComponent<IDamageable>(out var targetDamageable))
            return;

        var context = new EffectContext
        {
            source = gameObject,
            target = target,
            damage = projectileData.stats.GetStat(StatType.Damage).currentValue,
            hitInterval = projectileData.hitInterval,
            damageId = this

        };

        ownerCombat.Damage(context);

        if (!projectileData.isPiercing)
        {
            Deactivate();
        }
        
    }
}