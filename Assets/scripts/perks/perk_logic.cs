using System;
using System.Collections.Generic;
using statusEffects;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class perk_logic : MonoBehaviour
{
    
    public int hoe_customization;
    public int scythe_customization;
    public int seed_bag_customization;
    public int watering_can_customization;

    public GameObject water_ball_prefab;

    public Image hoe_slot;
    public Image scythe_slot;
    public Image seed_bag_slot;
    public Image watering_can_slot;
    
    public Image perk_slot_1;
    public Image perk_slot_2;
    public Image perk_slot_3;
    public Image perk_slot_4;

    public List<Sprite> perk_images;
    public List<Sprite> hoe_images;
    public List<Sprite> scythe_images;
    public List<Sprite> seed_bag_images;
    public List<Sprite> watering_can_images;

    public int perk_slot_1_perk;
    public int perk_slot_2_perk;
    public int perk_slot_3_perk;
    public int perk_slot_4_perk;

    public int slot_old_value;
    
    public static perk_logic current;
    public static event Action<perk_logic> OnPerkSlotLogicEvent;
    
    
    public bool perk1; //rushing river. when you run out of watering can water, gain a speed boost and spawn water below your feet.

    public bool perk2; //slippery. increased weapon swing speed by how wet you are.
    public Animator trowel_animator;
    public Animator hoe_animator;
    public Animator scythe_animator;
    public Animator seed_bag_animator;
    public Animator watering_can_animator;
    
    public bool perk3; //slidey. while wet you have 0 friction with the ground.
    
    public bool perk4;//dehydration. grab a crop to dehydrate it and make it more space efficient in your bag, at the cost of growing slower.
    public bool perk5;//overhydration. grab a crop to overhydrate it and make it less space efficient in your bag, but it grows faster.

    public bool perk6; //current hands. allows you to pick up multiple objects at once by giving them boids.

    public bool perk7; //blessing of the plants. for every seed you have, of the same type as the plant you are harvesting, gain a 'chance' to get an extra seed when harvesting. 'chance' works the same way that it does for the shopping

    public bool perk8; //filthy. digging applies dirty.

    public bool perk9; //blessing of the ground. increased jump height per dirty.
    
    public bool perk10; //green thumb. increased yield for manually planted crops(non thrown)

    public bool perk11; //pet rock. n amount of your stock will be bought at minimum every time. n scaling with dirty.
    public GameObject pet_rock;

    public bool perk12; //unkempt charm. add a bonus to sell price based on how dirty you are.
    
    public void hoe_customization_choser(int i)
    {
        hoe_customization = i;
        hoe_slot.sprite = hoe_images[i];
        hoe_customization_logic();
    }
    public void scythe_customization_choser(int i)
    {
        scythe_customization = i;
        scythe_slot.sprite = scythe_images[i];
    }
    public void seed_bag_customization_choser(int i)
    {
        seed_bag_customization = i;
        seed_bag_slot.sprite = seed_bag_images[i];
    }
    public void watering_can_customization_choser(int i)
    {
        watering_can_customization = i;
        watering_can_slot.sprite = watering_can_images[i];
        watering_can_customization_logic();
    }

    public void perk_slot_logic(int new_value, int slot_value)//slot value like 1 is slot 1
    {
        slot_old_value = 0;
        
        if (slot_value == 1)  slot_old_value = perk_slot_1_perk;
        if (slot_value == 2)  slot_old_value = perk_slot_2_perk;
        if (slot_value == 3)  slot_old_value = perk_slot_3_perk;
        if (slot_value == 4)  slot_old_value = perk_slot_4_perk;
        
        
        if (slot_old_value == 1) perk1_toggle();
        if (slot_old_value == 2) perk2_toggle();
        if (slot_old_value == 3) perk3_toggle();
        if (slot_old_value == 4) perk4_toggle();
        if (slot_old_value == 5) perk5_toggle();
        if (slot_old_value == 6) perk6_toggle();
        if (slot_old_value == 7) perk7_toggle();
        if (slot_old_value == 8) perk8_toggle();
        if (slot_old_value == 9) perk9_toggle();
        if (slot_old_value == 10) perk10_toggle();
        if (slot_old_value == 11) perk11_toggle();
        if (slot_old_value == 12) perk12_toggle();


        if (slot_value == 1)
        {
            perk_slot_1_perk = new_value; 
            perk_slot_1.sprite = perk_images[new_value];
        }

        if (slot_value == 2)
        {
            perk_slot_2_perk = new_value; 
            perk_slot_2.sprite = perk_images[new_value];
        }

        if (slot_value == 3)
        {
            perk_slot_3_perk = new_value; 
            perk_slot_3.sprite = perk_images[new_value];
        }

        if (slot_value == 4)
        {
            perk_slot_4_perk = new_value; 
            perk_slot_4.sprite = perk_images[new_value];
        }
        slot_old_value = new_value;
        
        if (slot_old_value == 1) perk1_toggle();
        if (slot_old_value == 2) perk2_toggle();
        if (slot_old_value == 3) perk3_toggle();
        if (slot_old_value == 4) perk4_toggle();
        if (slot_old_value == 5) perk5_toggle();
        if (slot_old_value == 6) perk6_toggle();
        if (slot_old_value == 7) perk7_toggle();
        if (slot_old_value == 8) perk8_toggle();
        if (slot_old_value == 9) perk9_toggle();
        if (slot_old_value == 10) perk10_toggle();
        if (slot_old_value == 11) perk11_toggle();
        if (slot_old_value == 12) perk12_toggle();
        
        if (OnPerkSlotLogicEvent != null)
        {
            OnPerkSlotLogicEvent(this);
        }
        
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
    public void perk7_toggle()
    {
        perk7 = !perk7;
    }
    public void perk8_toggle()
    {
        perk8 = !perk8;
    }
    public void perk9_toggle()
    {
        perk9 = !perk9;
    }
    public void perk10_toggle()
    {
        perk10 = !perk10;
    }
    public void perk11_toggle()
    {
        perk11 = !perk11;
        pet_rock.SetActive(perk11);
    }
    public void perk12_toggle()
    {
        perk12 = !perk12;
    }
    public void hoe_customization_logic()
    {
        if (hoe_customization == 2)
        {
            Debug.Log("triggering hoe 2");
            hoe_animator.TryGetComponent(out TerrainInteractor terrainInteractor);
            terrainInteractor.voxelIDToPlace = 1;
            terrainInteractor.ReplaceBlockInPlace = false;
            terrainInteractor.toolType = TerrainInteractor.ToolType.Radius;
        }
        else
        {
            hoe_animator.TryGetComponent(out TerrainInteractor terrainInteractor);
            terrainInteractor.voxelIDToPlace = 0;
            terrainInteractor.ReplaceBlockInPlace = true;
            terrainInteractor.toolType = TerrainInteractor.ToolType.SingleBlock;
        }
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
