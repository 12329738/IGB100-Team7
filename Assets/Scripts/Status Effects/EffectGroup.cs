using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect Group")]
[System.Serializable]
public class EffectGroup : ScriptableObject
{


    public List<EffectEntryNode> effects;
}