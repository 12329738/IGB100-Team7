using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Transformation")]
public class Transformation : ScriptableObject
{
    public string name;
    public string description;
    public List<Upgrade> upgrades;
    //public StatusEffect transformationEffect;
}
