
using System.Collections;
using UnityEngine;


public class Projectile : MonoBehaviour, IEventHandler
{
    public IProjectileState state;
    public ProjectileData projectileData;
    private Combat ownerCombat;
    public EventHandler eventHandler {  get; set; } 
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


        foreach (EffectEntryNode node in data.effects)
        {
            effectHandler.AddToMap(node);
        }

        this.projectileData = new ProjectileData(data);
        ownerCombat = this.projectileData.owner.GetComponent<Combat>();

        eventHandler = new EventHandler();
        effectHandler = new EffectHandler(eventHandler);

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
        foreach (EffectEntryNode node in projectileData.effects)
        {
            if (node.trigger == CombatEvent.OnExpire)
            {
                EffectContext context = new EffectContext
                {
                    source = this.gameObject
                };
                node.Execute(context);
            }
        }
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
            damageId = this,
            isHit = projectileData.isHit

        };

        ownerCombat.DealDamage(context);

        if (!projectileData.isPiercing)
        {
            Deactivate();
        }
        
    }
}