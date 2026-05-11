using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cropGrowth : MonoBehaviour
{
    [Header("Growth data")]
    public int maxGrowth;
    public int growth_rate;
    
    [Header("sections")]
    public GameObject bottom_section;
    public GameObject middle_section;
    public GameObject top_section;

    [Header("segments")]
    public List<GameObject> segments;
    public int current_segment;
    public List<cropSegment> segments_data;

    [Header("points")]
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
                
                bottom.transform.parent = this.transform;
                add_segment_to_crop_segments(i,bottom);
                previous_point = this.gameObject;
                segments.Add(bottom);
            }
            else
            {
                if (i == maxGrowth)
                {
                    var top = Instantiate(top_section, transform.position, procedural_rotation);
                    segments.Add(top);
                    top.transform.parent = previous_point.transform;//this makes it so all segments above the previous one move up at once, so that the whole plant can actually grow instead of collecting in one spot
                    add_segment_to_crop_segments(i,top);
                    next_point = null;;
                    
                    
                }
                else
                {
                    procedural_rotation = new Quaternion(transform.rotation.x, transform.rotation.y + (i * 90f),
                        transform.rotation.z, transform.rotation.w);
            
                    var middle = Instantiate(middle_section,previous_point.transform.position,procedural_rotation ); //the i * 90 is to make it turn more each segment
                    next_point = middle.transform.Find("next_point").gameObject;
                    middle.transform.parent = previous_point.transform;//this makes it so all segments above the previous one move up at once, so that the whole plant can actually grow instead of collecting in one spot
                    add_segment_to_crop_segments(i,middle);
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
        segments_data[current_segment].dist_percentage += growth_rate;

        if (segments_data[current_segment].dist_percentage >= 100)
        {
            current_segment++;  //after current segment has finished growing, tick the next segment
        }
    }

    private void add_segment_to_crop_segments(int i, GameObject segment)
    {
        //using insert because with that, you can insert a null, and then populate it. I think...
        segments_data.Insert(i , null);
        segments_data[i]._segment = segment;
        segments_data[i]._next_point = next_point; //dont need to worry about getting these from the sepcific object since next point and previous point are global in this script. as in, when set by one object, they are set like that for all of them ig?
        segments_data[i]._previous_point = previous_point;
        segments_data[i].dist_percentage = 0;
    }
}


[Serializable]
public class cropSegment
{
    
    public GameObject _segment;
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

            _segment.transform.position = Vector3.Lerp(_previous_point.transform.position, _next_point.transform.position, (dist_percentage / 100));// previous_point.transform.position + (distance * (dist_percentage / 100));
        }
        
        
        
    }

}
