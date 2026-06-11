using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class nav_pathfinding : MonoBehaviour
{
    public Transform destination;
    public int destination_type;
    public List<Transform> destinations;
    public int destination_number;//as in, where we are on the destination list

    public Transform paused_destination; //this is used to go back to where you were when being interrupted by break or having to refill stuff.
    public int paused_destination_type;
    
    public Transform break_area;
    public float break_time;

    public int carry_capacity;

    public int duty_type; //1 is tiller, 2 is planter, 3 is waterer, 4 is harvester
    public NavMeshAgent agent;
    
    [Header("purposes")]
    public TerrainInteractor digger;
    public planter planter;
    //when watering is added, add a behaviour here.
    public harvester harvester;
    public Transform refiller;
    
    [ContextMenu("set_to_destination")]
    void move_towardsDestination()
    {
       agent.SetDestination(destination.position);
    }

    float time
    {
        get
        {
            return Time_manager.current.time;
        }
        set
        {
            time = value;
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                do_purpose();
            }
        }
    }
    

    void do_purpose()
    {
        if (destination_type == 1)
        {
            digger.dig_pulse();
            destination_number++;
            if (destination_number == destinations.Count)
            {
                destination_type = 0;
            }
            destination = destinations[destination_number];
        }

        if (destination_type == 2)
        {
            if (planter.plants_left > 0)
            {
                planter.plant_crop();
                destination_number++;
                if (destination_number == destinations.Count)
                {
                    destination_type = 0;
                }
                destination = destinations[destination_number];
            }
            else
            {
                paused_destination = destination;
                paused_destination_type = destination_type;
                destination = refiller;
                destination_type = 6;
            }
        }
        
        if (destination_type == 3)//do watering stuff

        if (destination_type == 4)
        {
            harvester.gameObject.SetActive(true);
            destination_number++;
            if (destination_number == destinations.Count)
            {
                destination_type = 0;
            }
            destination = destinations[destination_number];
        }

        if (destination_type == 5) //depositer for harvester
        {
            harvester.gameObject.SetActive(true);
            harvester.output();
            destination = paused_destination;
            destination_type = paused_destination_type;
        }

        if (destination_type == 6) //refill point for seeds and water
        {
            //honestly the employee just kinda stands there for a second and the refiller should just refill everything.
            
            destination = paused_destination;
            destination_type = paused_destination_type;
        }
        
    }
    
    
    
}
