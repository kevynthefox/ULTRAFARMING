using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace statusEffects
{


//[CreateAssetMenu(fileName = "StatusEffectTemplate", menuName = "status effect template")] //note, this may only let you add copies of this script via scriptable objects.. so you might just have to copy and paste this script a lot instead.
    public class rushing_river_buff : MonoBehaviour
    {
        public float time_remaining, time_max = 60; //if time max is 0 then the effect lasts until some other condition is met. like having a speed boost until you jump?

        public int stack_count, max_stack_count = 2;

        public Canvas effect_display_area;
        public GameObject effect_display;
        public GameObject effect_display_prefab;
        public TextMeshProUGUI effect_display_text_time;
        public TextMeshProUGUI effect_display_text_stack;

        public int apply_type = 1; //1 means applies on start, 2 means applies over time, 3 means applies when the effect ends(ie a burst of damage when the effect ends)

        public float effect_repetition_time = 0.5f;

        public bool moved_version; //this bool exists as a way to check if this is the script that has been created by the script moving to the parent object.

        public float speed_multiplier = 5;
        
        public void Start()
        {
            if (moved_version == false)
            {
                if (this.transform.parent.TryGetComponent(out rushing_river_buff this_script)) //change statuseffecttemplate out for the name of the script.
                {
                    this_script.add();
                }
                else
                {
                    transform.parent.AddComponent<rushing_river_buff>().moved_version = true;
                }

                Destroy(this.gameObject);
            }
            else
            {
                
                stack_count = 1;
                time_remaining = time_max;
                
                effect_display_prefab = StatusEffectAdder.current.statusEffect_displays[0];
                
                effect_display_area = GetComponentInChildren<Canvas>();
                effect_display = Instantiate(effect_display_prefab);
                effect_display.transform.SetParent(effect_display_area.transform.GetChild(0));
                effect_display_text_time = effect_display.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                effect_display_text_stack = effect_display.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                
                effect_display_text_stack.text = "x"+stack_count;

                //set the state of the variables for the script upon instantiation on the parent object, here.(as in what the base states of the variables should be.)

                //keep in mind that anything to consolodate the effects must happen before the start in this object, so before the effect is added.
                StartCoroutine(apply_effect());
                StartCoroutine(timer_tracker());
            }
        }

        public IEnumerator apply_effect()
        {
            if (apply_type == 1)
            {
                //apply 1 time, at the start.
                if (this.TryGetComponent(out FirstPersonMovement movement))
                {
                    for (int i = 0; i < stack_count; i++)
                    {
                        if (StatusEffectAdder.current.player.TryGetComponent(out wet_buff wet))
                        {
                            movement.speed *= (speed_multiplier * math.pow(wet.water_element_multiplier, wet.stack_count));
                        }
                        else
                        {
                            movement.speed *= speed_multiplier;
                        }
                    }
                }
            }

            //if (apply_type == 2)
            //{ with this specific buff it applies at start AND does a thing every once in a while.
                while (time_remaining > 0)
                {
                    Vector3 below_feet = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
                    var water_ball = Instantiate(perk_logic.current.water_ball_prefab, below_feet, transform.rotation);
                    yield return new WaitForSeconds(effect_repetition_time);
                }
            //}

        }

        public IEnumerator timer_tracker()
        {
            if (time_max != 0)
            {
                while (time_remaining > 0)
                {

                    time_remaining -= 0.1f;
                    effect_display_text_time.text = time_remaining.ToString();
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
                if (this.TryGetComponent(out FirstPersonMovement movement))
                {
                    for (int i = 0; i < stack_count; i++)
                    {
                        movement.speed /= speed_multiplier;
                    }
                }
                //add logic here to undo the effect.
                stack_count += 1;
                StartCoroutine(apply_effect());
                time_remaining = time_max;
                effect_display_text_stack.text = "x"+stack_count;
            }
        }

        public void remove()
        {

            if (apply_type == 3)
            {
                //can add logic here like "apply x amount of damage to the target now"
            }
            else
            {
                
                if (this.TryGetComponent(out FirstPersonMovement movement))
                {
                    for (int i = 0; i < stack_count; i++)
                    {
                        movement.speed /= speed_multiplier;
                    }
                }
                
                //add logic here to undo what has been done.
            }
            
            Destroy(effect_display);
            effect_display = null;
            effect_display_text_time = null;

            Destroy(this); //doesnt destroy the gameobject, destroys this script on it.
        }
    }
}