using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectEntryNode
{
    public CombatEvent trigger;
    [SerializeReference]
    public List<EffectNodeData> effectData;


    public void Validate()
    {
        if (effectData == null) return;

        for (int i = 0; i < effectData.Count; i++)
        {
            if (effectData[i] == null)
                effectData[i] = new EffectNodeData { type = EffectNodeType.ApplyStatusEffect };

            effectData[i].Validate();
        }
    }

    internal void Execute(EffectContext ctx)
    {
        foreach (EffectNodeData nodeData in effectData)
        {
            nodeData.effectConfig.Execute(ctx);
        }
    }
}