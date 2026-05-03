using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    private readonly List<EffectInstance> effects = new();
    public EffectExecutor executor;

    void Awake()
    {
        executor = GameManager.instance.effectExecutor;
    }
    private void Update()
    {
        float now = Time.time;

        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].Tick(now, executor);
        }
    }

    public void Dispatch(EffectContext ctx)
    {
        List<CombatIntent> intents = new();

        for (int i = 0; i < effects.Count; i++)
        {
            if (ctx.trigger == CombatEvent.OnDamageTaken && (object)ctx.target == GameManager.instance.player)
            {}
            else if (ctx.damageSource != effects[i].effectHolder && ctx.damageSource.owner != effects[i].effectHolder)
                continue;
            effects[i].entryNode.Execute(ctx, intents);

        }

        GameManager.instance.effectExecutor.Execute(intents);
    }

    public void Modify(EffectContext ctx, ref CombatIntent combatIntent)
    {
        List<CombatIntent> intents = new();
        intents.Add((CombatIntent)combatIntent);


        for (int i = 0; i < effects.Count; i++)
        {
            if (ctx.damageSource.owner == effects[i].effectHolder)
                effects[i].entryNode.Modify(ctx, ref intents);

        }
        combatIntent = intents[0];
    }

    public void Register(EffectInstance instance)
    {
        effects.Add(instance);

        List<CombatIntent> intents = new List<CombatIntent>();
        EffectContext context = new EffectContext { trigger = CombatEvent.OnApply };
        instance.entryNode.Execute(context, intents);
        instance.entryNode.Modify(context, ref intents);

    }

    private void Register(StatusEffectInstance instance)
    {
        foreach (var runtime in instance.runtimes)
        {
            Register(runtime);
        }
    }

    public void UnRegister(IDamageSource owner)
    {
        effects.RemoveAll(x => x.effectHolder == owner);
    }

    public void UnRegister(EffectInstance instance)
    {
        effects.Remove(instance);
    }



}