using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class growthIncrementer : MonoBehaviour
{
    public static growthIncrementer current;

    public List<cropGrowth> crops;
    [SerializeField]
    private int _current_crop;

    public float wait_time;

    public float crop_size_multiplier; //the fraction that crops are multiplied by on the output of the harvester. instead of every 6 of 10 crops dying, all crops are outputted at 6/10 of the size they should be.
    public float growth_rate_world_extra; // for controlling how much EVERY crop grows extra in speed(multiply the crop speed by this number). this is for if you wanna like.. code fertilizer to be deployed everwhere at once or something.
    public float max_growth_world_extra; //same as the above one, but for the amount of extra plant per plant i guess.
    
    private void Awake()
    {
        current = this;
    }

    
    void Start()
    {
        growthIncrement();
    }



    public void startGrowthIncrement()
    {
        //this is because you appearently cant start a coroutine from another script.
        StartCoroutine(growthIncrement());
    }
    [ContextMenu("growthIncrement")]
    public IEnumerator growthIncrement()
    {
        

        if (crops.Count > 0)
        {
            yield return new WaitForSeconds(wait_time); //this one is here so that it doesnt grow at light speed if there's just 1 crop
            foreach (cropGrowth crop in crops)
            {
                //Debug.Log("do crop");
                crop.step_growth();
                current_crop++;
                yield return new WaitForSeconds(wait_time);
            }
        }
        else
        {
            yield break; // if there are no crops, stop before this explodes.
        }
    }

    [ContextMenu("cycle_crop_list")]
    public void cycle_crop_list()
    {
        crops.Clear();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("growing_crop"))
        {
            crops.Add(obj.GetComponent<cropGrowth>());
        }
    }

    public int current_crop
    {
        get
        {
            return _current_crop;
        }
        
        set
        {
            _current_crop = value;


            if (crops.Count > 0)
            {
                if (current_crop >= crops.Count)
                {
                    StopAllCoroutines();//this is mainly to shut up the 'collection was modified, may not execute' error. cant check a list that's been modified if you arent working ;p
                    //Debug.Log("reached the end of the list, beginning restart process");
                    cycle_crop_list();
                    current_crop = 0;
                    StartCoroutine(growthIncrement());
                    //if you have incremented through all of the crops, restart the loop
                }
            }
        }
        
    }
}
