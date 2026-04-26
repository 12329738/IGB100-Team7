using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class ValueSource
{
    public enum Mode
    {
        Constant,
        MissingHealth,
        StackCount,
        Custom,
        SecondsTransformed,
        MaxHealth
    }

    public Mode mode;

    public float constant;
    public float multiplier;
    public bool useTarget;
    public bool useSource;

    public float Evaluate(CombatIntent intent)
    {
        IDamageSource target;
        if (useSource)
            target = intent.source;
        else
            target = intent.target;

        switch (mode)
        {
            case Mode.MaxHealth:
                if (target is IModifierReceiver receiver)
                {
                    return receiver.stats.GetStat(StatType.MaxHealth) / 20;
                }
                
                return 0;

            case Mode.SecondsTransformed:
                return 1 + (GameManager.instance.player.timeTransformed * multiplier /100);

            default:
                return 0;
        }

    }


}
