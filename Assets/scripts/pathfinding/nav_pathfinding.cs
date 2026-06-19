using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class nav_pathfinding : MonoBehaviour
{
    [Header("scalers")]
    public float time_waiting; //this is the time for the while loop for do purpose stuff.

    public bool multiple_points; //when this is true, set the plow to skip plants, the idea is that the amount of points is the length of each row, so just add the number of plants in a row
    
    [Header("destinations")]
    public Transform destination;
    public int destination_type;
    public List<Transform> destinations;
    public int destination_number;//as in, where we are on the destination list

    //public Transform paused_destination; //this is used to go back to where you were when being interrupted by break or having to refill stuff.
    //public int paused_destination_type;

    public List<Transform> site_destinations;
    public int site_destination_number;
    public List<Transform> dest_sit_temp;
    
    public Transform break_area;
    public float break_time;

    [Header("employee_info")]
    public int carry;
    public int carry_capacity;

    public int duty_type; //1 is tiller, 2 is planter, 3 is waterer, 4 is harvester
    public NavMeshAgent agent;
    public task_assigner current_work_site;
    
    
    [Header("purposes")]
    public TerrainInteractor digger;
    public planter planter;
    //when watering is added, add a behaviour here.
    public harvester harvester;
    public Transform refiller;
    public List<GameObject> purpose_objects;
    
    private void Start()
    {
        site_destination_number = 0;
        foreach (GameObject dest in GameObject.FindGameObjectsWithTag("site_destination"))
        {
            site_destinations.Add(dest.transform);
        }

        purpose_objects[duty_type - 1].SetActive(true);
        
        StartCoroutine(time_keeper());
        move_towards_first_Site_Destination();
    }

    [ContextMenu("set_to_destination")]
    public void move_towardsDestination()
    {
       agent.SetDestination(destination.position);
    }
    public void move_towards_first_Destination()
    {
        agent.SetDestination(destinations[0].position);
    }
    public void move_towards_first_Site_Destination()
    {
        agent.SetDestination(site_destinations[0].position);
    }

    public IEnumerator time_keeper()
    {
        while(this.enabled == true)
        {
            //Debug.Log("the time shish is working");
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                do_purpose();
            }

            if (time_waiting == 0) time_waiting = 1;
            yield return new WaitForSeconds(time_waiting);
        }
    }
    

    void do_purpose()
    {
        Debug.Log("trying to do purpose");
        if (destination_type == 1)
        {
            digger.dig_pulse();

            if (multiple_points == true)
            {
                destination_number += current_work_site.transform.parent.GetComponent<area_designator>().x_between;
            }
            else
            {
                destination_number++;
            }
            
            if (destination_number == destinations.Count)
            {
                destination_type = 8;
                destination = current_work_site.site_exit;
            }
            else
            {

                destination = destinations[destination_number];
            }
            move_towardsDestination();
            return;
        }

        if (destination_type == 2)
        {
            if (planter.plants_left > 0 + planter.plant_points.Count)
            {
                planter.work_site = current_work_site;
                foreach (Transform plantPoint in planter.plant_points)
                {
                    planter.plant_crop();
                    planter.current_plant_point = plantPoint;
                }

                if (multiple_points == true)
                {
                    destination_number += current_work_site.transform.parent.GetComponent<area_designator>().x_between;
                }
                else
                {
                    destination_number++;
                }
                
                if (destination_number == destinations.Count)
                {
                    destination_type = 8;
                    destination = current_work_site.site_exit;
                }
                else
                {

                    destination = destinations[destination_number];
                }
                move_towardsDestination();
            }
            else
            {
                
                destination = refiller;
                destination_type = 6;
                move_towardsDestination();
            }
            return;
        }

        if (destination_type == 3) //do watering stuff
        {
            return;
        }

        if (destination_type == 4)
        {
            harvester.harvesting = true;
            
            if (carry >= carry_capacity)
            {
                //paused_destination = destination;
                //paused_destination_type = destination_type;
                destination = refiller;
                
                harvester.harvesting = false;
                move_towardsDestination();
                Debug.Log("heading to drop off");
                destination_type = 5;
            }
            else
            {
                Debug.Log("carry was not over capacity, continuing work");
                
                if (multiple_points == true)
                {
                    destination_number += current_work_site.transform.parent.GetComponent<area_designator>().x_between;
                }
                else
                {
                    destination_number++;
                }
                
                if (destination_number == destinations.Count)
                {
                    destination_type = 8;
                    destination = current_work_site.site_exit;
                }
                else
                {

                    destination = destinations[destination_number];
                }

                move_towardsDestination();
            }
            
            return;
        }

        
        
        if (destination_type == 5) //depositer for harvester
        {
            harvester.output();
            //destination_number++;
            if (destination_number == destinations.Count)
            {
                destination_type = 8;
                destination = current_work_site.site_exit;
            }
            else
            {
                destination_type = 4;
                destination = destinations[destination_number];
            }
            move_towardsDestination();
            return;
        }

        if (destination_type == 6) //refill point for seeds and water
        {
            //honestly the employee just kinda stands there for a second and the refiller should just refill everything.

            planter.plants_left = refiller.GetComponent<refiller>().quantity_availabe;
            refiller.GetComponent<refiller>().quantity_availabe = 0;
            
            if (destination_number == destinations.Count)
            {
                destination_type = 8;
                destination = current_work_site.site_exit;
            }
            else
            {
                destination_type = 2;
                destination = destinations[destination_number];
            }
            move_towardsDestination();
            return;
        }
        
        if (destination_type == 7) //refill point for seeds and water
        {
            //honestly the employee just kinda stands there for a second and the refiller should just refill everything.
            
            
            
            if (destination_number == destinations.Count)
            {
                destination_type = 8;
                destination = current_work_site.site_exit;
            }
            else
            {
                destination_type = 3;
                destination = destinations[destination_number];
            }
            return;
        }

        if (destination_type == 8)
        {
            site_destination_number++;
            if (site_destination_number + 1 > site_destinations.Count)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                destination = site_destinations[site_destination_number];
                destination_type = 0;
                destination_number = 0;
                if (destination == site_destinations.Last()) destination_type = 8; //this is to cause it to step 1 forward and thus trigger the above check for if its greater than the amount of destinations
                move_towardsDestination();
            }
            return;
        }

        if (destination_type == 9)
        {

            destination = site_destinations[site_destination_number];
            move_towardsDestination();
            destination_type = 0;
            return;
        }
        
    }
    
    
    
}
