using System;
using System.Collections.Generic;
using System.Linq;
using statusEffects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class harvester : MonoBehaviour
{
    public List<harvestable_part> parts;

    public bool harvesting;

    public Transform output_point;
    public Vector3 point_to_output_at; //this is so fireballs can drop the crops right at where they are instead of all in the middle

    public nav_pathfinding holder;
    public bool holder_is_person;

    public bool is_scythe;
    public Animator animator;
    
    public float additional_parts;

    public bool is_fire_ball;

    public bool scythe_in_hand;
    public float radius_to_search;
    public simple_flight scythe_flight;
    public List<Transform> targets;
    public GameObject return_point;

    public void Start()
    {
        if (output_point == null)
        {
            output_point = transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (harvesting == true)
        {
            if (other.gameObject.CompareTag("growing_crop") || other.gameObject.CompareTag("finished_crop"))
            {
                parts.Add(new harvestable_part());
                parts.Last().part = other.gameObject.GetComponent<cropGrowth>().harvested_part;
                parts.Last().number_of_harvestable_parts = other.gameObject.GetComponent<cropGrowth>().number_of_harvestable_parts;
                parts.Last().local_sizeMultiplier = other.gameObject.GetComponent<cropGrowth>().local_sizeMultiplier;
                Destroy(other.gameObject);

                if (holder_is_person == true)
                {
                    foreach (harvestable_part part in parts)
                    {
                        holder.carry += part.number_of_harvestable_parts;
                    }

                    
                    harvesting = false;
                    Debug.Log("harvesting disabled");
                }

                if (is_scythe == true)
                {
                    if (perk_logic.current.scythe_customization == 1)
                    {
                        output_point = perk_logic.current.seed_bag_animator.GetComponent<seed_bag_logic>().crop_tp_point.transform;
                    }
                    else
                    {
                        output_point = transform.GetChild(4);
                    }
                    output();
                }

                if (is_fire_ball)
                {
                    point_to_output_at = other.transform.position;
                    output();
                }
            }
        }
    }

    
    

    [ContextMenu("output")]
    public void output()
    {
        foreach (var part in parts)
        {
            
            if (perk_logic.current.perk7)
            {
                if (perk_logic.current.perk8)
                {
                    if (StatusEffectAdder.current.player.TryGetComponent(out dirty_buff dirty))
                    {
                        additional_parts = Mathf.RoundToInt(perk_logic.current.seed_bag_animator.gameObject
                            .GetComponent<seed_bag_logic>().current_seed_count * Mathf.Pow(dirty.earth_element_multiplier,dirty.stack_count));
                    }
                    else
                    {
                        additional_parts = perk_logic.current.seed_bag_animator.gameObject.GetComponent<seed_bag_logic>().current_seed_count;
                    }
                }
                else
                {
                    additional_parts = perk_logic.current.seed_bag_animator.gameObject.GetComponent<seed_bag_logic>().current_seed_count;
                }
            }
            for (int i = 0; i < part.number_of_harvestable_parts + additional_parts; i++)
            {
                
                
                float sizeMultiplier = growthIncrementer.current.crop_size_multiplier + part.local_sizeMultiplier;
                var crop = Instantiate(part.part, output_point.position, Quaternion.identity);
                crop.transform.localScale = new Vector3(crop.transform.localScale.x * sizeMultiplier,crop.transform.localScale.y * sizeMultiplier,crop.transform.localScale.z * sizeMultiplier);
                
                crop.TryGetComponent(out cropGrowth growth);
                
                growth.local_sizeMultiplier = sizeMultiplier;
                crop.GetComponent<product_data_holder>().local_value_multiplier = sizeMultiplier;
                if (is_scythe || is_fire_ball) crop.GetComponent<Rigidbody>().isKinematic = true;


                
                
                if (is_fire_ball)
                {
                    if (perk_logic.current.perk4)
                    {
                        growth.hydrate(Mathf.RoundToInt(-1 * transform.localScale.x));
                    }
                    crop.transform.position = point_to_output_at;
                }
                
                if (perk_logic.current.scythe_customization == 2)
                {
                    float randomX = UnityEngine.Random.Range(-3, 3);
                    float randomY = UnityEngine.Random.Range(-3, 3);
                    float randomZ = UnityEngine.Random.Range(-3, 3);
                    
                    crop.transform.parent = this.transform;
                    crop.transform.localPosition = new Vector3(randomX, randomY, randomZ);
                    crop.transform.parent = null;
                    crop.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }
        parts.Clear();
        //point_to_output_at.Clear();
        if (holder_is_person == true)
        {
            holder.carry = 0;
            harvesting = false;
        }
    }
    
    
    public void play_scythe_swing(InputAction.CallbackContext context)
    {
        if (this.gameObject.activeSelf == true)
        {
            if (perk_logic.current.scythe_customization == 3)
            {
                Collider[] hitColliders = Physics.OverlapSphere(StatusEffectAdder.current.player.transform.position, radius_to_search);
                foreach (var hitCollider in hitColliders)
                {
                    //hitCollider.SendMessage("AddDamage");
                    if (hitCollider.CompareTag("finished_crop") || hitCollider.CompareTag("growing_crop"))
                    {
                        if (!targets.Contains(hitCollider.transform)) targets.Add(hitCollider.transform);
                    }
                }

                transform.parent = null;
                transform.position = StatusEffectAdder.current.player.transform.position;
                
                animator.enabled = false;
                
                scythe_flight.targets = targets;
                scythe_flight.enabled = true;
                scythe_flight.Start();
                
            }
            else
            {
                if (context.started)
                {
                    animator.Play("scythe_down");
                    //PlayRandomClip(audioSource,clips);
                    //animator.SetBool("scythe_down", true);
                    animator.SetBool("actively_doing_animation", true);
                }

                if (context.canceled)
                {
                    animator.SetBool("actively_doing_animation", false);
                    animator.SetBool("scythe_down", false);
                    harvesting = false;
                }
            }

            
        }
    }

    public void _scythe_down_true()
    {
        animator.SetBool("scythe_down", true);
        harvesting = true;
    }
}


[System.Serializable]
public class harvestable_part
{
    public GameObject part;
    public int number_of_harvestable_parts;
    public float local_sizeMultiplier;
}
