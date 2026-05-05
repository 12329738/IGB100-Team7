using System;
using System.Collections;
using UnityEngine;

public class Chest : Pickup
{
    public int upgradeAmount = 3;
    internal override void PickUp(Collider other)
    {
        StartCoroutine(OpenChest());
        

    }

    private IEnumerator OpenChest()
    {
        yield return StartCoroutine(GameManager.instance.player.ShowUpgrades(upgradeAmount));
        Destroy(gameObject);
    }
}
