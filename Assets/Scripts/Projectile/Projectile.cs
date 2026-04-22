
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour, IEventHandler, IModifierProvider
{
    [HideInInspector]
    public IProjectileState state;
    [HideInInspector]
    public Dictionary<StatType, float> stats;
    [HideInInspector]
    public ProjectileData data;
    private Combat ownerCombat;
    [HideInInspector]
    public EventHandler eventHandler {  get; set; } 
    private EffectHandler effectHandler;
    [HideInInspector]
    public Transform visual;

    [SerializeField]
    private StatsPreset _statPreset;
    [HideInInspector]
    public virtual StatsPreset statPreset { get => _statPreset; set => _statPreset = value; }



    public void AddModifier(StatModifier mod)
        => provider.AddModifier(mod);

    public void RemoveModifier(StatModifier mod)
        => provider.RemoveModifier(mod);

    public List<StatModifier> Modifiers => provider.Modifiers;
    private readonly ModifierProvider provider = new ModifierProvider();

    public void Update()
    {      
        if (data.behaviour != null)
        {
            data.behaviour.Move(this, state);
        }
    }
    public event Action OnDirty
    {
        add => provider.OnDirty += value;
        remove => provider.OnDirty -= value;
    }

    public void Initialize(ProjectileData d)
    {
        visual = GetComponentInChildren<SpriteRenderer>().transform;
        data = d;
        stats =data.stats;

        transform.localScale *= TryGetStat(StatType.Area);

        eventHandler = new EventHandler();
        effectHandler = new EffectHandler(eventHandler);

        foreach (EffectEntryNode node in data.effects)
        {
            effectHandler.AddToMap(node);
        }
        ownerCombat = this.data.owner.GetComponent<Combat>();

        StopAllCoroutines();

        if (stats[StatType.Duration] > 0)
        {
            StartCoroutine(LifetimeRoutine());
        }

    }



    IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(TryGetStat(StatType.Duration));
        Deactivate();
    }

    public void Deactivate()
    {
        foreach (EffectEntryNode node in data.effects)
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
        ObjectPool.instance.ReturnObject(data.prefab, gameObject);

    }

    private void OnTriggerStay(Collider other)
    {
        TryHit(other.gameObject);

    }

    private void TryHit(GameObject target)
    {
        if (target == data.owner)
            return;

        if (!target.TryGetComponent<IDamageable>(out var targetDamageable))
            return;

        var context = new EffectContext
        {
            source = gameObject,
            target = target,
            value = TryGetStat(StatType.Damage),
            hitInterval = data.hitInterval,
            valueId = this,
            isHit = data.isHit

        };

        ownerCombat.DealDamage(context);

        if (!data.isPiercing)
        {
            Debug.Log($"Deactivating {this} because it collided with {target}");
            Deactivate();
        }
        
    }

    public float TryGetStat(StatType stat)
    {
        stats.TryGetValue(stat, out var value);
        return value;
    }
}