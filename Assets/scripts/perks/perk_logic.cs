using System;
using statusEffects;
using Unity.Mathematics;
using UnityEngine;

public class perk_logic : MonoBehaviour
{

    public static perk_logic current;
    public bool perk1;

    public int seed_bag_customization;

    public GameObject water_ball_prefab;

    public bool perk2;
    public Animator trowel_animator;
    public Animator hoe_animator;
    public Animator scythe_animator;
    public Animator seed_bag_animator;
    public Animator watering_can_animator;
    
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
    
    public void perk2_toggle()
    {
        perk2 = !perk2;
        perk2_logic();
    }

    public void perk2_logic()
    {
        if (perk2)
        {
            if (StatusEffectAdder.current.player.TryGetComponent(out wet_buff wet_logic))
            {
                trowel_animator.speed = math.pow(wet_logic.water_element_multiplier,wet_logic.stack_count);
                hoe_animator.speed = math.pow(wet_logic.water_element_multiplier,wet_logic.stack_count);
                scythe_animator.speed = math.pow(wet_logic.water_element_multiplier,wet_logic.stack_count);
                seed_bag_animator.speed = math.pow(wet_logic.water_element_multiplier,wet_logic.stack_count);
                watering_can_animator.speed = math.pow(wet_logic.water_element_multiplier,wet_logic.stack_count);
            }
            else
            {
                trowel_animator.speed = 1;
                hoe_animator.speed = 1;
                scythe_animator.speed = 1;
                seed_bag_animator.speed = 1;
                watering_can_animator.speed = 1;
            }
        }
        else
        {
            trowel_animator.speed = 1;
            hoe_animator.speed = 1;
            scythe_animator.speed = 1;
            seed_bag_animator.speed = 1;
            watering_can_animator.speed = 1;
        }
    }
}
