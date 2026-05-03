public enum CombatEvent
{
    OnHit,
    OnKill,
    OnCrit,
    OnDamageTaken,
    OnHeal,
    OnTick,
    OnApply,
    OnContact,
    OnExpire,
    OnDamage,
    OnDeath,
    OnSpawn,
    OnStackCount,
    IsTransformed
}

public enum ComparisonType
{
    GreaterThan,
    LessThan,
    EqualTo,
    GreaterOrEqual,
    LessOrEqual
}

[System.Serializable]
public class CombatCondition
{
    public CombatEvent triggerEvent;

    public bool useComparison;

    public ComparisonType comparisonType;
    public float value;

    public CombatCondition(CombatCondition condition)
    {
        triggerEvent = condition.triggerEvent;
        useComparison = condition.useComparison;
        comparisonType = condition.comparisonType;
        value = condition.value;
    }
}