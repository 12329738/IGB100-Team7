using System;
using UnityEngine;

public class Enemy : Entity
{
    public float contactDamage;
    Player player;
    //[SerializeField] private Team _team;
    //public override Team team => _team;
    //[SerializeField] private Stats _stats;
    //public override Stats stats => _stats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (player != null)
        {

            Vector3 dir = player.transform.position - transform.position;
            transform.position += dir.normalized * stats.GetStat(StatType.MoveSpeed).currentValue * Time.deltaTime;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            player.TakeDamage(contactDamage);
        }
    }

    internal override void Die()
    {
        SpawnerManager.instance.SpawnExperienceGem(transform.position);
        Destroy(gameObject);
    }
}
