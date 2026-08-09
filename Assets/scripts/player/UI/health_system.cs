using System;
using System.Collections;
using statusEffects;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class health_system : MonoBehaviour
{
    public Image health_bar, health_bar_crystal;
    public TextMeshProUGUI health_text;
    
    public static event Action<health_system> OnPlayerDeathEvent;
    
    public float max_health;
    [SerializeField]
    private float _health;
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            if (!invincible)
            {
                if (value < _health)
                {
                    if (perk_logic.current.perk18)
                    {
                        StatusEffectAdder.current.addStatusEffect(StatusEffectAdder.current.player,1);
                        StatusEffectAdder.current.addStatusEffect(StatusEffectAdder.current.player,2);
                        StatusEffectAdder.current.addStatusEffect(StatusEffectAdder.current.player,3);
                    }
                }
                
                _health = value;
                health_bar.fillAmount = value / max_health;
                health_text.text = value + "/" + max_health;

                if (health <= 0)
                {
                    transform.position = new Vector3(0, 27, 0);

                    health = max_health;

                    if (OnPlayerDeathEvent != null)
                    {
                        OnPlayerDeathEvent(this);
                    }
                }

                if (health > max_health)
                {
                    if (!health_bar_crystal.gameObject.activeSelf) health_bar_crystal.gameObject.SetActive(true);

                    health_bar_crystal.fillAmount = (value - max_health) /(max_health*4);
                }
                else
                {
                    if (health_bar_crystal.gameObject.activeSelf) health_bar_crystal.gameObject.SetActive(false);
                }
            }
        }
    }

    public bool invincible;

    /*public bool regenerate;
    public 
*/
    public void Start()
    {
        Debug.Log(health);
        health = max_health;
    }

    /*public IEnumerator regeneration_tracker()
    {
        while (Time_manager.current.time_flowing)
        {
            if (regenerate)
            {
                health += max_health/
            }
        }
    }*/
}
