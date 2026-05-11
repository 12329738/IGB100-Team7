using System;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;
using Random = System.Random;

public class Enemy : Entity, IDamageable
{
    public float expAmount;
    Player player => GameManager.instance.player;
    public Weapon weaponData;
    [HideInInspector]
    public Weapon weapon;
    public EnemyBehaviour behaviour;
    public Action OnDeathCallback;
    public bool dropsChest;
    public Image healthBar;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleKnockbackOrMovement();
        UpdateHealthBar();
    }

   

    private void HandleKnockbackOrMovement()
    {
        if (knockbackRemaining > 0.1f)
        {
            gameObject.layer = LayerMask.NameToLayer("Knockback");
            float step = GameManager.instance.knockBackSpeed * Time.deltaTime;

            step = Mathf.Min(step, knockbackRemaining);

            transform.position += knockbackDirection * step;

            knockbackRemaining -= step;
            if (knockbackRemaining < 0.01f)
            {
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                knockBack = null;
            }
                

            return; 
        }


        Move();
    }

    void Move()
    {
        if (behaviour != null)
        {
            behaviour.Move(player, this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var context = new EffectContext
        {
            damageSource = this,
            target = other.GetComponent<IDamageSource>(),

            value = stats.GetStat(StatType.Damage),
        };

        context.damageSource.definition = definition;
        var intent = new CombatIntent
        {
            value = stats.GetStat(StatType.Damage),
            source = this,
            target = other.GetComponent<IDamageSource>(),
            context = context
        };

        if (other.TryGetComponent<Player>(out Player player))
        {
            

            combat.DealDamage(intent);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy) && knockbackRemaining > 0.1f && knockBack.knockBackDamage > 0)
        {
            var context = new EffectContext
            {
                damageSource = this,
                target = other.GetComponent<IDamageSource>(),

                value = knockBack.knockBackDamage
            };

            context.damageSource.definition = definition;
            var intent = new CombatIntent
            {
                value = knockBack.knockBackDamage,
                source = this,
                target = other.GetComponent<IDamageSource>(),
                context = context
            };
            context.value = knockBack.knockBackDamage;
            combat.DealDamage(intent);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.fillAmount = GetCurrentHealthPercent();
    }

    internal override void Die()
    {
        OnDeathCallback?.Invoke();

        SpawnerManager.instance.SpawnExperienceGem(transform.position, expAmount);
        if (dropsChest)
            SpawnerManager.instance.SpawnChest(transform.position);
        float random = UnityEngine.Random.Range(0, 100);
        if (random <= GameManager.instance.healthPickupDropRate)
            SpawnerManager.instance.SpawnHealthPickup(transform.position);

        random = UnityEngine.Random.Range(0, 100);
        if (random <= GameManager.instance.magnetPickupDropRate)
            SpawnerManager.instance.SpawnMagnetPickup(transform.position);

        SpawnerManager.instance.UnregisterEnemy();

        weapon = null;

        if (status != null)
            status.ResetStatusEffects();

        GameManager.instance.effectHandler.UnRegister(this);
        flashScript.ResetMaterial();

        ObjectPool.instance.ReturnObject(gameObject);
    }



    void OnEnable()
    {
        if (weaponData != null)
        {
            weapon = Instantiate(weaponData);
            weapon.Initialize(this);
        }
        
        SpawnerManager.instance.RegisterEnemy();
    }

}
