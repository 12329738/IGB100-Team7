using static TreeEditor.TreeEditorHelper;

public static class EffectNodeFactory
{
    public static EffectNodeConfig Create(EffectNodeType type)
    {
        return type switch
        {
            EffectNodeType.ApplyStatusEffect => new ApplyStatusConfig(),
            EffectNodeType.DamageOverTime => new DamageOverTimeConfig(),
            _ => new ApplyStatusConfig(),
        };
    }

    public static EffectNodeData CreateDefault()
    {
        return new EffectNodeData
        {
            type = EffectNodeType.ApplyStatusEffect,
            node = new ApplyStatusConfig()
        };
    }
}