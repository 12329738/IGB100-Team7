using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class Chest : Pickup
{
    public int minUpgrades;
    public int maxUpgrades;
    internal override void PickUp()
    {
        StartCoroutine(OpenChest());
        

    }

    private IEnumerator OpenChest()
    {
        int random = Random.Range(minUpgrades, maxUpgrades);

        yield return StartCoroutine(GameManager.instance.player.ShowUpgrades(random));
        ObjectPool.instance.ReturnObject(this.gameObject);
    }
}
