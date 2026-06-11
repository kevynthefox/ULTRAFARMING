using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Time_manager : MonoBehaviour
{
    public static Time_manager current;

    public float time;

    public float time_incrementer;
    public float time_wait; //how long is waited for the next thing of time to be ticked up(like how games have different time scales than irl)
    
    private void Awake()
    {
        current = this;
        StartCoroutine(sun_rotation());
    }

    IEnumerator sun_rotation()
    {
        while (this.enabled == true)
        {
            time += time_incrementer;
            transform.localEulerAngles = new Vector3(time,0,0);// = new Vector3(time,0,0);

            if (time >= 360)
            {
                time = 0;
            }
            yield return new WaitForSeconds(time_wait);
        }
    }
}
