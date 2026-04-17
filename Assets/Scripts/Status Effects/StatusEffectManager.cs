using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class StatusEffectManager : MonoBehaviour
{

    private Dictionary<StatusEffectData, List<StatusEffectInstance>> activeEffects = new();

    public void ApplyEffect(StatusEffectData data, GameObject source)
    {
        if (!activeEffects.TryGetValue(data, out var list))
        {
            list = new List<StatusEffectInstance>();
            activeEffects[data] = list;
        }


        if (data.maxStacks > 1)
        {
            if (list.Count >= data.maxStacks)
            {
                list[0].Remove();
                list.RemoveAt(0);
            }
        }

        var instance = new StatusEffectInstance(data, source, gameObject);
        instance.OnApply();

        list.Add(instance);
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
        CombatEventBus.OnEvent += HandleEvent;
    }

    private void OnDisable()
    {
        CombatEventBus.OnEvent -= HandleEvent;
    }

    private void HandleEvent(CombatEvent type, EffectContext ctx)
    {
        foreach (var effects in activeEffects.Values)
        {
            foreach (var instance in effects)
            {
                instance.HandleEvent(type, ctx);
            }
        }
    }
}