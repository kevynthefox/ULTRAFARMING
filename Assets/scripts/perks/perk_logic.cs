using System;
using statusEffects;
using UnityEngine;

public class perk_logic : MonoBehaviour
{

    public static perk_logic current;
    public bool perk1;

    public void Awake()
    {
        current = this;
    }

    public void perk1_toggle()
    {
        perk1 = !perk1;
    }

    public void perk1_logic()
    {
        if (perk1)
        {
            StatusEffectAdder.current.addStatusEffect(this.gameObject, 0);
        }
    }
}
