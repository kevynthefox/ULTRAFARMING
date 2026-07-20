using System;
using System.Collections;
using TMPro;
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

    private void Start()
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
                    animator.Play("can_down");
                }
            }
            
            if (context.canceled)
            {
                watering = false;
                animator.SetBool("can_down", false);
                audio_source.Stop();
                if (water_count > 0) audio_source.PlayOneShot(water_end);
            }
        }
    }

    public IEnumerator water_tracker()
    {
        while (Time_manager.current.gameObject.activeSelf == true)
        {
            if (watering)
            {
                if (water_count > 0)
                {
                    water_count -= water_drain_amount;
                    update_texts();
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
                        growth.growth_rate *= speed_multiplier;
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
        if (weapon_wheel_controller.current.selected_weapon == 5)
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
        Debug.Log("bag placed");
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
                water_count = max_water_count;
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
