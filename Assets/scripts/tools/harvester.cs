using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class harvester : MonoBehaviour
{
    public List<harvestable_part> parts;

    public bool harvesting;

    public Transform output_point;

    public nav_pathfinding holder;
    public bool holder_is_person;

    public bool is_scythe;
    public Animator animator;
    

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
            for (int i = 0; i < part.number_of_harvestable_parts; i++)
            {
                float sizeMultiplier = growthIncrementer.current.crop_size_multiplier + part.local_sizeMultiplier;
                var crop = Instantiate(part.part, output_point.position, Quaternion.identity);
                crop.transform.localScale = new Vector3(crop.transform.localScale.x * sizeMultiplier,crop.transform.localScale.y * sizeMultiplier,crop.transform.localScale.z * sizeMultiplier);
                crop.GetComponent<cropGrowth>().local_sizeMultiplier = sizeMultiplier;
                crop.GetComponent<product_data_holder>().local_value_multiplier = sizeMultiplier;
                if (is_scythe == true) crop.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        parts.Clear();
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
