using System.Collections.Generic;
using UnityEngine;

public class EffectInstance
{
    public EffectEntryNode entryNode;
    public EffectState state;
    public EffectExecutor executor;
    public IDamageSource effectHolder;

    public EffectInstance(EffectEntryNode def, IDamageSource source, IDamageSource target, IDamageSource effectCreator)
    {
        entryNode = def;
        executor = GameManager.instance.effectExecutor;
        effectHolder = target;
        state = new EffectState
        {
            source = source,
            target = target,
            startTime = Time.time,
            lastTickTime = Time.time,
            stacks = 1
        };


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
            damageSource = state.source,
            target = state.target,
            trigger = CombatEvent.OnTick,
            stacks = state.stacks
        };
        List<CombatIntent> intents = new List<CombatIntent>();
        entryNode.Execute(ctx, intents);
        entryNode.Modify(ctx, ref intents);


        executor.Execute(intents);
        
        
    }
}