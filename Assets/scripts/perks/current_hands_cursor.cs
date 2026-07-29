using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class current_hands_cursor : MonoBehaviour
{
    public Pickup pickup;
    public BoidManager boidManager;
    public List<GameObject> objects_affected;

    public IEnumerator puppet()
    {
        while (Time_manager.current.time_flowing)
        {
            if (perk_logic.current.perk6)
            {
                if (pickup.grabbing)
                {
                    
                }
                else
                {
                    foreach (GameObject obj in objects_affected)
                    {
                        obj.GetComponent<Boid>().dead = true;
                        if (obj.TryGetComponent(out Rigidbody rb))
                        {
                            rb.isKinematic = false;
                        }
                    }
                    objects_affected.Clear();
                }
            }
            else
            {
                foreach (GameObject obj in objects_affected)
                {
                    obj.GetComponent<Boid>().dead = true;
                    if (obj.TryGetComponent(out Rigidbody rb))
                    {
                        rb.isKinematic = false;
                    }
                }
                objects_affected.Clear();
            }

            yield return new WaitForSeconds(1f);
        }
    }
    
    public void OnTriggerStay(Collider other)
    {
        if (perk_logic.current.perk6)
        {
            //Debug.Log("perk6 active");
            if (other.gameObject.layer == 0 || other.gameObject.layer == 8)
            {
                Debug.Log("is in an acceptable layer");
                if (pickup.grabbing)
                {
                    
                    if (other.TryGetComponent(out Boid boid))
                    {
                        boid.target = this.transform;
                        objects_affected.Add(other.gameObject);
                        if (other.TryGetComponent(out Rigidbody rb))
                        {
                            rb.isKinematic = true;
                        }

                        boid.dead = false;
                    }
                    else
                    {
                        other.AddComponent<Boid>();
                        boidManager.Start();
                    }
                    
                }
                
            }
        }
        
    }
}
