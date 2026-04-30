using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
[CreateAssetMenu(menuName = "Enemy Behaviour/Move Towards Player")]
public class MoveTowardPlayer : EnemyBehaviour
{
    public override void Move(Player player, Enemy enemy)
    {
        if (player != null)
        {

            Vector3 dir = player.transform.position - enemy.transform.position;
            enemy.transform.position += dir.normalized * enemy.stats.GetStat(StatType.MoveSpeed) * Time.deltaTime;
        }
    }
}
