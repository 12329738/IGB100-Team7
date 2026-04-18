using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInstance
{
    public StatusEffectData data;
    public EffectContext context;
    private float duration;
    private float startTime;
    public HashSet<CombatEvent> subscribedEvents = new();

    public StatusEffectInstance(StatusEffectData data, GameObject _source, GameObject _target)
    {
        this.data = data;
        context = new EffectContext
        {
            source = _source,
            target = _target,
            damageId = this
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
        foreach (var entry in data.entries)
        {
            if (entry.trigger == CombatEvent.OnApply)
            {

                foreach (var node in entry.nodes)
                {
                    node.Execute(context);
                }
                
            }
        }
    }


    public void Tick(float dt)
    {

        foreach (var entry in data.entries)
        {
            if (entry.trigger != CombatEvent.Tick)
                continue;

            foreach (var node in entry.nodes)
                {
                    node.Execute(context);
                }
                    
        }
        
    }


    public void HandleEvent(CombatEvent type, EffectContext ctx)
    {

        Debug.Log($"Status effect {data.name} trigger event {type}");
        for (int i = 0; i < data.entries.Count; i++)
        {
            var entry = data.entries[i];

            if (entry.trigger != type)
                continue;

            entry.Execute(ctx);
        }
    }


    public bool IsExpired() => startTime + duration <= Time.time;

    public void Remove() { }


}