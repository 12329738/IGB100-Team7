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

        for (int i = 0; i < random; i++)
            GameManager.instance.player.upgradeQueue.Enqueue(1);

        if (GameManager.instance.player.upgradeCoroutine == null)
        {        
            yield return StartCoroutine(GameManager.instance.player.ShowUpgrades());
        }

        else
            yield return GameManager.instance.player.upgradeCoroutine;

        ObjectPool.instance.ReturnObject(this.gameObject);
    }
}
