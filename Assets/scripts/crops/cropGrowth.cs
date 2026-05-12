using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class cropGrowth : MonoBehaviour
{
    [Header("Growth data")]
    public int maxGrowth;
    public float growth_rate;
    public float excess_growth;
    public int excess_segments;
    
    [Header("sections")]
    public GameObject bottom_section;
    public GameObject middle_section;
    public GameObject top_section;

    [Header("segments")]
    public List<GameObject> segments;
    public int current_segment;
    public List<cropSegment> segments_data;
    public float current_dist_percentage;
    public float future_dist_percentage;

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
                
                bottom.transform.Rotate(90,0,0);// = new quaternion(90,0,0,0);
                bottom.transform.parent = this.transform;
                
                previous_point = this.gameObject;
                add_segment_to_crop_segments(i,bottom);
                segments.Add(bottom);
                previous_point = bottom;
            }
            else
            {
                if (i == maxGrowth - 1)
                {
                    var top = Instantiate(top_section, transform.position, transform.rotation);
                    segments.Add(top);
                    
                    top.transform.parent = previous_point.transform;//this makes it so all segments above the previous one move up at once, so that the whole plant can actually grow instead of collecting in one spot
                    top.transform.Rotate(90f, transform.rotation.y + (i * 90f),0);
                    
                    add_segment_to_crop_segments(i,top);
                    next_point = null;
                    
                    
                }
                else
                {
                    procedural_rotation = new Quaternion(90f , transform.rotation.y + (i * 90f),
                        transform.rotation.z , transform.rotation.w);
            
                    var middle = Instantiate(middle_section,previous_point.transform.position,transform.rotation ); //the i * 90 is to make it turn more each segment
                    next_point = middle.transform.Find("next_point").gameObject;
                    
                    middle.transform.parent = previous_point.transform;//this makes it so all segments above the previous one move up at once, so that the whole plant can actually grow instead of collecting in one spot
                    middle.transform.Rotate(90,transform.rotation.y + (i*90),0);
                    
                    add_segment_to_crop_segments(i,middle);
                    previous_point = middle;
                    segments.Add(middle);
                    
                    
                }
            }
           

            
        }

        foreach (var segment in segments)
        { 
            
            //add a thing to add it to the segments data list thingy here
            if (segment != segments.First())
            {
                segment.SetActive(false);   
            }
            //shrink them down until they're invisible so that they can grow, then turn them off to save processing power
        }
        
        growthIncrementer.current.crops.Add(this);
        if (growthIncrementer.current.crops.Count == 1)
        {
            growthIncrementer.current.startGrowthIncrement();
            //if this is the first crop, start the growth incrementer
        }
    }

    public void step_growth()
    {
        Debug.Log("step_growth");
        if (current_segment <= segments.Count && current_segment >= 1)
        {
            if (segments[current_segment].activeSelf == false)
            {
                segments[current_segment].SetActive(true);
                segments[current_segment].transform.localPosition = new Vector3(0, 0, -4);
                segments[current_segment].transform.localScale = new Vector3(1, 1, 1);
                segments[current_segment].name = ("segment: " + current_segment);
            }
        }

        current_dist_percentage = segments_data[current_segment].dist_percentage;
        
        segments_data[current_segment].dist_percentage += (growth_rate + excess_growth);

        if (current_dist_percentage >= 1.00f)
        {
            //Debug.Log("current segment: " + current_segment + " segments.count: " + segments.Count);
            if (current_segment >= segments.Count - 1) // the -1 is here because segment count has to start at 0, and segments.count counts 0 as one of the counted
            {
                //Debug.Log("finished growing, removing from list");
                //growthIncrementer.current.crops.Remove(this);
                this.tag = "finished_crop"; 
            }
            else
            {
                if (current_dist_percentage > 1)
                {
                    excess_growth = current_dist_percentage - 1;
                    if (excess_growth < 0.001)
                    {
                        excess_growth = 0; //this is to avoid the slow buildup of excess growth due to floating point errors
                    }

                    if (excess_growth >= 1)
                    {
                        excess_segments = Mathf.FloorToInt(excess_growth);

                        if (excess_segments % 2 == 1 ) step_growth_segments_even(); //checks if its odd, then sends to the even one
                        if (excess_segments % 2 == 0 ) step_growth_segments_odd(); //checks if its even, then sends it to the odd one

                        
                        //current_segment += excess_segments;
                        
                        excess_growth -= excess_segments;
                        excess_segments = 0;
                    }
                }
                segments_data[current_segment + 1]._next_point = segments_data[current_segment]._next_point_obj.transform.position;

                
                
                current_segment++;  //after current segment has finished growing, tick the next segment
            }
            
        }

        
    }
    
    public void step_growth_segments_odd()
    {
        for (int i = 0; i < excess_segments; i++)
        {
            Debug.Log("step_segments " + excess_segments);
            if (current_segment <= segments.Count && current_segment >= 1)
            {
                if (segments[current_segment].activeSelf == false)
                {
                    segments[current_segment].SetActive(true);
                    segments[current_segment].transform.localPosition = new Vector3(0, 0, -4);
                    segments[current_segment].transform.localScale = new Vector3(1, 1, 1);
                    segments[current_segment].name = ("segment: " + current_segment);
                }
            }

            current_dist_percentage = segments_data[current_segment].dist_percentage;
            //future_dist_percentage = current_dist_percentage + (growth_rate + excess_growth);

            

            segments_data[current_segment].dist_percentage += 1;

            if (current_dist_percentage >= 1.00f)
            {
                //Debug.Log("current segment: " + current_segment + " segments.count: " + segments.Count);
                if (current_segment >= segments.Count - 1) // the -1 is here because segment count has to start at 0, and segments.count counts 0 as one of the counted
                {
                    //Debug.Log("finished growing, removing from list");
                    //growthIncrementer.current.crops.Remove(this);
                    this.tag = "finished_crop";
                }
                else
                {
                    

                    segments_data[current_segment + 1]._next_point = segments_data[current_segment]._next_point_obj.transform.position;
                    current_segment++; //after current segment has finished growing, tick the next segment
                }

            }

        }
    }
    public void step_growth_segments_even()
    {
        for (int i = 0; i <= excess_segments; i++)
        {
            Debug.Log("step_segments " + excess_segments);
            if (current_segment <= segments.Count && current_segment >= 1)
            {
                if (segments[current_segment].activeSelf == false)
                {
                    segments[current_segment].SetActive(true);
                    segments[current_segment].transform.localPosition = new Vector3(0, 0, -4);
                    segments[current_segment].transform.localScale = new Vector3(1, 1, 1);
                    segments[current_segment].name = ("segment: " + current_segment);
                }
            }

            current_dist_percentage = segments_data[current_segment].dist_percentage;
            //future_dist_percentage = current_dist_percentage + (growth_rate + excess_growth);

            

            segments_data[current_segment].dist_percentage += 1;

            if (current_dist_percentage >= 1.00f)
            {
                //Debug.Log("current segment: " + current_segment + " segments.count: " + segments.Count);
                if (current_segment >= segments.Count - 1) // the -1 is here because segment count has to start at 0, and segments.count counts 0 as one of the counted
                {
                    //Debug.Log("finished growing, removing from list");
                    //growthIncrementer.current.crops.Remove(this);
                    this.tag = "finished_crop";
                }
                else
                {
                    

                    segments_data[current_segment + 1]._next_point = segments_data[current_segment]._next_point_obj.transform.position;
                    current_segment++; //after current segment has finished growing, tick the next segment
                }

            }

        }
    }

    private void add_segment_to_crop_segments(int i, GameObject segment)
    {
        //using insert because with that, you can insert a null, and then populate it. I think...
        segments_data.Insert(i , new cropSegment());
        segments_data[i]._segment = segment;
        segments_data[i]._next_point = next_point.transform.position;
        segments_data[i]._next_point_obj = next_point; //dont need to worry about getting these from the sepcific object since next point and previous point are global in this script. as in, when set by one object, they are set like that for all of them ig?
        segments_data[i]._previous_point = previous_point;
        //segments_data[i].dist_percentage = 0;
        segments_data[i].calculate_distance();
    }
}


[Serializable]
public class cropSegment
{
    
    public GameObject _segment;
    public Vector3 _next_point;
    public GameObject _next_point_obj;
    public GameObject _previous_point;

    [SerializeField]
    private float _dist_percentage;

    public float distance;


    public void calculate_distance()
    {
        distance = Vector3.Distance(_previous_point.transform.position,_next_point);
        dist_percentage = 0;
    }
    
    public float dist_percentage 
    {
        
        get
        {
            return _dist_percentage;
        }
        set
        {
            _dist_percentage = value;

            _segment.transform.position = Vector3.Lerp(_previous_point.transform.position, _next_point, dist_percentage );// previous_point.transform.position + (distance * (dist_percentage / 100));
        }
        
        
        
    }

}
