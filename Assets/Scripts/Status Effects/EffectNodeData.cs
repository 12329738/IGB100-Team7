using UnityEditor;
using UnityEngine;


[System.Serializable]
public class EffectNodeData
{
    public EffectNodeType type;

    [SerializeReference]
    public EffectNodeConfig node;
    [System.NonSerialized]
    public EffectNodeConfig runtimeNode;

    public void Validate()
    {
        var expected = GetExpectedType(type);

        if (node == null || node.GetType() != expected)
        {
            node = EffectNodeFactory.Create(type);
        }
    }


    public void Execute(EffectContext ctx)
    {
        node?.Execute(ctx);
    }

    private System.Type GetExpectedType(EffectNodeType type)
    {
        return type switch
        {
            EffectNodeType.ApplyStatusEffect => typeof(ApplyStatusConfig),
            EffectNodeType.DamageOverTime => typeof(DamageOverTimeConfig),
            _ => typeof(EffectNodeConfig)
        };
    }
}