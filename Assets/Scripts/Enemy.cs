using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : Entity, IDamageable
{
    public float contactDamage;
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
                    damage = stats.GetStat(StatType.Damage),
                    hitInterval = 1f,
                    damageId = this
                };

                combat.DealDamage(context);
            }
        }
        
    }

    internal override void Die()
    {
        SpawnerManager.instance.SpawnExperienceGem(transform.position, expAmount);
        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        SpawnerManager.instance.RegisterEnemy();
    }

    void OnDisable()
    {
        SpawnerManager.instance.UnregisterEnemy();
        ObjectPool.instance.ReturnObject(this.gameObject, gameObject);
    }
}
