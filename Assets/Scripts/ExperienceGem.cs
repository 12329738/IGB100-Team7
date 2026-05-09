using UnityEngine;

public class ExperienceGem : Pickup
{
    public float experienceValue;
    internal override void PickUp()
    {
        GameManager.instance.player.AddExperience(experienceValue);
        ObjectPool.instance.ReturnObject(this.gameObject);
    }
}
