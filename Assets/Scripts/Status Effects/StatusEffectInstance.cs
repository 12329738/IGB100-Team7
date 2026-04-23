using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class StatusEffectInstance
{
    public StatusEffectData data;
    public EffectContext context;
    private float duration;
    public int stacks;
    private float startTime;
    public HashSet<CombatEvent> subscribedEvents = new();
    public Action<StatusEffectInstance> OnApplied;
    public Action<StatusEffectInstance> OnExpired;
    public Action<StatusEffectInstance, int> OnStacksChanged;
    Guid Id = Guid.NewGuid();

    public float LastTickTime { get; internal set; }

    public StatusEffectInstance(StatusEffectData data, GameObject _source, GameObject _target)
    {
        this.data = data;
        context = new EffectContext
        {
            source = _source,
            target = _target,
        };
        context.effectInstanceId = Id;
        context.sourceInstanceId = context.source.GetInstanceID();
        this.duration = data.duration;
        startTime = Time.time;


        foreach (var entry in data.entries)
        {          
            foreach (CombatEvent trigger in entry.triggers)
             subscribedEvents.Add(trigger);          
        }

    }

    public void OnApply()
    {
        stacks++;
        foreach (var entry in data.entries)
        {
            if (entry.triggers.Contains(CombatEvent.OnApply))
            {

                foreach (var node in entry.effectData)
                {
                    node.Execute(context);
                }
                
            }
        }
    }


    public void Tick()
    {
        ExecuteEntries(CombatEvent.OnTick, context);
    }

    public void AddStack(int amount)
    {
        stacks += amount;
        if (stacks > data.maxStacks)
            stacks = data.maxStacks;

        Refresh();

        OnStacksChanged?.Invoke(this, stacks);
        

    }

    private void Refresh()
    {
        startTime = Time.time;
    }

    public void HandleEvent(CombatEvent type, EffectContext ctx)
    {
        ExecuteEntries(type, ctx);
    }

    private void ExecuteEntries(CombatEvent type, EffectContext ctx)
    {
        var localCtx = ctx.Clone();
        localCtx.stacks = stacks;

        foreach (var entry in data.entries)
        {
            if (!entry.triggers.Contains(type))
                continue;

            foreach (var node in entry.effectData)
            {
                node.Execute(localCtx);
            }

            GameManager.instance.effectExecutor.Execute(localCtx);
        }
    }
    public bool IsExpired() => startTime + duration <= Time.time;

    public void Remove() { }

    public void OnExpire() { }


}