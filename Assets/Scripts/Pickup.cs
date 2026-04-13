using System;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PickUp(other);
    }

    internal abstract void PickUp(Collider other);

}
