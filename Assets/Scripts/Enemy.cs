using System;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class Enemy : Entity, IDamageable
{
    public float expAmount;
    Player player => GameManager.instance.player;
    public DamageSourceDefinition damageSourceDefinition;
   

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
        if (player != null)
        {

            Vector3 dir = player.transform.position - transform.position;
            transform.position += dir.normalized * stats.GetStat(StatType.MoveSpeed) * Time.deltaTime;
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
                damageSourceOwner = this,
                definition = damageSourceDefinition,
                value = stats.GetStat(StatType.Damage),
                sourceInstanceId = this.gameObject.GetInstanceID(),
                hitInterval = 1f,
            };

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
        SpawnerManager.instance.SpawnExperienceGem(transform.position, expAmount);
        OnDespawn();
        this.gameObject.SetActive(false);
    }

    private void OnDespawn()
    {
        if (status != null)
            status.ResetStatusEffects();
    }

    void OnEnable()
    {

        SpawnerManager.instance.RegisterEnemy();
    }

    void OnDisable()
    {
        SpawnerManager.instance.UnregisterEnemy();
        GameManager.instance.effectHandler.UnRegister(this);
        ObjectPool.instance.ReturnObject(gameObject);
    }
}
