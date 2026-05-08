using System;
using UnityEngine;
[Serializable]
[CreateAssetMenu(menuName = "Weapon Evolution")]
public class WeaponEvolution : ScriptableObject
{
    public WeaponBehaviour newBehaviour;
    public WeaponBehaviour addedBehaviour;
    public EffectEntryNode newEffect;
    public EffectEntryNode improvedEffect;
    public ProjectilePattern newPattern;
    public StatModifier modifiers;
    private void OnValidate()
    {
        if (newEffect == null) return;
        newEffect.Validate();
        if (improvedEffect == null) return;
        improvedEffect.Validate();

    }
}
