using System;
using statusEffects;
using Unity.Mathematics;
using UnityEngine;

public class perk_logic : MonoBehaviour
{
    
    public int hoe_customization;
    public int scythe_customization;
    public int seed_bag_customization;
    public int watering_can_customization;

    public GameObject water_ball_prefab;
    
    public static perk_logic current;
    public bool perk1; //rushing river

    public bool perk2; //slippery
    public Animator trowel_animator;
    public Animator hoe_animator;
    public Animator scythe_animator;
    public Animator seed_bag_animator;
    public Animator watering_can_animator;
    
    public bool perk3; //slidey
    
    public bool perk4;//dehydration
    public bool perk5;//overhydration

    public bool perk6; //current hands

    
    public void hoe_customization_choser(int i)
    {
        hoe_customization = i;
    }
    public void scythe_customization_choser(int i)
    {
        scythe_customization = i;
    }
    public void seed_bag_customization_choser(int i)
    {
        seed_bag_customization = i;
    }
    public void watering_can_customization_choser(int i)
    {
        watering_can_customization = i;
        watering_can_customization_logic();
    }
    
    
    public void Awake()
    {
        current = this;
    }
    public void perk1_toggle()
    {
        perk1 = !perk1;
    }
    public void perk2_toggle()
    {
        perk2 = !perk2;
        perk2_logic();
    }
    public void perk3_toggle()
    {
        perk3 = !perk3;
    }
    public void perk4_toggle()
    {
        perk4 = !perk4;
    }
    public void perk5_toggle()
    {
        perk5 = !perk5;
    }
    public void perk6_toggle()
    {
        perk6 = !perk6;
    }

    public void watering_can_customization_logic()
    {
        if (watering_can_customization == 1)
        {
            if (watering_can_animator.TryGetComponent(out watering_can can_logic))
            {
                if (StatusEffectAdder.current.player.TryGetComponent(out wet_buff wet_logic))
                {
                    can_logic.water_collider.size = new Vector3(50 * Mathf.Pow(wet_logic.water_element_multiplier,wet_logic.stack_count), 3, 40);
                    can_logic.water_drain_amount = 0.1f *  Mathf.Pow(wet_logic.water_element_multiplier,wet_logic.stack_count);
                }
                else
                {
                    can_logic.water_collider.size = new Vector3(100, 3, 40);
                    can_logic.water_drain_amount = 0.2f;
                }
            }
        }
        else
        {
            if (watering_can_animator.TryGetComponent(out watering_can can_logic))
            {
                can_logic.water_collider.size = new Vector3(50, 3, 40);
                can_logic.water_drain_amount = 0.1f;
            }   
        }
    }
    
    public void perk1_logic()
    {
        if (perk1)
        {
            StatusEffectAdder.current.addStatusEffect(this.gameObject, 0);
        }
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
