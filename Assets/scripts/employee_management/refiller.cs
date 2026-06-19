using System;
using UnityEngine;

public class refiller : MonoBehaviour
{
    public int thing_to_refill; //0 is water, 1 is corn seeds.
    public int quantity_availabe;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("employee"))
        {
            if (other.TryGetComponent(out nav_pathfinding employee_logic))
            {
                if (thing_to_refill == 0)
                {

                }

                if (thing_to_refill == 1)
                {
                    employee_logic.planter.plants_left = quantity_availabe;
                    quantity_availabe = 0;
                }
            }
        }
    }
    
    
}
