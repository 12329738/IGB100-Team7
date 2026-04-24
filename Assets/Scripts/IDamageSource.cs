using UnityEngine;

public interface IDamageSource 
{
    public Entity owner { get; set; }
    public DamageSourceDefinition definition {  get; set; }
}
