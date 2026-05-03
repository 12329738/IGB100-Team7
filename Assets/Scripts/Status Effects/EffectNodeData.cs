using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EffectNodeData
{
    public EffectIntent type;
    [SerializeReference]
    public EffectOperation effectOperation;
    [System.NonSerialized]
    public EffectOperation runtimeNodeConfig;

    public EffectNodeData(EffectNodeData effect)
    {
        type = effect.type;
        effectOperation = effect.effectOperation;
    }

    public EffectNodeData(EffectIntent intent)
    {
        type = intent;
    }

    public void Validate()
    {
        var expectedType = EffectNodeDatabase.Get(type);

        if (effectOperation == null || effectOperation.GetType() != expectedType)
        {
            effectOperation = EffectNodeDatabase.Create(type);
        }
    }


    public void Execute(EffectContext ctx, List<CombatIntent> intents)
    {
        effectOperation?.Generate(ctx, intents);
    }

    private EffectOperation GetExpectedType(EffectIntent type)
    {
        return EffectNodeDatabase.Create(type);
    }

    internal void Modify(EffectContext ctx, ref List<CombatIntent> intents)
    {
        IIntentModifier modifier = effectOperation as IIntentModifier;
        for (int i = 0; i < intents.Count; i++)
        {
            if (intents[i].intent == modifier.effectToModifiy)
            {
                var temp = intents[i];
                modifier.Modify(ref temp);
                intents[i] = temp;
            }
        }
    }
}