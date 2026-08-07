using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using statusEffects;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class cropGrowth : MonoBehaviour
{
    [Header("Growth data")]
    public int maxGrowth;
    public float growth_rate;
    public float excess_growth;
    public int excess_segments;
    public bool started_growth;
    
    public bool ground_type; //this is so it only checks if the crop is in the ground if the crop is supposed to go in the ground. ie: not a vine

    public int growth_mult_sign; //when positive, things are added, when negative, things are subtracted. ie "++" becomes "--" essentially.
    
    
    [Header("sections")]
    public Vector3 offset; //this is so that the bottom segment isnt floating
    public GameObject bottom_section;
    public GameObject middle_section;
    public GameObject top_section;
    

    [Header("segments")]
    public List<GameObject> segments;
    public int current_segment;
    public List<cropSegment> segments_data;
    public float current_dist_percentage;
    public float future_dist_percentage;
    public GameObject harvested_part;
    public crop_data cropData;
    public int number_of_harvestable_parts;
    public int number_of_harvestable_parts_per_segment;

    [Header("points")]
    public GameObject next_point;
    public GameObject previous_point;
    public Quaternion procedural_rotation;

    [Header("value")]
    public float local_sizeMultiplier;
    public float base_price;
    
    [Header("perk related info")]
    public bool manual_or_thrown;
    public int hydration_count;
    public Material crop_material;
    public int defaultR, defaultG, defaultB, defaultA;
    //public Color default_color;
    public float hydration_scale;
    public float base_growth_rate;
    
    [Header("progress bar")]
    public TextMeshProUGUI text_part;
    public TextMeshProUGUI text_dev_note;
    public Image progress_bar;
    public Canvas canvas;
    
    
    void Awake()
    {
        harvested_part = cropData.cropPrefab;
        crop_material = new Material(crop_material);//this makes you able to dehydrate only 1 crop at a time, by making each one have a 'seperate material' :3  
        this.GetComponent<Renderer>().material = crop_material;
    }

    public IEnumerator Start_growing()
    {
        started_growth = true;

        growth_rate *= growthIncrementer.current.growth_rate_world_extra;
        maxGrowth = Mathf.RoundToInt(maxGrowth * growthIncrementer.current.max_growth_world_extra);

        if (ground_type == true)
        {
            if (transform.position.y <= 24.5)
            {
                local_sizeMultiplier = -0.1f;
            }
            else
            {
                local_sizeMultiplier = -0.2f;
            }
        }
        

        if (maxGrowth / 60 > 0.2f)
        {
            canvas.gameObject.SetActive(true);
            if (maxGrowth >= 1000)
            {
                text_dev_note.text = "plant booting <br> <br> this is your fault. i hope you are happy";
            }
        }

        for (int i = 0; i < maxGrowth; i++)
        {
            if (i == 0)
            {
                var bottom = Instantiate(bottom_section, transform.position + offset, transform.rotation);
                next_point = bottom.transform.Find("next_point").gameObject;

                bottom.transform.Rotate(90, 0, 0); // = new quaternion(90,0,0,0);
                bottom.transform.parent = this.transform;

                previous_point = this.gameObject;
                add_segment_to_crop_segments(i, bottom);
                segments.Add(bottom);
                previous_point = bottom;
            }
            else
            {
                if (i == maxGrowth - 1)
                {
                    var top = Instantiate(top_section, transform.position, transform.rotation);
                    segments.Add(top);

                    top.transform.parent =
                        previous_point
                            .transform; //this makes it so all segments above the previous one move up at once, so that the whole plant can actually grow instead of collecting in one spot
                    //top.transform.Rotate(90f, transform.rotation.y + (i * 90f),0);

                    add_segment_to_crop_segments(i, top);
                    next_point = null;


                }
                else
                {
                    procedural_rotation = new Quaternion(90f, transform.rotation.y + (i * 90f),
                        transform.rotation.z, transform.rotation.w);

                    var middle = Instantiate(middle_section, previous_point.transform.position,
                        quaternion.identity); //transform.rotation ); //the i * 90 is to make it turn more each segment
                    next_point = middle.transform.Find("next_point").gameObject;


                    middle.transform.parent =
                        previous_point
                            .transform; //this makes it so all segments above the previous one move up at once, so that the whole plant can actually grow instead of collecting in one spot
                    //middle.transform.Rotate(90,0,0 + (i*90));
                    //Debug.Log("rotation of " + i.ToString() + ": " + middle.transform.localEulerAngles);



                    add_segment_to_crop_segments(i, middle);
                    previous_point = middle;
                    segments.Add(middle);


                }
            }

            if (i != 0) segments[i].SetActive(false);

            if (maxGrowth / 60 > 0.2f)
            {
                //Debug.Log(segments.Count / maxGrowth);
                progress_bar.fillAmount = (float)segments.Count / maxGrowth;
                text_part.text = segments.Count + "/" + maxGrowth;

                if (frame_display.current.frameRate <= 15)
                {
                    text_dev_note.text =
                        "uh you should probably pull this thing <br> <br> out of the ground before your computer explodes..";
                }
                
                if (CompareTag("finished_crop"))
                {
                    text_dev_note.text =
                        "thanks. uh you should probably not make something <br> <br> this large again.. im gonna delete this now for you.";
                    yield return new WaitForSeconds(20);
                    Destroy(this.gameObject);
                    
                    yield break;
                }
            }

            yield return new WaitForEndOfFrame();
        }





        if (growthIncrementer.current.crops.Count == 0)
        {
            growthIncrementer.current.crops.Add(this);
            growthIncrementer.current.startGrowthIncrement();
            //if this is the first crop, start the growth incrementer
        }
        
        if (maxGrowth / 60 > 0.2f)
        {
            canvas.gameObject.SetActive(false);
        }
        
        tag = "growing_crop";
    }

    //public quaternion rotaty;
    public void step_growth()
    {
        //Debug.Log("step_growth");
        if (current_segment <= maxGrowth - 1)
        {
            if (current_segment >= 1)
            {
                if (segments[current_segment].activeSelf == false)
                {
                    segments[current_segment].SetActive(true);
                    segments[current_segment].transform.localPosition = new Vector3(0, 0, -4);
                    segments[current_segment].transform.localScale = new Vector3(1, 1, 1);
                    segments[current_segment].transform.localRotation = /*rotaty;*/ new quaternion(0, 0, 1, 1);
                    //if (current_segment == 80) segments[current_segment].transform.localRotation = /*new Quaternion(1,1,0,0);*/ rotaty;
                    /*if (current_segment == segments.Count-1)
                    {
                        Debug.Log("on the last segment");
                        segments[current_segment].transform.localRotation = new Quaternion(1,1,0,0);
                    } */
                    segments[current_segment].name = ("segment: " + current_segment);
                }
            }

            current_dist_percentage = segments_data[current_segment].dist_percentage;

            segments_data[current_segment].dist_percentage += (growth_rate + excess_growth);

            if (current_dist_percentage >= 1.00f)
            {
                //Debug.Log("current segment: " + current_segment + " segments.count: " + segments.Count);
                if (current_segment >=
                    segments.Count -
                    1) // the -1 is here because segment count has to start at 0, and segments.count counts 0 as one of the counted
                {
                    //Debug.Log("finished growing, removing from list");
                    //growthIncrementer.current.crops.Remove(this);
                    this.tag = "finished_crop";
                    //adjust_collider_size();
                }
                else
                {


                    if (current_dist_percentage > 1)
                    {
                        excess_growth = current_dist_percentage - 1;
                        if (excess_growth < 0.001)
                        {
                            excess_growth =
                                0; //this is to avoid the slow buildup of excess growth due to floating point errors
                        }

                        if (excess_growth >= 1)
                        {
                            excess_segments = Mathf.FloorToInt(excess_growth);

                            if (excess_segments % 2 == 1)
                                step_growth_segments_even(); //checks if its odd, then sends to the even one
                            if (excess_segments % 2 == 0)
                                step_growth_segments_odd(); //checks if its even, then sends it to the odd one


                            //current_segment += excess_segments;

                            excess_growth -= excess_segments;
                            excess_segments = 0;
                        }
                    }

                    if (current_segment + 1 <= maxGrowth - 1)
                    {
                        segments_data[current_segment + 1]._next_point =
                            segments_data[current_segment]._next_point_obj.transform.position;
                    }


                    check_off_segment(); //after current segment has finished growing, tick the next segment
                }

            }
        }

    }
    
    public void step_growth_segments_odd()
    {
        for (int i = 0; i < excess_segments; i++)
        {
            if (current_segment <= maxGrowth - 1)
            {
                //Debug.Log("step_segments " + excess_segments);
                if (current_segment >= 1)
                {
                    if (segments[current_segment].activeSelf == false)
                    {
                        segments[current_segment].SetActive(true);
                        segments[current_segment].transform.localPosition = new Vector3(0, 0, -4);
                        segments[current_segment].transform.localScale = new Vector3(1, 1, 1);
                        segments[current_segment].transform.localRotation = /*rotaty;*/ new quaternion(0, 0, 1, 1);
                        //if (current_segment == 80) segments[current_segment].transform.localRotation = /*new Quaternion(1,1,0,0);*/ rotaty;
                        /*if (current_segment == segments.Count-1)
                        {
                            Debug.Log("on the last segment");
                            segments[current_segment].transform.localRotation = new Quaternion(1,1,0,0);
                        } */
                        segments[current_segment].name = ("segment: " + current_segment);
                    }
                }

                current_dist_percentage = segments_data[current_segment].dist_percentage;
                //future_dist_percentage = current_dist_percentage + (growth_rate + excess_growth);



                segments_data[current_segment].dist_percentage += 1;

                if (current_dist_percentage >= 1.00f)
                {
                    //Debug.Log("current segment: " + current_segment + " segments.count: " + segments.Count);
                    if (current_segment >=
                        segments.Count -
                        1) // the -1 is here because segment count has to start at 0, and segments.count counts 0 as one of the counted
                    {
                        //Debug.Log("finished growing, removing from list");
                        //growthIncrementer.current.crops.Remove(this);
                        this.tag = "finished_crop";
                        //adjust_collider_size();
                    }
                    else
                    {

                        if (current_segment + 1 <= maxGrowth - 1)
                        {
                            segments_data[current_segment + 1]._next_point =
                                segments_data[current_segment]._next_point_obj.transform.position;
                        }

                        check_off_segment(); //after current segment has finished growing, tick the next segment
                    }

                }

            }
        }
    }
    public void step_growth_segments_even()
    {
        for (int i = 0; i <= excess_segments; i++)
        {
            Debug.Log("step_segments " + excess_segments);
            if (current_segment <= maxGrowth - 1)
            {
                if (current_segment >= 1)
                {
                    if (segments[current_segment].activeSelf == false)
                    {
                        segments[current_segment].SetActive(true);
                        segments[current_segment].transform.localPosition = new Vector3(0, 0, -4);
                        segments[current_segment].transform.localScale = new Vector3(1, 1, 1);
                        segments[current_segment].transform.localRotation = /*rotaty;*/ new quaternion(0, 0, 1, 1);
                        //if (current_segment == 80) segments[current_segment].transform.localRotation = /*new Quaternion(1,1,0,0);*/ rotaty;
                        /*if (current_segment == segments.Count-1)
                        {
                            Debug.Log("on the last segment");
                            segments[current_segment].transform.localRotation = new Quaternion(1,1,0,0);
                        } */
                        segments[current_segment].name = ("segment: " + current_segment);
                    }
                }

                current_dist_percentage = segments_data[current_segment].dist_percentage;
                //future_dist_percentage = current_dist_percentage + (growth_rate + excess_growth);



                segments_data[current_segment].dist_percentage += 1;

                if (current_dist_percentage >= 1.00f)
                {
                    //Debug.Log("current segment: " + current_segment + " segments.count: " + segments.Count);
                    if (current_segment >=
                        segments.Count -
                        1) // the -1 is here because segment count has to start at 0, and segments.count counts 0 as one of the counted
                    {
                        //Debug.Log("finished growing, removing from list");
                        //growthIncrementer.current.crops.Remove(this);
                        this.tag = "finished_crop";
                        //adjust_collider_size();
                    }
                    else
                    {

                        if (current_segment + 1 <= maxGrowth - 1)
                        {
                            segments_data[current_segment + 1]._next_point =
                                segments_data[current_segment]._next_point_obj.transform.position;
                        }

                        check_off_segment(); //after current segment has finished growing, tick the next segment
                    }

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
    
    public void check_off_segment()
    {
        
        
        current_segment++; //its not current segment +1 below because the first segment has no crop on itZz
        number_of_harvestable_parts = (current_segment) * number_of_harvestable_parts_per_segment;
        
        //adjust_collider_size();
    }

    /*public void adjust_collider_size()
    {
        x_list.Clear();
        y_list.Clear();
        z_list.Clear();
        foreach (GameObject segment in segments)
        {
            x_list.Add(segment.transform.position.x);
            y_list.Add(segment.transform.position.y);
            z_list.Add(segment.transform.position.z);
        }
        
        /*if (this.CompareTag("finished_crop"))
        {
            //Vector3 bet = Vector3.Distance(segments[0].transform.position, segments.Last().transform.position);
            average_distance_between_segments.x = x_list[1] - x_list[0];
            
            
            average_distance_between_segments.y = y_list[1] - y_list[0];
            
            
            average_distance_between_segments.z = z_list[1] - z_list[0];

            float temp_x = x_list.Last();
            x_list.Add(temp_x + average_distance_between_segments.x);
            
            float temp_y = y_list.Last();
            y_list.Add(temp_y + average_distance_between_segments.y);
            Debug.Log("temp y: " + temp_y);
            Debug.Log("average distance between segments: " + average_distance_between_segments.y);
            
            float temp_z = z_list.Last();
            z_list.Add(temp_z + average_distance_between_segments.z);
        }//

        max_x = Mathf.Max(x_list.ToArray());
        max_y = Mathf.Max(y_list.ToArray());
        max_z = Mathf.Max(z_list.ToArray());
        
        min_x = Mathf.Min(x_list.ToArray());
        min_y = Mathf.Min(y_list.ToArray());
        min_z = Mathf.Min(z_list.ToArray());
        
        
        
        average_x = (max_x - min_x)/2;
        average_y = (max_y - min_y)/2;
        average_z = (max_z - min_z)/2;
        
        
        
        collider.center = new Vector3(average_x, average_y, average_z);
        
        if (max_x - min_x <= 1)
        {
            average_x = 0.5f;
        }
        if (max_y - min_y <= 1)
        {
            average_y = 0.5f;
        }
        if (max_z - min_z <= 1)
        {
            average_z = 0.5f;
        }
        //collider.size = new Vector3(average_x * 2, average_y * 2, average_z * 2);

        Vector3 min = new Vector3(min_x, min_y, min_z);
        Vector3 max = new Vector3(max_x, max_y, max_z);
        
        collider.bounds.SetMinMax(min,max);
    }*/

    private void OnDestroy()
    {
        if (growthIncrementer.current != null) // this is to make it not have an error every time the game is paused in the inspector
        {
            growthIncrementer.current.crops.Remove(this);
            growthIncrementer.current.StopCoroutine(growthIncrementer.current.growthIncrement());
            growthIncrementer.current.StartCoroutine(growthIncrementer.current.growthIncrement());
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        

        if (other.gameObject.layer == 7) //7 is the layer for ground
        {

            if (manual_or_thrown)
            {
                if (perk_logic.current.seed_bag_customization == 0 || perk_logic.current.seed_bag_customization == 2)
                {


                }
                else
                {
                    if (perk_logic.current.seed_bag_customization == 1)
                    {
                        var water_ball = Instantiate(perk_logic.current.water_ball_prefab, transform.position, transform.rotation);

                        if (StatusEffectAdder.current.player.TryGetComponent(out wet_buff wet))
                        {
                            water_ball.transform.localScale = Vector3.one * math.pow(wet.water_element_multiplier,  wet.stack_count);
                        }
                    
                        Destroy(this.gameObject);
                    }
                    
                    if (perk_logic.current.seed_bag_customization == 3)
                    {
                        var fire_ball = Instantiate(perk_logic.current.fire_ball_prefab, transform.position, transform.rotation);

                        if (StatusEffectAdder.current.player.TryGetComponent(out fire_buff fire))
                        {
                            fire_ball.transform.localScale = Vector3.one * math.pow(fire.fire_element_multiplier,  fire.stack_count);
                        }
                    
                        Destroy(this.gameObject);
                    }
                }
            }
            else
            {
                if (perk_logic.current.perk10)
                {
                    if (StatusEffectAdder.current.player.TryGetComponent(out dirty_buff dirty))
                    {
                        number_of_harvestable_parts_per_segment = Mathf.RoundToInt(number_of_harvestable_parts_per_segment * Mathf.Pow(dirty.earth_element_multiplier,dirty.stack_count));
                    }
                    else
                    {
                        number_of_harvestable_parts_per_segment *= 2;
                    }
                }
            }
            
            if (TryGetComponent(out Rigidbody rb))
            {
                if (rb.useGravity == true) // this part is here so you cant plant while holding
                {
                    Destroy(rb);


                    if (TryGetComponent(out MeshRenderer mr))
                    {
                        mr.enabled = false;
                    }

                    if (started_growth == false)
                    {

                        //Physics.Raycast(this.transform.position,Vector3.down,out RaycastHit hit);
                        transform.rotation = quaternion.identity; // = Quaternion.LookRotation(hit.transform.up);
                        //honestly.. like.. why would it ever need to be sideways? this is a perfectly flat plane
                        
                        StartCoroutine(Start_growing());
                    }
                }
            }
        }
        
        
    }

    public void hydrate(int hydration_change)//use negative numbers to rehydrate
    {
        hydration_count += hydration_change;
        hydration_scale = Mathf.Pow(2, hydration_count);
        Color new_color = new Vector4(defaultR * hydration_scale,defaultG * hydration_scale, defaultB * hydration_scale, defaultA * hydration_scale);
        crop_material.SetColor("_BaseColor",new_color);
        growth_rate = base_growth_rate * hydration_scale;
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


