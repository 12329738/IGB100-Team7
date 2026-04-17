using UnityEngine;

public interface IDamageable 
{
    void TakeDamage(float damage);
    bool IsDamageable();
    Team team { get; }
    float lastHitTime { get; }
    float hitCooldown {  get; }

    float GetCurrentHealthPercent();
    float[] GetCurrentHealth();
    public void KnockBack(float magnitude, Vector3 attackerPosition);

}
