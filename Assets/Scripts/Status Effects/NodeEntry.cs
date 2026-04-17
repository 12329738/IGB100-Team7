using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeEntry
{
    public CombatEvent trigger;
    [SerializeReference]
    public List<EffectNodeData> nodes;

    public void Validate()
    {
        if (nodes == null) return;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] == null)
                nodes[i] = EffectNodeFactory.CreateDefault();

            nodes[i].Validate();
        }
    }

    internal void Execute(EffectContext ctx)
    {
        foreach (EffectNodeData nodeData in nodes)
        {
            nodeData.node.Execute(ctx);
        }
    }
}