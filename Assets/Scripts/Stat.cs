
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
[Serializable]
public class Stat 
{
    public float baseValue;
    [HideInInspector]
    public float cachedValue;
    [HideInInspector]
    public bool isDirty;

    private readonly List<StatModifier> modifiers = new List<StatModifier>();

  
}
