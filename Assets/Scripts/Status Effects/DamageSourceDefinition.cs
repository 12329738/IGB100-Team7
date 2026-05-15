using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Damage Source")]
public class DamageSourceDefinition : ScriptableObject
{
    public string id;
    public bool ignoreModifiers = false;
    public bool usesValueSource = false;
    public ValueSource source;
    public Color textColour = Color.white;
}