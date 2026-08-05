using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simple_flight : MonoBehaviour
{
    public float speed;

    public List<Transform> targets;
    
    public void Start()
    {
        StartCoroutine(mover());
    }

    private IEnumerator mover()
    {
        while (Time_manager.current.time_flowing)
        {
            Debug.Log("flying");
            transform.position = Vector3.MoveTowards(transform.position, targets[0].position, speed * 0.1f);
            
            transform.LookAt(new Vector3(targets[0].position.x,targets[0].position.y,targets[0].position.z));
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (targets.Contains(other.transform))
        {
            Debug.Log("reached a target, removing");
            targets.Remove(other.transform);
        }
    }
}
