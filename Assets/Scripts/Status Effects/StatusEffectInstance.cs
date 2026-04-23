using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class StatusEffectInstance
{
    public StatusEffectData data;
    public List<EffectInstance> runtimes = new();
    public EffectContext context;
    //public int stacks;
    //public float startTime;
    //public float lastTickTime;
    //public GameObject source;
    //public GameObject target;
    public EffectState state;
    public StatusEffectManager effectManager;

    public StatusEffectInstance(StatusEffectData data, GameObject source, GameObject target, StatusEffectManager manager)
    {
        effectManager = manager;
        this.data = data;

        context = new EffectContext
        {
            source = source,
            target = target,
            stacks = 1,
            origin = data.definition
        };
        state = new EffectState
        {
            source = source,
            target = target,
            stacks = 1,
            startTime = Time.time,
            lastTickTime = Time.time,
        };

        foreach (var entry in data.entries)
        {
            var runtime = new EffectInstance(
                entry,
                source: source,
                target: target,
                owner: source
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
            instance.definition.Execute(context, intents);                      
        }

        GameManager.instance.effectExecutor.Execute(intents);        
        
    }


    public void AddStack(int amount)
    {
        context.stacks += amount;
        if (context.stacks > data.maxStacks)
            context.stacks = data.maxStacks;

        Refresh();
        
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
            if (!entry.triggers.Contains(type))
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