using System;
using System.Collections.Generic;
using UnityEngine;

public class task_assigner : MonoBehaviour
{

    public List<Transform> dig_points;
    public List<Transform> crops;
    public Transform site_exit;
    
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided with something");
        if (other.CompareTag("employee"))
        {
            if (other.TryGetComponent( out nav_pathfinding nav))
            {
                Debug.Log("collided with an employee");
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
                    nav.destinations = crops;
                    nav.destination_type = 3;
                    nav.move_towards_first_Destination();
                }
                
                if (nav.duty_type == 4)
                {
                    nav.destinations = crops;
                    nav.destination_type = 4;
                    nav.move_towards_first_Destination();
                }
            }
        }
    }
}
