
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Member;
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

    public AudioClip expireSoundEffect;



    private EffectContext context = new();
    private List<CombatIntent> intents = new();
    private Team ownerTeam;
    private readonly Dictionary<GameObject, TargetContact> targets = new();
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

    void Update()
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

        CheckTargets();
    }

    private void LateUpdate()
    {
        foreach (var kv in targets.Values.ToArray())
        {
            var t = kv;
            if (!t.gameObject.activeSelf)
                targets.Remove(t.gameObject);
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
        context.Reset();
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
        ownerTeam = owner.GetComponent<IDamageable>().team;
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
            intents.Clear();
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

    private void CheckTargets()
    {

        foreach (var kv in targets.Values.ToArray()) 
        {
            var t = kv;
            if (!t.gameObject.activeSelf)
                targets.Remove(t.gameObject);
            else
            {
                if (!ownerCombat.CheckHitTime(this, t.source))
                    continue;

                TryHit(t.damageable, t.source, t.gameObject);
                RaiseContactEvent(t.damageable, t.source, t.gameObject);
            }
        }
    }

    public void Deactivate()
    {
        if (expireSoundEffect != null)
            AudioManager.instance.PlaySound(expireSoundEffect, transform.position);
        foreach (EffectEntryNode node in data.effects)
        {
            EffectContext context = new EffectContext { damageSource = this, trigger = CombatEvent.OnExpire };
            intents.Clear();
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
        targets.Clear();
        ObjectPool.instance.ReturnObject(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<IDamageable>(out var damageable))
            return;

        if (!other.TryGetComponent<IDamageSource>(out var source))
            return;

        targets[other.gameObject] = (new TargetContact
        {
            gameObject = other.gameObject,
            damageable = damageable,
            source = source
        });
    }

    private void OnTriggerExit(Collider other)
    {
        targets.Remove(other.gameObject);
    }

    private void RaiseContactEvent(IDamageable damageable, IDamageSource source, GameObject target)
    {
        for (int i = 0; i < data.effects.Count; i++)
        {
            var node = data.effects[i];

            for (int j = 0; j < node.conditions.Count; j++)
            {
                if (node.conditions[j].triggerEvent != CombatEvent.OnContact)
                    continue;

                context.Reset();

                context.damageSource = this;
                context.target = source;
                context.trigger = CombatEvent.OnContact;
                

                var intent = new CombatIntent
                {
                    source = this,
                    target = source,
                    context = context
                };

                ownerCombat.TriggerContact(intent);
            }
        }
    }

    private void TryHit(IDamageable damageable, IDamageSource source, GameObject target)
    {
        if (target == data.owner.gameObject)
            return;

        if (damageable.team == ownerTeam)
            return;

        

        context.Reset();
        context.damageSource = this;
        context.target = source;
        context.value = TryGetStat(StatType.Damage);
        context.isHit = data.isHit;


        var intent = new CombatIntent
        {
            value = stats[StatType.Damage],
            source = this,
            target = source,
            context = context
        };

        ownerCombat.DealDamage(intent);

        if (!data.isPiercing)
            Deactivate();
    }

    public float TryGetStat(StatType stat)
    {
        stats.TryGetValue(stat, out var value);
        return value;
    }

    private struct TargetContact
    {
        public GameObject gameObject;
        public IDamageable damageable;
        public IDamageSource source;
    }
}