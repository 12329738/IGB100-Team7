using System;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class Enemy : Entity, IDamageable
{
    public float expAmount;
    Player player;


    void Start()
    {
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (knockbackRemaining > 0)
        {
            float step = GameManager.instance.knockBackSpeed * Time.deltaTime;

            if (step > knockbackRemaining)
                step = knockbackRemaining;

            transform.position += knockbackDirection * step;
            knockbackRemaining -= step;
        }

        else
        {
            Move();
        }
            
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
        if (other is BoxCollider)
        {
            Player player = other.GetComponentInParent<Player>();
            if (player != null)
            {
                var context = new EffectContext
                {
                    source = gameObject,
                    target = other.gameObject,
                    value = stats.GetStat(StatType.Damage),
                    effectInstanceId = this,
                    hitInterval = 1f,
                };
                context.sourceInstanceId = context.source.GetInstanceID();
                combat.DealDamage(context);
            }
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
        status.ResetStatusEffects();
    }

    void OnEnable()
    {
        SpawnerManager.instance.RegisterEnemy();
    }

    void OnDisable()
    {
        SpawnerManager.instance.UnregisterEnemy();
        ObjectPool.instance.ReturnObject(gameObject);
    }
}
