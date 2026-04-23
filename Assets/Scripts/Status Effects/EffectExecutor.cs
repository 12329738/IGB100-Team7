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
    
        switch (intent.type)
        {
            case EffectIntent.DealDamage:
                intent.target.GetComponent<Combat>().DealDamage(intent);
                break;

            case EffectIntent.Heal:
                intent.target.GetComponent<Combat>().Heal(intent);
                break;

            case EffectIntent.Knockback:
                intent.target.GetComponent<Combat>().KnockBack(intent);
                break;

            case EffectIntent.ApplyStatusEffect:
                intent.target.GetComponent<StatusEffectManager>()
                    .Apply(intent.context.payload.status, intent.source);
                break;
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