using System.Collections.Generic;
using UnityEngine;

public class Magnet : Pickup
{
    internal override void PickUp()
    {
        ExperienceGem[] gems = FindObjectsByType<ExperienceGem>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (ExperienceGem gem in gems)
        {
            gem.pickedUp = true;
        }
        ObjectPool.instance.ReturnObject(this.gameObject);
    }
}
