using System;
using System.Collections;
using statusEffects;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class watering_can : MonoBehaviour
{
    public bool watering;
    public float speed_multiplier;
    
    public Animator animator;
    public AudioSource audio_source;

    public AudioClip water_middle;
    public AudioClip water_end;
    
    public float water_count,max_water_count,water_drain_amount,water_drain_rate;

    public Canvas canvas_world, canvas_player;
    public TextMeshProUGUI text_world, text_player;
    public bool placed,placed_forWeaponWheel;
    public bool placing_picking;
    public GameObject place_origin;
    public Transform return_point;

    public BoxCollider water_collider;
    
    public void OnEnable()
    {
        StartCoroutine(water_tracker());
    }
    
    public void water(InputAction.CallbackContext context)
    {
        if (weapon_wheel_controller.current.selected_weapon == 5)
        {
            if (context.started)
            {
                if (water_count > 0)
                {
                    //Debug.Log("water start");
                    animator.Play("can_down");
                }
            }
            
            if (context.canceled)
            {
                //Debug.Log("water cancel");
                watering = false;
                animator.SetBool("can_down", false);
                audio_source.Stop();
                if (water_count > 0) audio_source.PlayOneShot(water_end);
            }
        }
    }

    

    public IEnumerator water_tracker()
    {
        while (Time_manager.current.time_flowing)
        {
            if (watering)
            {
                Debug.Log("watering is true");
                if (water_count > 0)
                {
                    Debug.Log("draining water");
                    water_count -= water_drain_amount;
                    Debug.Log("water drained");
                    update_texts();
                }
                else
                {
                    perk_logic.current.perk1_logic();
                }
            }

            
            yield return new WaitForSeconds(water_drain_rate);
        }
        
    }

    public IEnumerator OnTriggerEnter (Collider other)
    {
        while (watering)
        {
            if (other.CompareTag("growing_crop"))
            {
                if (other.TryGetComponent(out cropGrowth growth))
                {
                    if (water_count > 0)
                    {
                        if (perk_logic.current.watering_can_customization == 0 || perk_logic.current.watering_can_customization == 1)
                        {
                            growth.growth_rate *= speed_multiplier;
                        }
                        else
                        {
                            if (perk_logic.current.watering_can_customization == 2)
                            {
                                growth.local_sizeMultiplier = Mathf.Abs(growth.local_sizeMultiplier * speed_multiplier);
                                other.transform.localScale *= speed_multiplier;
                            }
                        }
                    }
                }
            }
            
            if (other.gameObject.CompareTag("sellable"))
            {
                if (other.gameObject.TryGetComponent(out cropGrowth growth))
                {
                    if (perk_logic.current.perk5)
                    {
                        Debug.Log("sped up plant");
                        
                        growth.growth_rate *= speed_multiplier;
                        
                        growth.hydrate(Mathf.RoundToInt(speed_multiplier));
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
    
    

    public void can_down_true()
    {
        animator.SetBool("can_down", true);
        watering = true;
        
    }

    public void play_middle_audio()
    {
        audio_source.PlayOneShot(water_middle);
    }
    
    
    public void can_drop (InputAction.CallbackContext context)
    {
        if (this.gameObject.activeSelf == true)
        {
            if (context.started)
            {
                if (!placed) place_can();
                placing_picking = true;

            }

            if (context.canceled)
            {
                placing_picking = false;
            }
        }
    }
    
    public void place_can()
    {
        //Debug.Log("bag placed");
        transform.parent = null;
        
        Physics.Raycast(place_origin.transform.position, -place_origin.transform.up, out RaycastHit hit,
            100);
        transform.position = new Vector3(hit.point.x, hit.point.y + .5f, hit.point.z);
        transform.localEulerAngles = new Vector3(90, 90, 0);
        
        
        update_texts();
        
        placed_forWeaponWheel = true;
        StartCoroutine(wait_place_set());
    }
    
    public IEnumerator wait_place_set()
    {
        yield return new WaitForSeconds(0.1f);
        placed = true;
    }
    
    public void pickup_can()
    {
        Debug.Log("bag picked up");
        transform.parent = return_point;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = new Vector3(90, 90, 0);
        
        update_texts();
        
        placed = false;
        placed_forWeaponWheel = false;
    }
    
    public void update_texts()
    {
        if (placed == false)
        {
            canvas_player.enabled = true;
            canvas_world.enabled = false;
            
            text_player.text = "water left: " + water_count + "/" + max_water_count;
        }
        else
        {
            canvas_player.enabled = false;
            canvas_world.enabled = true;
            
            text_world.text = "water left: " + water_count + "/" + max_water_count;
        }
    }
    
    public void OnTriggerStay(Collider other)
    {
        if (placed == true)
        {
            if (other.CompareTag("water"))
            {
                water_count = 1;
            }
            
            if (other.CompareTag("Player"))
            {
                if (placing_picking == true)
                {
                    pickup_can();
                }
            }
            
        }
        update_texts();
    }
}
