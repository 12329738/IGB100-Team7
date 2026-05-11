using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Rarity")]
public class Rarity : ScriptableObject
{
   public RarityEnum rarity;
   public float chance;
   public float valueModifier;
   public Color color;
   public Sprite upgradeImage;
}
