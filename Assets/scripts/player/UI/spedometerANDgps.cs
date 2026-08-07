using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class spedometerANDgps : MonoBehaviour
{

    public Rigidbody rb;

    public TextMeshProUGUI speed_and_gps;

    public void Start()
    {
        StartCoroutine(update_text());
    }

    public IEnumerator update_text()
    {
        while (Time_manager.current.time_flowing)
        {

            speed_and_gps.text = "speed: " + rb.linearVelocity.magnitude + "<br> coordinates: " + rb.position; 
            
            yield return new WaitForSeconds(0.5f);
        }
    }
    
}
