using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace statusEffects
{


//[CreateAssetMenu(fileName = "StatusEffectTemplate", menuName = "status effect template")] //note, this may only let you add copies of this script via scriptable objects.. so you might just have to copy and paste this script a lot instead.
    public class fire_buff : MonoBehaviour
    {
        public float time_remaining, time_max = 60; //if time max is 0 then the effect lasts until some other condition is met. like having a speed boost until you jump?

        public int stack_count, max_stack_count = 10;

        public Canvas effect_display_area;
        public GameObject effect_display;
        public GameObject effect_display_prefab;
        public TextMeshProUGUI effect_display_text_time;
        public TextMeshProUGUI effect_display_text_stack;

        public int apply_type = 2; //1 means applies on start, 2 means applies over time, 3 means applies when the effect ends(ie a burst of damage when the effect ends)

        public float effect_repetition_time = 1;

        public bool moved_version; //this bool exists as a way to check if this is the script that has been created by the script moving to the parent object.

        public float fire_element_multiplier = 1.4f;

        public float damage_number = 1;
        
        public health_system health_sys;
        
        public void Start()
        {
            if (moved_version == false)
            {
                if (this.transform.parent.TryGetComponent(out fire_buff this_script)) //change statuseffecttemplate out for the name of the script.
                {
                    this_script.add();
                }
                else
                {
                    transform.parent.AddComponent<fire_buff>().moved_version = true;
                }

                Destroy(this.gameObject);
            }
            else
            {

                stack_count = 1;
                time_remaining = time_max;
                
                effect_display_prefab = StatusEffectAdder.current.statusEffect_displays[3];//replace with whatever number is associated with this effect
                
                effect_display_area = GetComponentInChildren<Canvas>();
                effect_display = Instantiate(effect_display_prefab);
                effect_display.transform.SetParent(effect_display_area.transform.GetChild(0));
                effect_display_text_time = effect_display.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                effect_display_text_stack = effect_display.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                
                effect_display_text_stack.text = "x"+stack_count;
                
                //set the state of the variables for the script upon instantiation on the parent object, here.(as in what the base states of the variables should be.)

                //keep in mind that anything to consolodate the effects must happen before the start in this object, so before the effect is added.
                
                health_system.OnPlayerDeathEvent += remove_on_death;
                
                TryGetComponent(out health_system health_sy);
                health_sys = health_sy;
                
                StartCoroutine(apply_effect());
                StartCoroutine(timer_tracker());
            }
        }

        public IEnumerator apply_effect()
        {
            //apply 1 time, at the start.
            if (perk_logic.current.perk14)
            {
                if (TryGetComponent(out wet_buff wet_buff))
                {
                    wet_buff.max_stack_count = 10 * Mathf.RoundToInt(Mathf.Pow(fire_element_multiplier, stack_count));
                }
                
                if (TryGetComponent(out dirty_buff dirtyBuff))
                {
                    dirtyBuff.max_stack_count = 10 * Mathf.RoundToInt(Mathf.Pow(fire_element_multiplier, stack_count));
                }
            }


            if (apply_type == 2)
            {
                while (time_remaining > 0)
                {
                    Debug.Log("damaging player via fire");
                    health_sys.health -= damage_number;
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
                //add logic here to undo the effect.
                
                stack_count += 1;
                StartCoroutine(apply_effect());
                
                effect_display_text_stack.text = "x"+stack_count;
            }
            time_remaining = time_max;
        }

        public void remove()
        {

            if (apply_type == 3)
            {
                //can add logic here like "apply x amount of damage to the target now"
            }
            else
            {
                //add logic here to undo what has been done.
            }
            
            Destroy(effect_display);
            effect_display = null;
            effect_display_text_time = null;
            
            if (perk_logic.current.perk14)
            {
                if (TryGetComponent(out wet_buff wetBuff))
                {
                    wetBuff.max_stack_count = 10;
                    if (wetBuff.stack_count > wetBuff.max_stack_count) wetBuff.stack_count = wetBuff.max_stack_count;
                    wetBuff.effect_display_text_stack.text = "x"+ wetBuff.stack_count;
                }
                
                if (TryGetComponent(out dirty_buff dirtyBuff))
                {
                    dirtyBuff.max_stack_count = 10;
                    if (dirtyBuff.stack_count > dirtyBuff.max_stack_count) dirtyBuff.stack_count = dirtyBuff.max_stack_count;
                    dirtyBuff.effect_display_text_stack.text = "x"+ dirtyBuff.stack_count;
                }
            }

            Destroy(this); //doesnt destroy the gameobject, destroys this script on it.
        }
        
        public void remove_on_death(health_system healthSystem)
        {
            Destroy(effect_display);
            effect_display = null;
            effect_display_text_time = null;

            if (perk_logic.current.perk14)
            {
                if (TryGetComponent(out wet_buff wetBuff))
                {
                    wetBuff.max_stack_count = 10;
                    if (wetBuff.stack_count > wetBuff.max_stack_count) wetBuff.stack_count = wetBuff.max_stack_count;
                    wetBuff.effect_display_text_stack.text = "x"+ wetBuff.stack_count;
                }
                
                if (TryGetComponent(out dirty_buff dirtyBuff))
                {
                    dirtyBuff.max_stack_count = 10;
                    if (dirtyBuff.stack_count > dirtyBuff.max_stack_count) dirtyBuff.stack_count = dirtyBuff.max_stack_count;
                    dirtyBuff.effect_display_text_stack.text = "x"+ dirtyBuff.stack_count;
                }
            }
            
            Destroy(this); //doesnt destroy the gameobject, destroys this script on it.
        }
    }
}