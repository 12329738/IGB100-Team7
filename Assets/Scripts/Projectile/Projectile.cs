
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Member;


public class Projectile : MonoBehaviour, IModifierProvider, IDamageSource
{
    [HideInInspector]
    public IProjectileState state;
    [HideInInspector]
    public Dictionary<StatType, float> stats;
    [HideInInspector]
    public ProjectileData data;
    private Combat ownerCombat;
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

    public Entity owner { get => data.owner; set => data.owner = value; }
    public DamageSourceDefinition definition { get => data.definition; set => data.definition = value; }

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

        foreach (EffectEntryNode node in data.effects)
        {
            EffectInstance instance = new EffectInstance(node, this, this,this);
            GameManager.instance.effectHandler.Register(instance);
        }

        ownerCombat = this.data.owner.GetComponent<Entity>().combat;

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
            EffectContext context = new EffectContext { damageSource = this, damageSourceOwner = data.owner, trigger = CombatEvent.OnExpire };
            List<CombatIntent> intents = new List<CombatIntent>();
            if (node.conditions.Any(x=> x.triggerEvent == CombatEvent.OnExpire))
            {
                node.Execute(context, intents);
                node.Modify(context, ref intents);
            }
            GameManager.instance.effectExecutor.Execute(intents);
        }
        GameManager.instance.effectHandler.UnRegister(this);
        ObjectPool.instance.ReturnObject(gameObject);

    }

    private void OnTriggerStay(Collider other)
    {
        TryHit(other.gameObject);

    }

    private void TryHit(GameObject target)
    {
        if (target == data.owner.gameObject)
            return;

        if (!target.TryGetComponent<IDamageable>(out var targetDamageable))
            return;

        var context = new EffectContext
        {
            damageSource = this,
            damageSourceOwner = data.owner,
            target = target.GetComponent<IDamageSource>(),
            value = TryGetStat(StatType.Damage),
            hitInterval = data.hitInterval,
            isHit = data.isHit,
            sourceInstanceId = this.GetInstanceID()
        };

        var intent = new CombatIntent
        {
            value = stats[StatType.Damage],
            source = this,
            target = context.target,
            context = context
        };

        //context.sourceInstanceId = context.source.GetInstanceID();

        ownerCombat.DealDamage(intent);

        if (!data.isPiercing)
        {
            Deactivate();
        }
        
    }

    public float TryGetStat(StatType stat)
    {
        stats.TryGetValue(stat, out var value);
        return value;
    }
}