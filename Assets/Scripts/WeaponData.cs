using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public string weaponDescription;
    public GameObject prefab;
    public ItemList itemType;

    //public float cooldown = 1;
    //public float damage = 1;
    //public float lifetime = 1;
    //public float speed = 1;
    //public float projectiles = 1;
    public Stats stats;
    [UnityEngine.Range (0f, 360f)]
    public float angleVariance;
    [UnityEngine.Range(0f, 360f) ]
    public float direction;
    public bool isPiercing;
    public WeaponBehaviour behaviour;
    public List<Upgrade> upgrades;
    public List<List<Upgrade>> upgradeList;

}