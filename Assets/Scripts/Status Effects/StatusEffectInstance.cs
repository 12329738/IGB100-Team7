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
    public Entity _owner;
    public Entity owner { get => _owner; set => _owner = value; }
    public DamageSourceDefinition definition { get => data.definition; set => data.definition = value; }

    private readonly ModifierProvider provider = new ModifierProvider();
    public List<StatModifier> Modifiers => provider.Modifiers;
    public IModifierProvider modifierProviderSource;
    public void AddModifier(StatModifier mod)
    => provider.AddModifier(mod);

    public void RemoveModifier(StatModifier mod)
        => provider.RemoveModifier(mod);
    public event Action OnDirty
    {
        add => provider.OnDirty += value;
        remove => provider.OnDirty -= value;
    }

    public StatusEffectInstance(StatusEffectDataInstance data, IDamageSource source, IDamageSource target, StatusEffectManager manager)
    {
        modifierProviderSource = (IModifierProvider)source;
        effectManager = manager;
        this.data = data;
        owner = GameManager.instance.player;

        context = new EffectContext
        {
            damageInstanceSource = source,
            target = target,
            origin = data.definition,
            stacks = 0,
            damageSourceOwner = owner
        };

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
            var runtime = new EffectInstance(
                entry,
                source: source,
                target: target,
                effectCreator: _owner.GetComponent<IDamageSource>()
            );

            runtimes.Add(runtime);
        }

    }

    public void OnApply(StatusEffectManager manager)
    {
        context.trigger = CombatEvent.OnApply;
        EmitEffects(context);
    }


    public void Tick()
    {
        
        if (Time.time - state.startTime > data.duration)
        {
            Expire();
            return;
        }
        if (!data.hasTick)
            return;
        if (Time.time - state.lastTickTime < data.tickInterval)
            return;

        state.lastTickTime = Time.time;

        context.trigger = CombatEvent.OnTick;
        EmitEffects(context);
                  
    }

    public void EmitEffects(EffectContext context)
    {
        List<CombatIntent> intents = new List<CombatIntent>();
        foreach (EffectInstance instance in runtimes)
        {

            context.effectInstance = instance;
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
        context.trigger = CombatEvent.OnStackCount;
        EmitEffects(context);
        GameManager.instance.effectHandler.Dispatch(context);    
    }

    private void Refresh()
    {
        state.startTime = Time.time;
    }

    public void HandleEvent(CombatEvent type, EffectContext ctx)
    {
        ExecuteEntries(type, ctx);
    }

    private void ExecuteEntries(CombatEvent type, EffectContext ctx)
    {
        var localCtx = ctx.Clone();
        localCtx.stacks = state.stacks;

        List<CombatIntent> intents = new List<CombatIntent>();
        foreach (var entry in data.entries)
        {
            if (entry.conditions.Any(x=>x.triggerEvent == type))
                continue;

            foreach (var node in entry.effectData)
            {
                node.Execute(localCtx, intents);
            }
            
        }
        GameManager.instance.effectExecutor.Execute(intents);
    }

    public void Remove() { }

    public void Expire()
    {      
        effectManager.RemoveStatus(this);
    }


}