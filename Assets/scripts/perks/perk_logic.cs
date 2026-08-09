using System;
using System.Collections.Generic;
using statusEffects;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class perk_logic : MonoBehaviour
{

    public GameObject perk_menu;

    public int hoe_customization;
    public int scythe_customization;
    public int seed_bag_customization;
    public int watering_can_customization;

    public GameObject water_ball_prefab, fire_ball_prefab;

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


    public bool
        perk1; //rushing river. when you run out of watering can water, gain a speed boost and spawn water below your feet.

    public bool perk2; //slippery. increased weapon swing speed by how wet you are.
    public Animator trowel_animator;
    public Animator hoe_animator;
    public Animator scythe_animator;
    public Animator seed_bag_animator;
    public Animator watering_can_animator;

    public bool perk3; //slidey. while wet you have 0 friction with the ground.
    public bool no_friction;

    public bool
        perk4; //dehydration. grab a crop to dehydrate it and make it more space efficient in your bag, at the cost of growing slower.

    public bool
        perk5; //overhydration. grab a crop to overhydrate it and make it less space efficient in your bag, but it grows faster.

    public bool perk6; //current hands. allows you to pick up multiple objects at once by giving them boids.

    public bool
        perk7; //blessing of the plants. for every seed you have, of the same type as the plant you are harvesting, gain a 'chance' to get an extra seed when harvesting. 'chance' works the same way that it does for the shopping

    public bool perk8; //filthy. digging applies dirty.

    public bool perk9; //blessing of the ground. increased jump height per dirty.

    public bool perk10; //green thumb. increased yield for manually planted crops(non thrown)

    public bool perk11; //pet rock. n amount of your stock will be bought at minimum every time. n scaling with dirty.
    public GameObject pet_rock;

    public bool perk12; //unkempt charm. add a bonus to sell price based on how dirty you are.


    public bool perk13; // gimbal jets. gives you the ability to redirect your current velocity to wherever you are looking. ability is triggered by a button like "perk 2 ability button" rather than "gimbal jet button"

    public GameObject camera;
    public float current_velocity;

    public bool perk14; //elemental heat. fire boosts the effects of other elemental buffs(like wet and dirty). it boosts those effects by increasing their max stack by roundtoint mathf.pow firemult, firestack. (done this way because this is less complicated than other ways)

    public bool perk15; //pocket rocket. consume max stacks of water, dirt, and fire, and some amount of money, to summon a rocket that rapidly transports you and anything inside of it straight home.
    public GameObject rocket;
    public GameObject cursor;
    bool fire_check;
    bool wet_check;
    bool dirt_check;
    bool money_check;
    public money_holder money;

    public bool perk16; //gust of steam. press a button to consume all of your fire and water, then apply fire * water amount of force forward(the cursor's version of foward, meaning it can be rotated to yourself) to all objects in front of the cursor. 
    public steam_gust_cursor cursor_gust_log;

    public List<int> perk_state;

    public bool perk17; //crystalization. consumes all of your dirty and flaming, to give you health(simply adds health to your healthbar, even if that gives more health than your max. this enables a second healthbar for the crystalization buff btw, that will stretch across the screen to be comically long.
    public health_system healthSystem;

    public bool perk18; //primoridal flame. when you take damage, gain 1 stack of each elemental buff, and raise the max stack count of each elemental buff to 60. this may sound really good, but fire does damage and if you have more fire you die faster >:3
    public int default_max_stack;
    
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

    public void perk_slot_logic(int new_value, int slot_value) //slot value like 1 is slot 1
    {
        slot_old_value = 0;

        if (slot_value == 1) slot_old_value = perk_slot_1_perk;
        if (slot_value == 2) slot_old_value = perk_slot_2_perk;
        if (slot_value == 3) slot_old_value = perk_slot_3_perk;
        if (slot_value == 4) slot_old_value = perk_slot_4_perk;


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
        if (slot_old_value == 13) perk13_toggle();
        if (slot_old_value == 14) perk14_toggle();
        if (slot_old_value == 15) perk15_toggle();
        if (slot_old_value == 16) perk16_toggle();
        if (slot_old_value == 17) perk17_toggle();
        if (slot_old_value == 18) perk18_toggle();


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
        if (slot_old_value == 13) perk13_toggle();
        if (slot_old_value == 14) perk14_toggle();
        if (slot_old_value == 15) perk15_toggle();
        if (slot_old_value == 16) perk16_toggle();
        if (slot_old_value == 17) perk17_toggle();
        if (slot_old_value == 18) perk18_toggle();
        
        if (OnPerkSlotLogicEvent != null)
        {
            OnPerkSlotLogicEvent(this);
        }

    }


    public void Awake()
    {
        current = this;
    }

    public void Start()
    {
        health_system.OnPlayerDeathEvent += reset_stack_count_on_death;
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
        if (!perk3) no_friction = false;
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

    public void perk13_toggle()
    {
        perk13 = !perk13;
        TryGetComponent(out Rigidbody rb);
        if (perk13)
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }
    
    public void perk14_toggle()
    {
        perk14 = !perk14;
        perk14_logic();
    }
    public void perk15_toggle()
    {
        perk15 = !perk15;
    }
    public void perk16_toggle()
    {
        perk16 = !perk16;
    }
    public void perk17_toggle()
    {
        perk17 = !perk17;
    }
    public void perk18_toggle()
    {
        perk18 = !perk18;
        if (perk18)
        {
            default_max_stack = 60;
        }
        else
        {
            default_max_stack = 10;
        }
    }
    public void hoe_customization_logic()
    {
        hoe_animator.TryGetComponent(out TerrainInteractor terrainInteractor);
        if (hoe_customization == 2)
        {
            Debug.Log("triggering hoe 2");
            terrainInteractor.voxelIDToPlace = 1;
            terrainInteractor.ReplaceBlockInPlace = false;
            terrainInteractor.toolType = TerrainInteractor.ToolType.Radius;
        }
        else
        {
            terrainInteractor.voxelIDToPlace = 0;
            terrainInteractor.ReplaceBlockInPlace = true;
            terrainInteractor.toolType = TerrainInteractor.ToolType.SingleBlock;
        }

        if (hoe_customization == 3)
        {
            terrainInteractor.toolType = TerrainInteractor.ToolType.Radius;
        }
        else
        {
            terrainInteractor.toolType = TerrainInteractor.ToolType.SingleBlock;
        }
    }

    public void watering_can_customization_logic()
    {
        watering_can_animator.TryGetComponent(out watering_can can_logic);

        if (watering_can_customization == 1)
        {

            if (this.TryGetComponent(out wet_buff wet_logic))
            {
                can_logic.water_collider.size =
                    new Vector3(50 * Mathf.Pow(wet_logic.water_element_multiplier, wet_logic.stack_count), 3, 40);
                can_logic.water_drain_amount =
                    0.1f * Mathf.Pow(wet_logic.water_element_multiplier, wet_logic.stack_count);
            }
            else
            {
                can_logic.water_collider.size = new Vector3(100, 3, 40);
                can_logic.water_drain_amount = 0.2f;
            }

        }
        else
        {
            can_logic.water_collider.size = new Vector3(50, 3, 40);
            can_logic.water_drain_amount = 0.1f;

        }

        if (watering_can_customization == 3)
        {
            can_logic.watering_sign = -1;
        }
        else
        {
            can_logic.watering_sign = 1;
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
            if (this.TryGetComponent(out wet_buff wet_logic))
            {
                trowel_animator.speed = math.pow(wet_logic.water_element_multiplier, wet_logic.stack_count);
                hoe_animator.speed = math.pow(wet_logic.water_element_multiplier, wet_logic.stack_count);
                scythe_animator.speed = math.pow(wet_logic.water_element_multiplier, wet_logic.stack_count);
                seed_bag_animator.speed = math.pow(wet_logic.water_element_multiplier, wet_logic.stack_count);
                watering_can_animator.speed = math.pow(wet_logic.water_element_multiplier, wet_logic.stack_count);
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

    public void perk14_logic()
    {
        if (perk14)
        {
            if (this.TryGetComponent(out fire_buff fireBuff))
            {
                if (this.TryGetComponent(out wet_buff wetBuff))
                {
                    wetBuff.max_stack_count =
                        default_max_stack * Mathf.RoundToInt(Mathf.Pow(fireBuff.fire_element_multiplier, fireBuff.stack_count));
                }

                if (this.TryGetComponent(out dirty_buff dirtyBuff))
                {
                    dirtyBuff.max_stack_count =
                        default_max_stack * Mathf.RoundToInt(Mathf.Pow(fireBuff.fire_element_multiplier, fireBuff.stack_count));
                }
            }
        }
        else
        {
            if (this.TryGetComponent(out wet_buff wetBuff))
            {
                wetBuff.max_stack_count = default_max_stack;
                if (wetBuff.stack_count > wetBuff.max_stack_count) 
                    wetBuff.stack_count = wetBuff.max_stack_count;
                wetBuff.effect_display_text_stack.text = "x"+ wetBuff.stack_count;
            }

            if (this.TryGetComponent(out dirty_buff dirtyBuff))
            {
                dirtyBuff.max_stack_count = default_max_stack;
                if (dirtyBuff.stack_count > dirtyBuff.max_stack_count)
                    dirtyBuff.stack_count = dirtyBuff.max_stack_count;
                dirtyBuff.effect_display_text_stack.text = "x"+ dirtyBuff.stack_count;
            }
        }

    }



    public void perk1_ability(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            perk_ability_logic(0);
        }
    }
    public void perk2_ability(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            perk_ability_logic(1);
        }
    }
    public void perk3_ability(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            perk_ability_logic(2);
        }
    }
    public void perk4_ability(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            perk_ability_logic(3);
        }
    }

    public void perk_ability_logic(int perk_activated)
    {
        perk_state.Clear();
        perk_state.Add(perk_slot_1_perk);
        perk_state.Add(perk_slot_2_perk);
        perk_state.Add(perk_slot_3_perk);
        perk_state.Add(perk_slot_4_perk);
        
        if (perk_state[perk_activated] == 13)
        {
            Debug.Log("perk_slot_1_perk");
            TryGetComponent(out Rigidbody rb);
            current_velocity = rb.linearVelocity.magnitude;
            rb.linearVelocity = camera.transform.forward * current_velocity;
        }
        if (perk_state[perk_activated] == 15)
        {

            if (this.TryGetComponent(out fire_buff fireBuff))
            {
                if (fireBuff.stack_count >= fireBuff.max_stack_count)
                {
                    fire_check = true;
                }
                else
                {
                    fire_check = false;
                }
            }
            else
            {
                fire_check = false;
            }
            if (this.TryGetComponent(out wet_buff wetBuff))
            {
                if (wetBuff.stack_count >= wetBuff.max_stack_count)
                {
                    wet_check = true;
                }
                else
                {
                    wet_check = false;
                }
            }
            else
            {
                wet_check = false;
            }
            if (this.TryGetComponent(out dirty_buff dirtyBuff))
            {
                if (dirtyBuff.stack_count >= dirtyBuff.max_stack_count)
                {
                    dirt_check = true;
                }
                else
                {
                    dirt_check = false;
                }
            }
            else
            {
                dirt_check = false;
            }
            if (money.money >= 500)
            {
                money_check = true;
            }
            else
            {
                money_check = false;
            }

            if (fire_check && wet_check && dirt_check && money_check)
            {
                rocket.transform.position = new Vector3(cursor.transform.position.x, 29, cursor.transform.position.z);
                rocket.transform.localEulerAngles = new Vector3(0,cursor.transform.eulerAngles.y,0);

                fireBuff.time_remaining = 0.1f;
                dirtyBuff.time_remaining = 0.1f;
                wetBuff.time_remaining = 0.1f;
                money.money_update(-500);
            }
            
        }
        if (perk_state[perk_activated] == 16)
        {
            if (TryGetComponent(out fire_buff fireBuff) && TryGetComponent(out wet_buff wetBuff))
            {
                cursor_gust_log.StartCoroutine(cursor_gust_log.pow(10 * fireBuff.stack_count * wetBuff.stack_count));
                fireBuff.time_remaining = 0.1f;
                wetBuff.time_remaining = 0.1f;
            }
            
            
        }
        if (perk_state[perk_activated] == 17)
        {
            if (TryGetComponent(out fire_buff fireBuff) && TryGetComponent(out dirty_buff dirtyBuff))
            {
                healthSystem.health += dirtyBuff.stack_count * fireBuff.stack_count;
                fireBuff.time_remaining = 0.1f;
                dirtyBuff.time_remaining = 0.1f;
            }
        }

        if (perk_state[perk_activated] == 18) 
        {
            default_max_stack += 60;
        }
    }

    public void reset_stack_count_on_death(health_system healthSystem)
    {
        if (perk18)
        {
            default_max_stack = 60;
        }
        else
        {
            default_max_stack = 10;
        }
    }
}
