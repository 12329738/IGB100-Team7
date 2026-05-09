using UnityEngine;

public class HealthPickup : Pickup
{
    public float healAmount;

    internal override void PickUp()
    {
        CombatIntent intent = new CombatIntent
        {
            intent = EffectIntent.Heal,
            target = GameManager.instance.player,
            value = healAmount
        };

        GameManager.instance.effectExecutor.Execute(intent);    
        ObjectPool.instance.ReturnObject(this.gameObject);
    }
}
