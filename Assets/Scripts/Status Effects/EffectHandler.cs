using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    private readonly List<EffectInstance> effects = new();
    public EffectExecutor executor;
    //private EffectPipeline pipeline;
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

            effects[i].definition.Execute(ctx, intents);
            
        }

        GameManager.instance.effectExecutor.Execute(intents);
    }


    public void Register(EffectInstance instance)
    {
        effects.Add(instance);
    }

    private void Register(StatusEffectInstance instance)
    {
        foreach (var runtime in instance.runtimes)
        {
            Register(runtime);
        }
    }


}