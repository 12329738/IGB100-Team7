
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour, IEventHandler, IModifierProvider
{
    public IProjectileState state;
    public Dictionary<StatType, float> stats;
    StatsPreset preset;
    public ProjectileData data;
    private Combat ownerCombat;
    public EventHandler eventHandler {  get; set; } 
    private EffectHandler effectHandler;
    public Transform visual;

    [SerializeField]
    private StatsPreset _statPreset;

    public virtual StatsPreset statPreset { get => _statPreset; set => _statPreset = value; }

    //private Stats _stats;
    //[HideInInspector]
    //public Stats stats { get => _stats; set => _stats = value; }


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

        foreach (EffectEntryNode node in data.effects)
        {
            effectHandler.AddToMap(node);
        }
        ownerCombat = this.data.owner.GetComponent<Combat>();

        eventHandler = new EventHandler();
        effectHandler = new EffectHandler(eventHandler);

        StopAllCoroutines();

        if (stats[StatType.Duration] > 0)
        {
            StartCoroutine(LifetimeRoutine());
        }

    }



    IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(stats[StatType.Duration]);
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
            damage = stats[StatType.Damage],
            hitInterval = data.hitInterval,
            damageId = this,
            isHit = data.isHit

        };

        ownerCombat.DealDamage(context);

        if (!data.isPiercing)
        {
            Debug.Log($"Deactivating {this} because it collided with {target}");
            Deactivate();
        }
        
    }
}