using UnityEngine;
[CreateAssetMenu(menuName = "Rarity")]
public class Rarity : ScriptableObject
{
   public RarityEnum rarity;
   public float chance;
   public float valueModifier;
}
