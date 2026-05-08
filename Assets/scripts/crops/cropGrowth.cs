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
    public List<cropSegment> segments_data;

    public GameObject next_point;
    public GameObject previous_point;
    public Quaternion procedural_rotation;

    public void Start()
    {
        
        for (int i = 0; i < maxGrowth; i++)
        {
            if (i == 0)
            {
                var bottom = Instantiate(bottom_section, transform.position, transform.rotation);
                next_point = bottom.transform.Find("next_point").gameObject;
                previous_point = bottom;
                segments.Add(bottom);
            }
            else
            {
                if (i == maxGrowth)
                {
                    var top = Instantiate(top_section, transform.position, procedural_rotation);
                    segments.Add(top);
                    next_point = null;;
                }
                else
                {
                    procedural_rotation = new Quaternion(transform.rotation.x, transform.rotation.y + (i * 90f),
                        transform.rotation.z, transform.rotation.w);
            
                    var middle = Instantiate(middle_section,previous_point.transform.position,procedural_rotation ); //the i * 90 is to make it turn more each segment
                    next_point = middle.transform.Find("next_point").gameObject;
                    previous_point = middle;
                    segments.Add(middle);
                }
            }
           

            
        }

        foreach (var segment in segments)
        { 
            //add a thing to add it to the segments data list thingy here
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


[Serializable]
public class cropSegment
{
    
    public GameObject segment;
    public GameObject _next_point;
    public GameObject _previous_point;

    [SerializeField]
    private int _dist_percentage;

    public float distance;


    void calculate_distance()
    {
        distance = Vector3.Distance(_previous_point.transform.position,_next_point.transform.position);
        dist_percentage = 0;
    }
    
    public int dist_percentage 
    {
        
        get
        {
            return _dist_percentage;
        }
        set
        {
            _dist_percentage = value;

            segment.transform.position = Vector3.Lerp(_previous_point.transform.position, _next_point.transform.position, (dist_percentage / 100));// previous_point.transform.position + (distance * (dist_percentage / 100));
        }
        
        
        
    }

}
