using System;
using System.Collections;
using System.Collections.Generic;
using statusEffects;
using UnityEngine;

public class simple_flight : MonoBehaviour
{
    public float base_speed;
    public float speed;

    public List<Transform> targets;
    public Transform home;
    public bool on_way_home;
    public bool arrived_home;

    
    
    public void Start()
    {
        on_way_home = false;
        arrived_home = false;

        if (StatusEffectAdder.current.player.TryGetComponent(out fire_buff fire))
        {
            speed = base_speed * Mathf.Pow(fire.fire_element_multiplier, fire.stack_count);
        }
        
        StartCoroutine(mover());
    }

    private IEnumerator mover()
    {
        while (Time_manager.current.time_flowing)
        {
            //Debug.Log("flying");
            if (arrived_home)
            {
                yield break;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, targets[0].position, speed * Time.deltaTime);
            
            transform.LookAt(new Vector3(targets[0].position.x,targets[0].position.y,targets[0].position.z));
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public IEnumerator OnTriggerStay(Collider other)
    {
        if (targets.Contains(other.transform))
        {
            Debug.Log("reached a target, removing");
            targets.Remove(other.transform);

            

            if (on_way_home)
            {
                arrived_home = true;
                TryGetComponent(out harvester harv);
                harv.scytheFlight_return();
                yield break;
            }
            
            if (targets.Count == 0)
            {
                targets.Add(home);
                on_way_home = true;
                yield break;
            }
        }
    }
}
