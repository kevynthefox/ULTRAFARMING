using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planter : MonoBehaviour
{
    public GameObject plant;

    public bool plant_state;
    public int range;
    public List<Transform> plant_points;
    public Transform current_plant_point;
    public LayerMask plant_mask;
    public float delay_between_actions;
    public int plants_left;
    public TerrainInteractor un_digger;

    private void Start()
    {
        StartCoroutine(plant_loop());
    }
    
    public IEnumerator plant_loop()
    {
        while (this.enabled == true)
        {
            if (plant_state == true)
            {
                foreach( Transform plantPoint in plant_points)
                {
                    current_plant_point = plantPoint;
                    if (plants_left - 1 >= 0)
                    {
                        plant_crop();
                        plants_left -= 1;
                    }
                    
                }

            }

            yield return new WaitForSeconds(delay_between_actions);
        }

        if (plant_state == false)
        {
            yield break;
        }
    }

    public void plant_crop()
    {
        Physics.Raycast(new Ray(current_plant_point.position, current_plant_point.forward), out var hitInfo);
        Vector3 plant_pos = hitInfo.point; 
        
        un_digger.dig_pulse();
        Instantiate(plant,plant_pos, transform.rotation);
        
    }
    
}
