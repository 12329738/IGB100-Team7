using System.Collections.Generic;
using UnityEngine;

public class EffectInstance
{
    public EffectEntryNode entryNode;
    public EffectState state;
    public EffectExecutor executor;
    //public HashSet<CombatEvent> subscribedEvents = new();
    public GameObject owner;

    public EffectInstance(EffectEntryNode def, GameObject source, GameObject target, GameObject owner)
    {
        entryNode = def;
        executor = GameManager.instance.effectExecutor;
        state = new EffectState
        {
            source = source,
            target = target,
            startTime = Time.time,
            lastTickTime = Time.time,
            stacks = 1
        };

      
        this.owner = owner;
    }

    public void Tick(float now, EffectExecutor executor)
    {
        if (!entryNode.hasTick)
            return;

        if (now - state.lastTickTime < entryNode.tickInterval)
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
        entryNode.Execute(ctx, intents);
        entryNode.Execute(ctx, ref intents);


        executor.Execute(intents);
        
        
    }
}