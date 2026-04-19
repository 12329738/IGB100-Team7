using System.Collections.Generic;
using UnityEngine;

public class EffectHandler 
{
    Dictionary<CombatEvent, List<EffectEntryNode>> eventMap = new();
    private EventHandler eventHandler;

    public EffectHandler(EventHandler eventHandler)
    {
        this.eventHandler = eventHandler;
        eventHandler.OnEvent += HandleEvent;
    }

    public void HandleEvent(CombatEvent type, EffectContext ctx)
    {
        if(!eventMap.TryGetValue(type, out var list))
            return;

        foreach (EffectEntryNode entry in list)
        {
            entry.Execute(ctx);
        }
    }

    public void AddToMap(EffectEntryNode effect)
    {
        if (!eventMap.TryGetValue(effect.trigger, out var list))
        {
            list = new List<EffectEntryNode>();
            eventMap[effect.trigger] = list;
        }

        list.Add(effect);
    }

}