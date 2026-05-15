using System;
using UnityEngine;

public class EffectPayload
{
    public StatusEffectDataInstance status;
    public Weapon weapon;
    public GameObject prefab;


    internal void Reset()
    {
        status = null;
        weapon = null;
        prefab = null;
    }
}