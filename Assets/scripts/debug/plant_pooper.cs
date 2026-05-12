using System;
using UnityEngine;

public class plant_pooper : MonoBehaviour
{
    public GameObject plant;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(plant, transform.position, transform.rotation);
            growthIncrementer.current.current_crop = growthIncrementer.current.crops.Count;
        }
    }
}
