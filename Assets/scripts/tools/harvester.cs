using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class harvester : MonoBehaviour
{
    public List<harvestable_part> parts;

    public bool harvesting;

    public Transform output_point;
    [SerializeField]
    private bool _outputting;

    public nav_pathfinding holder;
    public bool holder_is_person;
    public int carry;

    private void OnTriggerEnter(Collider other)
    {
        if (harvesting == true)
        {
            if (other.gameObject.CompareTag("growing_crop") || other.gameObject.CompareTag("finished_crop"))
            {
                parts.Add(new harvestable_part());
                parts.Last().part = other.gameObject.GetComponent<cropGrowth>().harvested_part;
                parts.Last().number_of_harvestable_parts = other.gameObject.GetComponent<cropGrowth>().number_of_harvestable_parts;
                
                Destroy(other.gameObject);

                if (holder_is_person == true)
                {
                    foreach (harvestable_part part in parts)
                    {
                        carry += part.number_of_harvestable_parts;
                    }

                    if (carry >= holder.carry_capacity)
                    {
                        holder.paused_destination = holder.destination;
                        holder.paused_destination_type = holder.destination_type;
                        holder.destination = holder.refiller;
                        holder.destination_type = 5;
                        this.gameObject.SetActive(false);
                    }
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    
    public bool outputting
    {
        get
        {
            return _outputting;
        }

        set
        {
            _outputting = value;
            
            output();
        }
    }

    [ContextMenu("output")]
    public void output()
    {
        foreach (var part in parts)
        {
            for (int i = 0; i < part.number_of_harvestable_parts; i++)
            {
                Instantiate(part.part, output_point.position, Quaternion.identity);
            }
        }
        parts.Clear();
        if (holder_is_person == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class harvestable_part
{
    public GameObject part;
    public int number_of_harvestable_parts;
}
