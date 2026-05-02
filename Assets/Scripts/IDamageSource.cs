using System;
using UnityEditor;
using UnityEngine;

public interface IDamageSource 
{
    [HideInInspector]
    public Entity owner { get; set; }
    public DamageSourceDefinition definition {  get; set; }
    public float hitInterval { get; set; }
    public Guid guid => new();
}
