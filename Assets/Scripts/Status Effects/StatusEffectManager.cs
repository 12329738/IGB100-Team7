using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class StatusEffectManager : MonoBehaviour
{

    private Dictionary<StatusEffectData, List<StatusEffectInstance>> activeEffects = new(); 
    private Dictionary<CombatEvent, List<StatusEffectInstance>> eventIndex = new();
    private List<(StatusEffectData, StatusEffectInstance)> toRemove = new();
    private List<(StatusEffectData, GameObject)> pendingAdds = new();

    EventHandler handler;
    public void Initialize(EventHandler eventHandler)
    {
        handler = eventHandler;
        handler.OnEvent += ctx =>
        {
            Dispatch(ctx);
        };
    }

    private void Update()
    {
        float now = Time.time;
        toRemove.Clear();

        var effectsSnapshot = new List<KeyValuePair<StatusEffectData, List<StatusEffectInstance>>>(activeEffects);

        foreach (var kvp in effectsSnapshot)
        {
            var instanceSnapshot = kvp.Value.ToArray();

            foreach (var instance in instanceSnapshot)
            {
                if (instance.IsExpired())
                {
                    toRemove.Add((kvp.Key, instance));
                    continue;
                }

                if (instance.data.hasTick)
                {
                    if (now - instance.LastTickTime >= instance.data.tickInterval)
                    {
                        instance.LastTickTime = now;
                        instance.Tick();
                    }
                }
            }
        }

        CleanupExpired();
        ProcessPendingAdds();
    }

    private void ProcessPendingAdds()
    {
        foreach (var (data, source) in pendingAdds)
        {
            ApplyEffectImmediate(data, source);
        }

        pendingAdds.Clear();
    }

    public void QueueApplyAffect(StatusEffectData data, GameObject source)
    {
        pendingAdds.Add((data, source));
    }

    public void ApplyEffectImmediate(StatusEffectData data, GameObject source)
    {

        if (source.GetComponent<IEventHandler>() is Weapon)
        {
            source = GameManager.instance.player.gameObject;
        }

        if (!activeEffects.TryGetValue(data, out var list))
        {
            list = new List<StatusEffectInstance>();
            activeEffects[data] = list;
        }


        StatusEffectInstance instance =
    list.Find(e => e.data == data && e.context.source == source);

        if (instance == null)
        {
            instance = new StatusEffectInstance(data, source, this.gameObject);

            instance.OnApplied += HandleInstanceApplied;
            instance.OnExpired += HandleInstanceExpired;

            list.Add(instance);
            instance.OnApply();

            foreach (var e in instance.subscribedEvents)
            {
                if (!eventIndex.TryGetValue(e, out var effectList))
                {
                    effectList = new List<StatusEffectInstance>();
                    eventIndex[e] = effectList;
                }

                effectList.Add(instance);
            }
        }

        else
        {
            instance.AddStack(1); 
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



    private void CleanupExpired()
    {
        var expired = toRemove.ToArray();

        // Phase 1: notify
        foreach (var (data, instance) in expired)
        {
            instance.OnExpire();
        }

        // Phase 2: remove
        foreach (var (data, instance) in expired)
        {
            if (!activeEffects.TryGetValue(data, out var list))
                continue;

            list.Remove(instance);

            if (list.Count == 0)
                activeEffects.Remove(data);
        }
    }

    public void Dispatch(EffectContext ctx)
    {
        if (!eventIndex.TryGetValue(ctx.trigger, out var list))
            return;

        var snapshot = list.ToArray(); 

        for (int i = 0; i < snapshot.Length; i++)
        {
            snapshot[i].HandleEvent(ctx.trigger, ctx);
        }
    }

    private void HandleInstanceApplied(StatusEffectInstance instance)
    {
        StatusEffectRegistry.instance?.AddStacks(instance.data, instance.stacks);
    }

    private void HandleInstanceExpired(StatusEffectInstance instance)
    {
        StatusEffectRegistry.instance?.RemoveStacks(instance.data, instance.stacks);
    }

    public void ResetStatusEffects()
    {
        activeEffects.Clear();
        eventIndex.Clear();
    }
}