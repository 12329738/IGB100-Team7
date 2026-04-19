using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class StatusEffectManager : MonoBehaviour
{

    private Dictionary<StatusEffectData, List<StatusEffectInstance>> activeEffects = new();
    Dictionary<CombatEvent, List<StatusEffectInstance>> eventMap = new();
    EventHandler handler;
    public void Initialize(EventHandler eventHandler)
    {
        handler = eventHandler;
        handler.OnEvent += HandleEvent;
    }

    public void ApplyEffect(StatusEffectData data, GameObject source)
    {

        if (source.GetComponent<IEventHandler>() is Weapon)
        {
            source = GameManager.instance.player.gameObject;
        }

        var instance = new StatusEffectInstance(data, source, gameObject);
        instance.OnApply();

        if (!activeEffects.TryGetValue(data, out var list))
        {
            list = new List<StatusEffectInstance>();
            activeEffects[data] = list;
        }

        list.Add(instance);

        foreach (var e in instance.subscribedEvents)
        {
            if (!eventMap.TryGetValue(e, out var handlers))
            {
                handlers = new List<StatusEffectInstance>();
                eventMap[e] = handlers;
            }

            handlers.Add(instance);
        }

        Debug.Log($"{gameObject.GetComponent<Entity>()} gained status {data.name}");
    }

    public float GetTotalValue(StatusEffectData data)
    {
        if (!activeEffects.TryGetValue(data, out var list))
            return 0;

        float total = 0;

        foreach (var e in list)
            total++; 

        return total;
    }

    private void Update()
    {
        CheckStatusEffectDurations();     
    }

    private void CheckStatusEffectDurations()
    {
        var keysToRemove = new List<StatusEffectData>();

        foreach (var kvp in activeEffects)
        {
            var effect = kvp.Value;

            for (int i = effect.Count - 1; i >= 0; i--)
            {
                var instance = effect[i];

                instance.Tick();

                if (instance.IsExpired())
                {
                    instance.OnExpire();
                    Debug.Log($"Status effect {instance} expired on {gameObject.name}");
                    effect.RemoveAt(i);

                    UnsubscribeFromEvents(instance);
                }
            }

            if (effect.Count == 0)
            {
                keysToRemove.Add(kvp.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            activeEffects.Remove(key);
        }
    }

    private void UnsubscribeFromEvents(StatusEffectInstance instance)
    {
        foreach (var e in instance.subscribedEvents)
        {
            if (eventMap.TryGetValue(e, out var handlers))
            {
                handlers.Remove(instance);

                if (handlers.Count == 0)
                {
                    eventMap.Remove(e);
                }
            }
        }
    }


    private void HandleEvent(CombatEvent type, EffectContext ctx)
    {
        if (!eventMap.TryGetValue(type, out var list))
            return;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].HandleEvent(type, ctx);
        }
    }
}