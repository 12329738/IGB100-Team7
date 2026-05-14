
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


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

    [HideInInspector]
    public virtual StatsPreset statPreset { get; set; }



    public void AddModifier(StatModifier mod)
        => provider.AddModifier(mod);

    public void RemoveModifier(StatModifier mod)
        => provider.RemoveModifier(mod);

    public List<StatModifier> Modifiers => provider.Modifiers;

    public Entity owner { get => data.owner; set => data.owner = value; }
    public DamageSourceDefinition definition { get => data.definition; set => data.definition = value; }
    public float hitInterval { get => data.hitInterval; set => data.hitInterval = value; }
    public Guid guid { get;set; }

    private readonly ModifierProvider provider = new ModifierProvider();
    public float elapsed;

    public void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed >= stats[StatType.Duration])
        {
            Deactivate();
            return;
        }
            

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
        guid = Guid.NewGuid();
        data = d;
        if (owner is Enemy enemy)
            gameObject.layer = LayerMask.NameToLayer("Enemy Projectile");
        else
            gameObject.layer = LayerMask.NameToLayer("Projectile");
        
        stats = new Dictionary<StatType, float>(d.stats);

        if (GetComponentInChildren<SpriteRenderer>() != null)
        {
            visual = GetComponentInChildren<SpriteRenderer>().transform;
            
        }
        ApplyAreaStat(TryGetStat(StatType.Area));

        foreach (EffectEntryNode node in data.effects)
        {
            EffectInstance instance = new EffectInstance(node, this, this,this);
            GameManager.instance.effectHandler.Register(instance);
        }

        ownerCombat = this.data.owner.GetComponent<Entity>().combat;
        if (data.hitMode == HitMode.Instant)
        {
            StartCoroutine(InstantHit());
        }

        
    }

    public void RaiseSpawnEvent()
    {
        foreach (EffectEntryNode node in data.effects)
        {
            EffectContext context = new EffectContext { damageSource = this, trigger = CombatEvent.OnSpawn };
            List<CombatIntent> intents = new List<CombatIntent>();
            if (node.conditions.Any(x => x.triggerEvent == CombatEvent.OnSpawn))
            {
                node.Execute(context, intents);
                node.Modify(context, ref intents);
            }
            GameManager.instance.effectExecutor.Execute(intents);
        }
    }

    IEnumerator InstantHit()
    {
        var shape = GetComponent<IProjectileShape>();
        yield return new WaitForFixedUpdate();
        
        shape.SetCollider(false);
    }


    public void ApplyAreaStat(float area)
    {
        var shape = GetComponent<IProjectileShape>();
        if (shape!= null)
        {
            shape.Initialize();
            shape.ResetSize();
            shape.SetSize(area);
            shape.SetCollider(true);
        }
    }

    public void Deactivate()
    {
        foreach (EffectEntryNode node in data.effects)
        {
            EffectContext context = new EffectContext { damageSource = this, trigger = CombatEvent.OnExpire };
            List<CombatIntent> intents = new List<CombatIntent>();
            if (node.conditions.Any(x=> x.triggerEvent == CombatEvent.OnExpire))
            {
                node.Execute(context, intents);
                node.Modify(context, ref intents);
            }
            GameManager.instance.effectExecutor.Execute(intents);
        }
        GameManager.instance.effectHandler.UnRegister(this);
        elapsed = 0;
        state = null;
        ObjectPool.instance.ReturnObject(gameObject);

    }

    private void OnTriggerStay(Collider other)
    {
        TryHit(other.gameObject);
        RaiseContactEvent(other.gameObject);
    }

    private void RaiseContactEvent(GameObject target)
    {

        foreach (EffectEntryNode node in data.effects)
        {
            if (node.conditions.Any(x => x.triggerEvent == CombatEvent.OnContact))
            {
                var context = new EffectContext
                {
                    damageSource = this,
                    target = target.GetComponent<IDamageSource>(),
                    trigger = CombatEvent.OnContact,
                };

                var intent = new CombatIntent
                {
                    source = this,
                    target = context.target,
                    context = context
                };

                ownerCombat.TriggerContact(intent);
            }

        }
        
    }

    private void TryHit(GameObject target)
    {
        if (target == data.owner.gameObject)
            return;


        if (!target.TryGetComponent<IDamageable>(out var targetDamageable))
            return;
        if (targetDamageable.team == owner.GetComponent<IDamageable>().team)
            return;
        var context = new EffectContext
        {
            damageSource = this,

            target = target.GetComponent<IDamageSource>(),
            value = TryGetStat(StatType.Damage),
            isHit = data.isHit,
        };

        context.damageSource.definition = definition;

        var intent = new CombatIntent
        {
            value = stats[StatType.Damage],
            source = this,
            target = context.target,
            context = context
        };



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