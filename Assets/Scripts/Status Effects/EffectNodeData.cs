using UnityEditor;
using UnityEngine;


[System.Serializable]
public class EffectNodeData
{
    public EffectNodeType type;

    [SerializeReference]
    public EffectNodeConfig effectConfig;
    [System.NonSerialized]
    public EffectNodeConfig runtimeNodeConfig;

    public void Validate()
    {
        var expectedType = EffectNodeDatabase.Get(type);

        if (effectConfig == null || effectConfig.GetType() != expectedType)
        {
            effectConfig = EffectNodeDatabase.Create(type);
        }
    }


    public void Execute(EffectContext ctx)
    {
        effectConfig?.Execute(ctx);
    }

    private EffectNodeConfig GetExpectedType(EffectNodeType type)
    {
        return EffectNodeDatabase.Create(type);
    }
}