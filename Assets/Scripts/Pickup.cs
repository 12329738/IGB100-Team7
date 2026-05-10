using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class Pickup : MonoBehaviour
{
    internal bool pickedUp;
    public float pickupSpeed = 10f;
    private void OnTriggerEnter(Collider other)
    {
        pickedUp = true;
    }

    internal abstract void PickUp();

    void Update()
    {
        if (pickedUp)
        {
            Vector3 dir = GameManager.instance.player.transform.position - transform.position;
            float step = pickupSpeed * Time.deltaTime;

            if (dir.magnitude <= step)
            {
                PickUp();
                pickedUp = false;
                return;
            }
                

            transform.position += pickupSpeed * Time.deltaTime * dir.normalized;

        }
    }


}
