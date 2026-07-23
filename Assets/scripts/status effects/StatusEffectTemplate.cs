using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectTemplate", menuName = "status effect template")] //note, this may only let you add copies of this script via scriptable objects.. so you might just have to copy and paste this script a lot instead.
public class StatusEffectTemplate : MonoBehaviour
{
    public float time_remaining,time_max;//if time max is 0 then the effect lasts until some other condition is met. like having a speed boost until you jump?
    public int stack_count,max_stack_count;

    public GameObject effect_display;
    public GameObject effect_display_prefab;

    public int apply_type; //1 means applies on start, 2 means applies over time, 3 means applies when the effect ends(ie a burst of damage when the effect ends)
    public float effect_repetition_time;
    
    public void Start()
    {
       // if (this.gameObject.GetComponents<TryGetComponent(out this this_script) > 1)
        //keep in mind that anything to consolodate the effects must happen before the start in this object, so before the effect is added.
        StartCoroutine(apply_effect());
    }

    public IEnumerator apply_effect()
    {
        if (apply_type == 1)
        {
            //apply 1 time, at the start.
        }

        if (apply_type == 2)
        {
            while (time_remaining > 0)
            {
                
                yield return new WaitForSeconds(effect_repetition_time);
            }
        }
        
    }

    public IEnumerator timer_tracker()
    {
        if (time_max != 0)
        {
            while (time_remaining > 0)
            {
                
                
                
                time_remaining -= 0.1f;
                //add some logic here for updating the text of the effect display.
                yield return new WaitForSeconds(0.1f);
            }
            remove();
        }
        else
        {
            //add some logic here for checking things. ie checking if the player jumps or whatever. idk.
        }
    }

    public void add()
    {
        if (stack_count < max_stack_count)
        {
            //add logic here to undo the effect.
            StartCoroutine(apply_effect());
            stack_count += 1;
            time_remaining = time_max;
        }
    }
    
    public void remove()
    {

        if (apply_type == 3)
        {
            //can add logic here like "apply x amount of damage to the target now"
        }

        Destroy(this);//doesnt destroy the gameobject, destroys this script on it.
    }
}
