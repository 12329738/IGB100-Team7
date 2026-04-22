
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public abstract class Entity : MonoBehaviour, IEventHandler, IModifierProvider, IModifierReceiver
{
    [HideInInspector] public bool canBeDamaged = true;
    [SerializeField] public FlashWhite flashScript;


    internal Vector3 knockbackDirection;
    internal float knockbackRemaining;
    private EffectHandler effectHandler;
    [HideInInspector]
    public EventHandler eventHandler {  get; set; }

    [HideInInspector]
    public Combat combat {  get; private set; }
    internal StatusEffectManager status;
    [HideInInspector]
    public List<EffectEntryNode> effects;
    [SerializeField]
    private StatsPreset _statPreset;

    public virtual StatsPreset statPreset { get => _statPreset; set => _statPreset = value; }

    private Stats _stats;
    [HideInInspector]
    public Stats stats { get => _stats; set => _stats = value; }

    [SerializeField]
    private Team _team;
    public virtual Team team { get => _team; set => _team = value; }
    [HideInInspector]
    public float currentHealth { get; set; } = 1;
    [HideInInspector]
    public readonly ModifierProvider provider = new ModifierProvider();

    public void AddModifier(StatModifier mod)
        => provider.AddModifier(mod);

    public void RemoveModifier(StatModifier mod)
        => provider.RemoveModifier(mod);
    [HideInInspector]
    public List<StatModifier> Modifiers => provider.Modifiers;

    [SerializeField]

    public void Awake()
    {
        status = GetComponent<StatusEffectManager>();
        eventHandler = new EventHandler();
        status.Initialize(eventHandler);
        OnSpawned();
    }

    public event Action OnDirty
    {
        add => provider.OnDirty += value;
        remove => provider.OnDirty -= value;
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

    public void OnSpawned()
    {

        stats = new Stats();
        stats.Initialize(statPreset);
        stats.AddModifierProvider(this.provider);
        currentHealth = stats.GetStat(StatType.MaxHealth);
        canBeDamaged = true;
        knockbackRemaining = 0;
        combat = GetComponent<Combat>();
        combat.Initialize(eventHandler);

        foreach (EffectEntryNode node in effects)
        {
            effectHandler.AddToMap(node);
        }
    }
    public virtual void TakeDamage(EffectContext context)
    {

        currentHealth -= context.value;
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



    public float GetCurrentHealthPercent()
    {
        return currentHealth / stats.GetStat(StatType.MaxHealth);
    }

    public float[] GetCurrentHealth()
    {
        float[] healthAmount = { currentHealth, stats.GetStat(StatType.MaxHealth) };

        return healthAmount;
    }

    public void KnockBack(float magnitude, Vector3 attackerPosition)
    {
        knockbackDirection = (transform.position - attackerPosition).normalized;
        knockbackRemaining = magnitude;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > stats.GetStat(StatType.MaxHealth))
        {
            currentHealth = stats.GetStat(StatType.MaxHealth);
        }

    }

}
