using UnityEngine;

public class ExperienceGem : Pickup
{
    public float experienceValue;
    internal override void PickUp(Collider other)
    {
        if (other.GetComponent<Player>() == GameManager.instance.player)
        {
            GameManager.instance.player.AddExperience(experienceValue);
            Destroy(gameObject);
        }
    }
}
