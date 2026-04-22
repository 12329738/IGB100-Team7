using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Transformation")]
public class Transformation : ScriptableObject
{
    public string name;
    public string description;
    public List<TransformationUpgrade> upgrades;
    public StatusEffectData effect;
    public Sprite transformationSprite;
    
    public void ApplyTransformation()
    {
        GameManager.instance.player.Transform();
    }
}
