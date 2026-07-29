using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class seed_bag_logic : MonoBehaviour
{
    public Transform return_point;
    public TextMeshProUGUI display_overlay,display_world;
    public Canvas canvas_overlay,canvas_world;
    public MeshFilter meshFilter;
    public Mesh mesh_held, mesh_placed;
    public Animator animator;
    //public BoxCollider boxCollider;

    public List<GameObject> seeds;
    public float throw_speed;
    public float throw_spacing;
    public float max_seed_count,current_seed_count;
    public List<Transform> throw_points;
    public GameObject throw_point_prefab;

    public List<GameObject> throw_points_to_destroy;

    //public int seed_to_throw;
    public Transform throw_point_origin; //ie when throwpoints are created, this is where they are made

    public bool placed,placed_forWeaponWheel;
    public bool placing_picking;

    public float randomizer_bounds;

    public GameObject crop_tp_point;

    public void drop_bag(InputAction.CallbackContext context)
    {
        if (this.gameObject.activeSelf == true)
        {
            if (context.started)
            {
                if (!placed) bag_place();
                placing_picking = true;

            }

            if (context.canceled)
            {
                placing_picking = false;
            }
        }
    }

    public void bag_place()
    {
        Debug.Log("bag placed");
        transform.parent = null;
        meshFilter.mesh = mesh_placed;
        Physics.Raycast(throw_point_origin.transform.position, -throw_point_origin.transform.up, out RaycastHit hit,
            100);
        transform.position = new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z);
        transform.localEulerAngles = new Vector3(90, 90, 0);
        //boxCollider.enabled = true;

        if (seeds.Count > 0)
        {
            foreach (GameObject seed in seeds)
            {
                seed.transform.localPosition = new Vector3(seed.transform.localPosition.x, seed.transform.localPosition.y, seed.transform.localPosition.z - 2);
            }
        }
        
        placed_forWeaponWheel = true;
        StartCoroutine(wait_place_set());
    }

    public IEnumerator wait_place_set()
    {
        yield return new WaitForSeconds(0.1f);
        placed = true;
    }

    public void bag_pickup()
    {
        Debug.Log("bag picked up");
        meshFilter.mesh = mesh_held;
        //boxCollider.enabled = false;
        transform.parent = return_point;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = new Vector3(90, 90, 0);

        if (seeds.Count > 0)
        {
            foreach (GameObject seed in seeds)
            {
                seed.transform.localPosition = new Vector3(seed.transform.localPosition.x, seed.transform.localPosition.y , seed.transform.localPosition.z + 2);
            }
        }
        
        placed = false;
        placed_forWeaponWheel = false;
    }


    public void OnTriggerStay(Collider other)
    {
        if (placed == true || perk_logic.current.scythe_customization == 1)
        {
            if (other.CompareTag("sellable"))
            {
                if (other.TryGetComponent(out product_data_holder prod_data))
                {
                    if (prod_data.being_held == false)
                    {
                        if (current_seed_count < max_seed_count)
                        {
                            if (!seeds.Contains(other.gameObject))
                            {
                                seeds.Add(other.gameObject);
                                if (other.TryGetComponent(out cropGrowth growth))
                                {
                                    if (growth.hydration_scale != 0)
                                    {
                                        current_seed_count += growth.hydration_scale;
                                    }
                                    else
                                    {
                                        current_seed_count += 1;
                                    }
                                }
                                else
                                {
                                    current_seed_count += 1;
                                }
                                other.transform.localScale *= 0.1f;
                                other.transform.parent = this.transform;
                                float randomx = UnityEngine.Random.Range(-randomizer_bounds, randomizer_bounds);
                                float randomy = UnityEngine.Random.Range(-randomizer_bounds, randomizer_bounds);
                                float randomz = UnityEngine.Random.Range(-randomizer_bounds, randomizer_bounds);

                                if (placed == false && perk_logic.current.scythe_customization == 1)
                                {
                                    other.transform.localPosition = new Vector3(randomx,randomy,randomz+2);
                                }
                                else
                                {
                                    other.transform.localPosition = new Vector3(randomx,randomy,randomz);
                                }
                                
                                if (other.TryGetComponent(out Rigidbody rb))
                                {
                                    Destroy(rb); //if you set the bodies to kinematic, you float around like you're using thors hammer
                                }
                            }
                        }
                    }
                }
                
            }

            if (other.CompareTag("Player") && placed == true)
            {
                if (placing_picking == true)
                {
                    bag_pickup();
                }
            }
            
        }
        update_texts();
    }

    

    public void bag_remove(GameObject other)
    {
        
        if (placed == true)
        {
            Debug.Log("bag remove");
            //Debug.Log(other.tag);
        
        
            //Debug.Log("it was sellable");
            if (seeds.Contains(other.gameObject))
            {
                Debug.Log("removed object");
                //Debug.Log("it was in the list");
                if (other.TryGetComponent(out cropGrowth growth))
                {
                    if (growth.hydration_scale != 0)
                    {
                        current_seed_count -= growth.hydration_scale;
                    }
                    else
                    {
                        current_seed_count -= 1;
                    }
                }
                else
                {
                    current_seed_count -= 1;
                }
                seeds.Remove(other.gameObject);
                Debug.Log("removed");
                other.transform.localScale /= 0.1f;
                //other.transform.parent = null;
                other.AddComponent<Rigidbody>().useGravity = true;
            }
        
        
        }
        update_texts();
    }

    /*public int test_seed_amount;

    [ContextMenu("test seed spread")]
    public void test_seed_spread()
    {
        change_seed_spread(test_seed_amount);
    }*/

    public void change_seed_spread(int seeds_throwing)
    {
        foreach (Transform throwpoint in throw_points)
        {
            throw_points_to_destroy.Add(throwpoint.gameObject);
        }

        for (int i = 0; i < throw_points_to_destroy.Count; i++)
        {
            Destroy(throw_points[i].transform.parent.gameObject);
        }

        throw_points.Clear();
        throw_points_to_destroy.Clear();

        for (int i = 0; i < seeds_throwing; i++)
        {
            GameObject new_point =
                Instantiate(throw_point_prefab, throw_point_origin.position, throw_point_origin.rotation);
            new_point.transform.SetParent(throw_point_origin);
            if (seeds_throwing > 1)
            {
                if (i == 0)
                    new_point.transform.localEulerAngles = new Vector3(0, ((seeds_throwing) * throw_spacing / 2), 0);
            }
            else
            {
                new_point.transform.localEulerAngles = new Vector3(0, 0, 0);
            }

            if (i != 0)
                new_point.transform.localEulerAngles = new Vector3(0,
                    throw_points[0].transform.parent.localEulerAngles.y - (i * throw_spacing),
                    0); //this will cause the first throw point to be the left most and then every other point will add onto that
            throw_points.Add(new_point.transform.GetChild(0).transform);
        }
    }

    public void re_scale_seed(GameObject seed)
    {
        seed.transform.localScale /= 0.1f;
    }

    public void throw_seed(Transform throw_point, GameObject seed)
    {
        seed.transform.parent = null;
        seed.transform.position = throw_point.position;
        seed.AddComponent<Rigidbody>().useGravity = true;

    }

    public void throw_single_seed()
    {
        change_seed_spread(1);
        throw_seed(throw_points.First(), seeds.First());
        if (seeds.First().TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = throw_points.First().forward * throw_speed;
        }

        seeds.First().GetComponent<cropGrowth>().manual_or_thrown = true;
        
        if (seeds.First().TryGetComponent(out cropGrowth growth))
        {
            if (growth.hydration_scale != 0)
            {
                current_seed_count -= growth.hydration_scale;
            }
            else
            {
                current_seed_count -= 1;
            }
        }
        else
        {
            current_seed_count -= 1;
        }
        seeds.Remove(seeds.First());
        

        update_texts();
    }

    public void throw_all_seeds()
    {
        change_seed_spread(seeds.Count);
        for (int i = 0; i < throw_points.Count; i++) // (Transform throwPoint in throw_points)
        {
            re_scale_seed(seeds[i]);
            throw_seed(throw_points[i], seeds[i]);

        }

        for (int i = 0; i < seeds.Count; i++)
        {
            if (seeds[i].TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = throw_points[i].forward * throw_speed;
                
            }
            seeds[i].GetComponent<cropGrowth>().manual_or_thrown = true;
        }
        
        

        seeds.Clear();
        
        update_texts();
    }



    public void animate_seed_single(InputAction.CallbackContext context)
    {
        if (weapon_wheel_controller.current.selected_weapon == 4 && placed == false)
        {
            if (context.started)
            {
                if (seeds.Count > 0)
                {
                    animator.SetInteger("throw_seed", 1);
                }
                else
                {
                    animator.SetInteger("throw_seed", 0);
                }
            }

            if (context.canceled)
            {
                animator.SetInteger("throw_seed", 0);
            }
        }
    }

    public void animate_seed_all(InputAction.CallbackContext context)
    {
        if (weapon_wheel_controller.current.selected_weapon == 4 && placed == false)
        {
            if (context.started)
            {
                if (seeds.Count > 0)
                {
                    animator.SetInteger("throw_seed", 2);
                }
                else
                {
                    animator.SetInteger("throw_seed", 0);
                }
            }

            if (context.canceled)
            {
                animator.SetInteger("throw_seed", 0);
            }
        }
    }

    public void in_single_throw_trigger() //the event that is triggered by the throw single seed animation
    {
        re_scale_seed(seeds.First());
        throw_single_seed();
        
        
    }

    public void in_all_throw_trigger() //i could just use throw all seeds but im putting this here for consistency
    {
        throw_all_seeds();
    }

    public void update_texts()
    {
        if (placed == false)
        {
            canvas_overlay.enabled = true;
            canvas_world.enabled = false;
            
            display_overlay.text = "seeds left: " + current_seed_count + "/" + max_seed_count;
        }
        else
        {
            canvas_overlay.enabled = false;
            canvas_world.enabled = true;
            
            display_world.text = "seeds left: " + current_seed_count + "/" + max_seed_count;
        }
    }
}
