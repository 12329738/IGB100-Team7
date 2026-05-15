using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectInstance
{
    public EffectEntryNode entryNode;
    public EffectState state;
    public EffectExecutor executor;
    public IDamageSource effectHolder;
    private List<CombatIntent> intents = new();
    private EffectContext context = new();

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

        var node = entryNode.conditions.FirstOrDefault(x => x.triggerEvent == CombatEvent.OnEffectGained);
        if (node != null)
            foreach (EffectNodeData effect in entryNode.effectData)
                effect.Execute(context, intents);
    }

    public void Tick(float now, EffectExecutor executor)
    {
        if (!entryNode.hasTick)
            return;

        if (now - state.lastTickTime < entryNode.tickInterval)
            return;

        state.lastTickTime = now;

        context.Reset();

        context.damageSource = state.source;
        context.target = state.target;
        context.trigger = CombatEvent.OnTick;
        context.stacks = state.stacks;
        
        intents.Clear();
        entryNode.Execute(context, intents);
        entryNode.Modify(context, ref intents);


        executor.Execute(intents);
        
        
    }
}