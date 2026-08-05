using System;
using statusEffects;
using Unity.Mathematics;
using UnityEngine;

public class fire_ball : MonoBehaviour
{
    
    //public float speed_multiplier;
    public float lifetime;
    public void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("sellable"))
        {
            if (other.gameObject.TryGetComponent(out cropGrowth growth))
            {
                if (perk_logic.current.perk4)
                {
                    growth.hydrate(Mathf.RoundToInt(-1 * transform.localScale.x));
                }
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            StatusEffectAdder.current.addStatusEffect(StatusEffectAdder.current.player,3);
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
