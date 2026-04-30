using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Behaviour/Fire Projectile")]
public class FireProjectile : EnemyBehaviour
{
    public override void Move(Player player, Enemy enemy)
    {
        if (player != null)
        {
            float range = enemy.weapon.stats.GetStat(StatType.Range);
            float rangeSqr = range * range;

            Vector3 toPlayer = player.transform.position - enemy.transform.position;

            if (toPlayer.sqrMagnitude <= rangeSqr)
            {
                enemy.weapon.Tick(Time.deltaTime);
            }
            else
            {
                enemy.transform.position += toPlayer.normalized *
                    enemy.stats.GetStat(StatType.MoveSpeed) * Time.deltaTime;
            }

        }
    }
}
