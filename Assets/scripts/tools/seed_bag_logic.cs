using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class seed_bag_logic : MonoBehaviour
{
    public Transform return_point;
    public TextMeshProUGUI display;
    public Canvas canvas;
    public MeshFilter meshFilter;
    public Mesh mesh_held, mesh_placed;
    public Animator animator;
    //public BoxCollider boxCollider;

    public List<GameObject> seeds;
    public float throw_speed;
    public float throw_spacing;
    public int max_seed_count;
    public List<Transform> throw_points;
    public GameObject throw_point_prefab;
    public List<GameObject> throw_points_to_destroy;
    //public int seed_to_throw;
    public Transform throw_point_origin;//ie when throwpoints are created, this is where they are made

    public bool placed;
    public bool placing_picking;

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
        Physics.Raycast(return_point.transform.position,-return_point.transform.up, out RaycastHit hit,100);
        transform.position = new Vector3(hit.point.x, hit.point.y + 0.25f, hit.point.z);
        transform.localEulerAngles = new Vector3(90, 90,0);
        //boxCollider.enabled = true;
       
        StartCoroutine(wait_place_set());
    }

    public IEnumerator wait_place_set()
    {
        yield return new WaitForSeconds(1f);
        placed = true;
    }
    public void bag_pickup()
    {
        Debug.Log("bag picked up");
        meshFilter.mesh = mesh_held;
        //boxCollider.enabled = false;
        transform.parent = return_point;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = new Vector3(90, 90,0);
        
        placed = false;
    }
    
    
    public void OnTriggerStay(Collider other)
    {
        if (placed == true)
        {
            if (other.CompareTag("sellable"))
            {
                if (seeds.Count < max_seed_count)
                {
                    if (!seeds.Contains(other.gameObject))
                    {
                        seeds.Add(other.gameObject);
                        other.transform.localScale *= 0.1f;
                        other.transform.parent = this.transform;
                        other.transform.localPosition = Vector3.zero;
                        if (other.TryGetComponent(out Rigidbody rb))
                        {
                            Destroy(rb);
                        }
                    }
                }
            }

            if (other.CompareTag("Player"))
            {
                if (placing_picking == true)
                {
                    bag_pickup();
                }
            }
        }
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
            GameObject new_point = Instantiate(throw_point_prefab,throw_point_origin.position,throw_point_origin.rotation);
            new_point.transform.SetParent(throw_point_origin);
            if (i == 0 ) new_point.transform.localEulerAngles = new Vector3( 0,((seeds_throwing) * throw_spacing/2),0);
            if (i != 0)  new_point.transform.localEulerAngles = new Vector3( 0,throw_points[0].transform.parent.localEulerAngles.y - (i * throw_spacing),0);//this will cause the first throw point to be the left most and then every other point will add onto that
            throw_points.Add(new_point.transform.GetChild(0).transform);
        }
    }
    
    public void re_scale_seed(GameObject seed)
    {
        seed.transform.localScale /= 0.1f; 
    }
    
    public void throw_seed(Transform throw_point,GameObject seed)
    {
        seed.transform.localPosition = throw_point.localPosition;
        seed.AddComponent<Rigidbody>().useGravity = true;
    }

    public void throw_single_seed()
    {
        change_seed_spread(1);
        throw_seed(throw_points.First(),seeds.First());
        if (seeds.First().TryGetComponent(out Rigidbody rb))
        {
            rb.AddExplosionForce(throw_speed,throw_point_origin.position,20,10);
        }
        seeds.Remove(seeds.First());
    }

    public void throw_all_seeds()
    {
        change_seed_spread(seeds.Count);
        for (int i = 0; i < throw_points.Count; i++)// (Transform throwPoint in throw_points)
        {
            re_scale_seed(seeds[i]);
            throw_seed(throw_points[i],seeds[i]);
            
        }

        foreach (GameObject seed in seeds)
        {
            if (seed.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(throw_speed,throw_point_origin.position,20,10);
            }
        }
        seeds.Clear();
    }
}
