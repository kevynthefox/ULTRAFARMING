using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cropGrowth : MonoBehaviour
{
    public int maxGrowth;
    public int currentGrowth;
    public GameObject bottom_section;
    public GameObject middle_section;
    public GameObject top_section;

    public List<GameObject> segments;
    public int current_segment;

    public GameObject next_point;
    public Quaternion procedural_rotation;

    public void Start()
    {
        
        for (int i = 0; i < maxGrowth; i++)
        {
            if (i == 0)
            {
                var bottom = Instantiate(bottom_section, transform.position, transform.rotation);
                next_point = bottom.transform.Find("next_point").gameObject;
                segments.Add(bottom);
            }

            procedural_rotation = new Quaternion(transform.rotation.x, transform.rotation.y + (i * 90f),
                transform.rotation.z, transform.rotation.w);
            
            var middle = Instantiate(middle_section,next_point.transform.position,procedural_rotation ); //the i * 90 is to make it turn more each segment
            next_point = middle.transform.Find("next_point").gameObject;
            segments.Add(middle);

            if (i == maxGrowth)
            {
                var top = Instantiate(top_section, transform.position, procedural_rotation);
                segments.Add(top);
                next_point = null;;
            }
        }

        foreach (var segment in segments)
        {
            segment.transform.localScale = new Vector3(segment.transform.localScale.x, 0, segment.transform.localScale.z);
            segment.SetActive(false);
            //shrink them down until they're invisible so that they can grow, then turn them off to save processing power
        }
    }

    public void step_growth()
    {
        segments[current_segment].SetActive(true);
        //play an animation here
        
        current_segment++;
    }

    
}
