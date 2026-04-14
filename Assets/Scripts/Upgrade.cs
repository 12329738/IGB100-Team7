using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : ScriptableObject
{

    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public ItemList itemType { get; set; }

}







