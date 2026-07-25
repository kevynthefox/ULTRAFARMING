using System;
using statusEffects;
using UnityEngine;

public class perk_logic : MonoBehaviour
{

    public static perk_logic current;
    public bool perk1;
    public int seed_bag_customization;

    public GameObject water_ball_prefab;
    
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

    public void seed_bag_customization_choser(int i)
    {
        seed_bag_customization = i;
    }
}
