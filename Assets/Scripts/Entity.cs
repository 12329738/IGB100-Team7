
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public abstract class Entity : MonoBehaviour, IDamageable
{
    public float currentHealth;
    [HideInInspector] public bool canBeDamaged = true;
    [SerializeField] public FlashWhite flashScript;

    internal Vector3 knockbackDirection;
    internal float knockbackRemaining;

    
    public Combat combat {  get; private set; }
    private StatusEffectManager status;

    [SerializeField]
    private Stats _stats;
    public virtual Stats stats { get => _stats; set => _stats = value; }
    [SerializeField]
    private Team _team;
    public virtual Team team { get => _team; set => _team = value; }
    [SerializeField]
    private float _hitCooldown;
    public virtual float hitCooldown { get => _hitCooldown; set => hitCooldown = value; }

    public float lastHitTime { get; set; }

    public void Awake()
    {
        SetupStats();
    }

    private void SetupStats()
    {
        stats.Initialize();
        currentHealth = stats.GetStat(StatType.MaxHealth).currentValue;
        combat = GetComponent<Combat>();
        status = GetComponent<StatusEffectManager>();
    }

    public bool IsDamageable()
    {
        if (canBeDamaged == false)
        {
            return false;
        }
        else
            return true;
    }

    public virtual void TakeDamage(EffectContext context)
    {
        //if (Time.time - lastHitTime < hitCooldown)
        //    return;

        //lastHitTime = Time.time;

        currentHealth -= context.damage;
        DamagePopup.instance.ShowCombatText(context);
        if (flashScript != null)
        {
            flashScript.TriggerFlash();
        }

        if (currentHealth <= 0)
        {
            canBeDamaged = false;
            Die();
        }
    }

    internal abstract void Die();


    public void AddStatModifier(float amount, StatType statType)
    {
        StatModifier modifier = new StatModifier();
        modifier.stat = statType;
        modifier.amount = amount;
        stats.modifiers.Add(modifier);
    }

    public float GetCurrentHealthPercent()
    {
        return currentHealth / stats.GetStat(StatType.MaxHealth).currentValue;
    }

    public float[] GetCurrentHealth()
    {
        float[] healthAmount = { currentHealth, stats.GetStat(StatType.MaxHealth).currentValue };

        return healthAmount;
    }

    public void KnockBack(float magnitude, Vector3 attackerPosition)
    {
        knockbackDirection = (transform.position - attackerPosition).normalized;
        knockbackRemaining = magnitude;
    }
}
