using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class StatusEffectManager : MonoBehaviour
{

    private Dictionary<StatusEffectData, List<StatusEffectInstance>> activeEffects = new();
    Dictionary<CombatEvent, List<StatusEffectInstance>> eventMap = new();
    Combat combat;

    public void ApplyEffect(StatusEffectData data, GameObject source)
    {
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
            total++; // if you support per-stack strength

        return total;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        var keysToRemove = new List<StatusEffectData>();

        foreach (var kvp in activeEffects)
        {
            var effect = kvp.Value;

            foreach (StatusEffectInstance instance in effect)
            {
                instance.Tick(dt);

                if (instance.IsExpired())
                {
                    instance.Remove();
                    Debug.Log($"Status effect {instance} expired on {gameObject.name}");
                    keysToRemove.Add(kvp.Key);
                }
            }

           
        }

        for (int i = 0; i < keysToRemove.Count; i++)
        {
            activeEffects.Remove(keysToRemove[i]);
        }
    }

    private void OnEnable()
    {
        combat = GetComponent<Combat>();
        combat.OnEvent += HandleEvent;
    }

    private void OnDisable()
    {
        combat.OnEvent -= HandleEvent;
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