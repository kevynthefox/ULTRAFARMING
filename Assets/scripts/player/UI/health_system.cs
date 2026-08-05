using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class health_system : MonoBehaviour
{
    public Image health_bar;
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
