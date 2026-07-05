using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class task_assigner : MonoBehaviour
{

    public List<Transform> dig_points;
    public List<Transform> crops;
    public Transform site_exit;
    public Transform waiting_area;

    public int place_in_shop_chance; //every 3 out of 100 people go to our shop. so, instead of randomizing it, the first 3 people go and then when we reach person 100 we start over
    public int shop_chance;
    public GameObject shopper_prefab;
    public Transform shopper_spawn_point;
    public GameObject your_shopper; //this is here to help make sure that you dont get several shoppers at once.
    public int shop_priority;
    public int times_to_clone; //amount of new shoppers spawned per one destroyed
    
    public void OnTriggerEnter(Collider other) 
    {
        //Debug.Log("collided with something");
        if (other.CompareTag("employee"))
        {
            if (other.TryGetComponent( out nav_pathfinding nav))
            {
                //Debug.Log("collided with an employee");
                nav.current_work_site = this;
                if (nav.duty_type == 1)
                {
                    nav.destinations = dig_points;
                    nav.destination_type = 1;
                    nav.move_towards_first_Destination();
                }
                
                if (nav.duty_type == 2)
                {
                    nav.destinations = dig_points;
                    nav.destination_type = 2;
                    nav.move_towards_first_Destination();
                }
                
                if (nav.duty_type == 3)
                {
                    if (crops.Count > 0)
                    {
                        nav.destinations = crops;
                        nav.destination_type = 3;
                        nav.move_towards_first_Destination();
                    }
                    else
                    {
                        nav.destination = waiting_area;
                        nav.destination_type = 9;
                        nav.move_towardsDestination();
                    }
                    
                }
                
                if (nav.duty_type == 4)
                {
                    if (crops.Count > 0)
                    {
                        nav.destinations = crops;
                        nav.destination_type = 4;
                        nav.move_towards_first_Destination();
                    }
                    else
                    {
                        nav.destination = waiting_area;
                        nav.destination_type = 9;
                        nav.move_towardsDestination();
                    }

                    
                    
                }

                if (nav.duty_type == 5) //shopping duty type
                {
                    if (dig_points[0].transform.parent.TryGetComponent(out sell seller))
                    {
                        if (seller.products.Count > 0)
                        {
                            if (your_shopper == null)
                            {
                                //Debug.Log("collided with type 5 (shopper)");
                                if (place_in_shop_chance <= shop_chance)
                                {
                                    nav.destinations = dig_points; //list of shops including ours.
                                    nav.destination_type = 10;
                                    nav.move_towards_first_Destination();
                                    your_shopper = nav.gameObject;
                                }
                                else
                                {
                                    nav.destinations.Add(crops[UnityEngine.Random.Range(0,crops.Count)]); //list of shops not including ours
                                    nav.destinations.Add(crops[UnityEngine.Random.Range(0,crops.Count)]);
                                    nav.destinations.Add(crops[UnityEngine.Random.Range(0,crops.Count)]);
                                    nav.destination_type = 11;
                                    nav.move_towards_first_Destination();
                                }

                                place_in_shop_chance++;
                                if (place_in_shop_chance > 100)
                                {
                                    place_in_shop_chance = 1;
                                }
                            }
                            else
                            {
                                nav.destinations.Add(crops[UnityEngine.Random.Range(0,crops.Count)]); //list of shops not including ours
                                nav.destinations.Add(crops[UnityEngine.Random.Range(0,crops.Count)]);
                                nav.destinations.Add(crops[UnityEngine.Random.Range(0,crops.Count)]);
                                nav.destination_type = 11;
                                nav.move_towards_first_Destination();
                            }
                        }
                        else
                        {
                            nav.destinations.Add(crops[UnityEngine.Random.Range(0,crops.Count)]); //list of shops not including ours
                            nav.destinations.Add(crops[UnityEngine.Random.Range(0,crops.Count)]);
                            nav.destinations.Add(crops[UnityEngine.Random.Range(0,crops.Count)]);
                            nav.destination_type = 11;
                            nav.move_towards_first_Destination();
                        }

                        nav.agent.avoidancePriority = shop_priority;
                        shop_priority++;
                    }
                }
            }
        }
    }

    public void shopper_cycler(GameObject shopper)
    {
        Destroy(shopper);
        for (int i = 0; i < times_to_clone; i++)
        {
            Instantiate(shopper_prefab, shopper_spawn_point.position, Quaternion.identity);
        }

        if (shop_priority >= 80)
        {
            shop_priority = 0;
            times_to_clone--;
        }
    }
}
