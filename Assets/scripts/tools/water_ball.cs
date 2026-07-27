using System;
using statusEffects;
using Unity.Mathematics;
using UnityEngine;

public class water_ball : MonoBehaviour
{
    
    public float speed_multiplier;
    public float lifetime;
    public void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.TryGetComponent(out watering_can watering_can_logic))
        {
            if (watering_can_logic.placed)
            {
                Debug.Log("water refilled");
                watering_can_logic.water_count = 2;// watering_can_logic.max_water_count;
            }
        }

        if (other.gameObject.CompareTag("growing_crop"))
        {
            if (other.gameObject.TryGetComponent(out cropGrowth growth))
            {
                Debug.Log("sped up plant");
                if (StatusEffectAdder.current.player.TryGetComponent(out wet_buff wet))
                {
                    growth.growth_rate *= (speed_multiplier * math.pow(wet.water_element_multiplier, wet.stack_count));
                }
                else
                {
                    growth.growth_rate *= speed_multiplier;
                }
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            StatusEffectAdder.current.addStatusEffect(StatusEffectAdder.current.player,1);
        }
    }

    public void Start()
    {
        Invoke("destroy",lifetime);
    }

    public void destroy()
    {
        Destroy(this.gameObject);
    }
}
