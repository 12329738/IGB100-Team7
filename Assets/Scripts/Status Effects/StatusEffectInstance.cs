using System;
using System.Collections.Generic;
using UnityEngine;

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

    public StatusEffectInstance(StatusEffectData data, GameObject _source, GameObject _target)
    {
        this.data = data;
        context = new EffectContext
        {
            source = _source,
            target = _target,
            valueId = this
        };

        this.duration = data.duration;
        startTime = Time.time;


        foreach (var entry in data.entries)
        {          
             subscribedEvents.Add(entry.trigger);          
        }

    }

    public void OnApply()
    {
        stacks += 1;
        foreach (var entry in data.entries)
        {
            if (entry.trigger == CombatEvent.OnApply)
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
        context.stacks = stacks;
        foreach (var entry in data.entries)
        {
            if (entry.trigger != CombatEvent.OnTIck)
                continue;

            foreach (var node in entry.effectData)
                {
                    node.Execute(context);
                }
                    
        }
        
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
        for (int i = 0; i < data.entries.Count; i++)
        {
            var entry = data.entries[i];

            if (entry.trigger != type)
                continue;

            ctx.stacks = stacks; 

            foreach (var node in entry.effectData)
            {
                node.Execute(ctx);
            }
        }
    }


    public bool IsExpired() => startTime + duration <= Time.time;

    public void Remove() { }

    public void OnExpire() { }


}