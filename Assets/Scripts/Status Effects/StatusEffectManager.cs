
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StatusEffectManager : MonoBehaviour
{
    private readonly List<StatusEffectInstance> effects = new();
    [HideInInspector]
    public EffectHandler effectHandler;
    public void Apply(StatusEffectDataInstance data, IDamageSource source)
    {

        var existing = effects.FirstOrDefault(x => x.data.definition == data.definition); 

        
        if (existing != null)
        {
            existing.AddStack(1);
            return;
        }

        var instance = new StatusEffectInstance(data, source, gameObject.GetComponent<IDamageSource>(), this);

        effects.Add(instance);

        if (gameObject.TryGetComponent<Entity>(out Entity e))
            e.provider.AddChild(instance.provider);

        instance.OnApply(this);
        DamagePopup.instance.ShowStatusEffect(instance);
        Debug.Log($"{source} applied {instance.data.name} to {gameObject}");

    }

    public void Dispatch(EffectContext context)
    {
        var snapshot = effects.ToArray();

        foreach (StatusEffectInstance instance in snapshot)
        {

            instance.EmitEffects(context);
        }
    }

    internal void RemoveStatus(StatusEffectInstance instance)
    {
        GameManager.instance.statusEffectRegistry.RemoveStacks(instance.data.definition, (int)instance.context.stacks);
        effects.Remove(instance);
        if (gameObject.TryGetComponent<Entity>(out Entity e))
            e.provider.RemoveChild(instance.provider);
    }

    internal void RemoveStatus(StatusEffectData statusEffectData)
    {
        StatusEffectInstance instance = effects.FirstOrDefault(i => i.data.definition == statusEffectData.definition);
        GameManager.instance.statusEffectRegistry.RemoveStacks(instance.data.definition, (int)instance.context.stacks);
        effects.Remove(instance);
        if (gameObject.TryGetComponent<Entity>(out Entity e))
            e.provider.RemoveChild(instance.provider);
    }

    internal void ResetStatusEffects()
    {
        StatusEffectInstance[] snapshot = effects.ToArray();
        foreach (StatusEffectInstance instance in snapshot)
        {
            RemoveStatus(instance);
        }
        effects.Clear();
    }

    private void Update()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].Tick();
        }
    }
}
