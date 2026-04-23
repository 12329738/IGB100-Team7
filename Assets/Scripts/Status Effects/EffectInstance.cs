using System.Collections.Generic;
using UnityEngine;

public class EffectInstance
{
    public EffectEntryNode definition;
    public EffectState state;
    public EffectExecutor executor;
    public HashSet<CombatEvent> subscribedEvents = new();
    public GameObject owner;

    public EffectInstance(EffectEntryNode def, GameObject source, GameObject target, GameObject owner)
    {
        definition = def;
        executor = GameManager.instance.effectExecutor;
        state = new EffectState
        {
            source = source,
            target = target,
            startTime = Time.time,
            lastTickTime = 0,
            stacks = 1
        };

        foreach (var t in def.triggers)
            subscribedEvents.Add(t);
        this.owner = owner;
    }

    public void Tick(float now, EffectExecutor executor)
    {
        if (!definition.hasTick)
            return;

        if (now - state.lastTickTime < definition.tickInterval)
            return;

        state.lastTickTime = now;

        var ctx = new EffectContext
        {
            source = state.source,
            target = state.target,
            trigger = CombatEvent.OnTick,
            stacks = state.stacks
        };
        List<CombatIntent> intents = new List<CombatIntent>();
        definition.Execute(ctx, intents);

       
         executor.Execute(intents);
        
        
    }
}