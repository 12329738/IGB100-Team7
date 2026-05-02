using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.EventSystems.EventTrigger;

public class EffectExecutor : MonoBehaviour
{
    public Combat combat;
    public void Execute(CombatIntent intent)
    {
        if (intent.target is Component comp)
        {

        
            switch (intent.intent)
        {
            
        
            case EffectIntent.DealDamage:
                comp.gameObject.GetComponent<Combat>().DealDamage(intent);
                break;

            case EffectIntent.Heal:
                comp.gameObject.GetComponent<Combat>().Heal(intent);
                break;

            case EffectIntent.Knockback:
                comp.gameObject.GetComponent<Combat>().KnockBack(intent);
                break;

            case EffectIntent.ApplyStatusEffect:
                comp.gameObject.GetComponent<StatusEffectManager>()
                    .Apply(intent.context.payload.status, intent.source);
                break;
            }
        }
            
        
    }

    public void Execute(List<CombatIntent> intents)
    {
        foreach (CombatIntent intent in intents)
        {
            Execute(intent);
        }
    }
}