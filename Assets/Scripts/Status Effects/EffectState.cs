using System.Collections.Generic;
using UnityEngine;

public class EffectState
{
    public GameObject source;
    public GameObject target;

    public float startTime;
    public float lastTickTime;

    public int stacks;

    public Dictionary<string, float> floats = new();
    public Dictionary<string, int> ints = new();
    public Dictionary<string, object> objects = new();

    public bool IsExpired(float duration)
        => Time.time > startTime + duration;
}