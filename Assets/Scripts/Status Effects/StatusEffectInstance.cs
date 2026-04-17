using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInstance
{
    public StatusEffectData data;
    private GameObject target;
    private GameObject source;
    private float duration;

    public StatusEffectInstance(StatusEffectData data, GameObject source, GameObject target)
    {
        this.data = data;
        this.source = source;
        this.target = target;
        this.duration = data.duration;
        
    }

    public void OnApply()
    {
        foreach (var entry in data.entries)
        {
            if (entry.trigger == CombatEvent.OnApply)
            {
                var ctx = new EffectContext
                {
                    source = source,
                    target = target
                };

                foreach (var node in entry.nodes)
                {
                    node.Execute(ctx);
                }
                
            }
        }
    }


    public void Tick(float dt)
    {

        var ctx = new EffectContext
        {
            source = target,
            target = target,
            deltaTime = dt
        };

        foreach (var entry in data.entries)
        {
            if (entry.trigger == CombatEvent.Tick)
            {
                foreach (var node in entry.nodes)
                {
                    node.Execute(ctx);
                }
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


    public bool IsExpired() => duration <= 0;

    public void Remove() { }


}