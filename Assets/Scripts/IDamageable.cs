using UnityEngine;

public interface IDamageable : IDamageSource
{
    void TakeDamage(CombatIntent intent);
    bool IsDamageable();
    Team team { get; }

    Combat combat { get; }
    float currentHealth { get; }
    float GetCurrentHealthPercent();
    float[] GetCurrentHealth();
    public void KnockBack(CombatIntent intent);
    public void Heal(float amount);

}
