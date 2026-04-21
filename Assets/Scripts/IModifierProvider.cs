using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IModifierProvider
{
    public List<StatModifier> Modifiers { get; }

    public event Action OnDirty;
}


