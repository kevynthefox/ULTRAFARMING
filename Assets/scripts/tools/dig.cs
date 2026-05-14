using System;
using System.Collections;
using UnityEngine;

public class dig : MonoBehaviour
{

    public float max_distance;
    public LayerMask layerMask;

    public void Update()
    {
        IsHeadingForCollision();

    }
    
    bool IsHeadingForCollision() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, max_distance, layerMask)) 
        {
            Debug.Log("hitting the thing");
            Debug.DrawRay(transform.position, transform.forward, Color.blue);
            hit.collider.gameObject.GetComponent<RMD_Deformation>().triggered_by_collision_proxy(this.gameObject.GetComponent<Collision>());
            hit.collider.gameObject.GetComponent<RMD_Deformation>().impulse = 10;
            hit.collider.gameObject.GetComponent<RMD_Deformation>().contactPoint = hit.point;
            return true;
            
        } 
        else 
        {
            Debug.DrawRay(transform.position, transform.forward, Color.red);
            return false;
        }
    }
}
