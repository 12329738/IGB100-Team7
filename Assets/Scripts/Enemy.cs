using System;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleKnockbackOrMovement();
    }

    private void HandleKnockbackOrMovement()
    {
        if (knockbackRemaining > 0.1f)
        {
            float step = GameManager.instance.knockBackSpeed * Time.deltaTime;

            step = Mathf.Min(step, knockbackRemaining);

            transform.position += knockbackDirection * step;

            knockbackRemaining -= step;

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
        
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
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

            combat.DealDamage(intent);
        }
        
        
    }

    internal override void Die()
    {
        OnDeathCallback?.Invoke();

        SpawnerManager.instance.SpawnExperienceGem(transform.position, expAmount);
        if (dropsChest)
            SpawnerManager.instance.SpawnChest(transform.position);
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
