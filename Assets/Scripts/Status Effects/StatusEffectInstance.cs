using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class StatusEffectInstance : IDamageSource, IModifierProvider 
{
    public StatusEffectDataInstance data;
    public List<EffectInstance> runtimes = new();
    public EffectContext context;
    public EffectState state;
    public StatusEffectManager effectManager;
    private List<CombatIntent> intents = new();
    public Entity _owner;
    public Entity owner { get => _owner; set => _owner = value; }
    public DamageSourceDefinition definition { get => data.definition; set => data.definition = value; }
    public float hitInterval { get => data.tickInterval;
        set => data.tickInterval = value;
    }

    public readonly ModifierProvider provider = new ModifierProvider();

    public void AddModifier(StatModifier mod)
    {
        Debug.Log($"ADDING MOD: {mod.stat} {mod.amount} TO {this}");
        provider.AddModifier(mod);
    }


    public void RemoveModifier(StatModifier mod)
        => provider.RemoveModifier(mod);

    public event Action OnDirty
    {
        add => provider.OnDirty += value;
        remove => provider.OnDirty -= value;
    }

    public List<StatModifier> Modifiers => provider.Modifiers;

    public Guid guid { get; set; }

    public StatusEffectInstance(StatusEffectDataInstance data, IDamageSource source, IDamageSource target, StatusEffectManager manager)
    {
        guid = Guid.NewGuid();
        effectManager = manager;
        this.data = data;
        owner = GameManager.instance.player;

        context = new EffectContext
        {
            damageSource = source,
            target = target,
            stacks = 0,

        };
        context.damageSource.definition = definition;

        state = new EffectState
        {
            source = source,
            target = target,
            stacks = 1,
            startTime = Time.time,
            lastTickTime = Time.time,
        };
        AddStack(1);
       

        foreach (var entry in data.entries)
        {
            AddRuntime(entry);
        }

        provider.Modifiers.AddRange(data.modifiers);
        GameManager.instance.statusEffectRegistry.AddInstance(this);


    }

    public void OnApply(StatusEffectManager manager)
    {
        context.trigger = CombatEvent.OnApply;
        ExecuteEffects(context);
    }

    public void AddRuntime(EffectEntryNode entry)
    {
        var runtime = new EffectInstance(
            entry,
            source: context.damageSource,
            target: context.target,
            effectCreator: _owner.GetComponent<IDamageSource>()
        );

        runtimes.Add(runtime);
    }


    public void Tick()
    {
        
        if (Time.time - state.startTime > data.duration)
        {
            Expire();
            return;
        }

        foreach (var runtime in runtimes)
            runtime.Tick(Time.time, GameManager.instance.effectExecutor);

        if (!data.hasTick)
            return;
        if (Time.time - state.lastTickTime < data.tickInterval)
            return;

        state.lastTickTime = Time.time;

        context.trigger = CombatEvent.OnTick;
        ExecuteEffects(context);
                  
    }

    public void ExecuteEffects(EffectContext context)
    {
        intents.Clear();
        foreach (EffectInstance instance in runtimes)
        {
            instance.entryNode.Execute(context, intents);                      
        }

        GameManager.instance.effectExecutor.Execute(intents);        
    }


    public void AddStack(int amount)
    {
            
        context.stacks += amount;
        if (context.stacks > data.maxStacks)
            context.stacks = data.maxStacks;
        Refresh();
        //Debug.Log($"{context.target} has {context.stacks} stacks of {data.name}");
        context.trigger = CombatEvent.OnStackCount;
        ExecuteEffects(context);
        GameManager.instance.effectHandler.Dispatch(context);    
    }

    private void Refresh()
    {
        state.startTime = Time.time;
    }



    public void Remove() { }

    public void Expire()
    {   
        provider.RemoveAllModifiers();
        effectManager.RemoveStatus(this);
    }


}