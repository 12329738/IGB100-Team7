using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    Dictionary<CombatEvent, List<EffectEntryNode>> eventMap = new();
    private EventHandler eventHandler;

    private void Awake()
    {
        eventHandler = GetComponent<EventHandler>();
    }

    private void OnEnable()
    {
        eventHandler.OnEvent += HandleEvent;
    }

    private void OnDisable()
    {
        eventHandler.OnEvent -= HandleEvent;
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