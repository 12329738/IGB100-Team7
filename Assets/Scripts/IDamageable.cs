using UnityEngine;

public interface IDamageable 
{
    void TakeDamage(CombatIntent intent);
    bool IsDamageable();
    Team team { get; }

    Combat combat { get; }
    float currentHealth { get; }
    float GetCurrentHealthPercent();
    float[] GetCurrentHealth();
    public void KnockBack(float magnitude, Vector3 attackerPosition);
    public void Heal(float amount);

}
