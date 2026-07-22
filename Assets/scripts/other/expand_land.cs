using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class expand_land : MonoBehaviour
{

    public float expansion_cost;
    public int current_expansion_level;

    public money_holder moneyHolder;

    public TextMeshProUGUI cost_display;

    public InfiniteTerrain terrain;
    public int chunkSize;
    
    public GameObject border_1, border_2,border_3,border_4;
    public GameObject border_prefab;

    public void Start()
    {
        chunkSize = terrain.worldSettings.chunkSize;
    }

    public void expand()
    {
        if (moneyHolder.money >= expansion_cost)
        {
            moneyHolder.money_update(-expansion_cost);

            current_expansion_level++;
            
            expansion_cost = (
                math.square(current_expansion_level+1) - math.square(current_expansion_level)
            );
            cost_display.text = "$" + expansion_cost + "<br> expansion level: " + (current_expansion_level+1) +"x"+(current_expansion_level+1);

            
            
            //border_1.transform.localScale = new Vector3(current_expansion_level * chunkSize, 1, 1);
            //border_2.transform.localScale = new Vector3(1, 1, current_expansion_level * chunkSize);
            //border_3.transform.localScale = new Vector3(current_expansion_level * chunkSize, 1, 1);
            //border_4.transform.localScale = new Vector3(1, 1, current_expansion_level * chunkSize);

            GameObject new_border_1 = Instantiate(border_prefab,Vector3.zero,quaternion.identity);
            GameObject new_border_2 = Instantiate(border_prefab,Vector3.zero,quaternion.identity);
            GameObject new_border_3 = Instantiate(border_prefab,Vector3.zero,quaternion.identity);
            GameObject new_border_4 = Instantiate(border_prefab,Vector3.zero,quaternion.identity);
            
            new_border_1.transform.localEulerAngles = new Vector3(90,90,0);
            new_border_2.transform.localEulerAngles = new Vector3(90,0,0);
            new_border_3.transform.localEulerAngles = new Vector3(90,90,0);
            new_border_4.transform.localEulerAngles = new Vector3(90,0,0);

            new_border_1.transform.parent = border_1.transform;
            new_border_2.transform.parent = border_2.transform;
            new_border_3.transform.parent = border_3.transform;
            new_border_4.transform.parent = border_4.transform;

            new_border_1.transform.localPosition = new Vector3(0,(current_expansion_level-1) * 80,0);
            new_border_2.transform.localPosition = new Vector3(0,(current_expansion_level-1) * 80,0);
            new_border_3.transform.localPosition = new Vector3(0,(current_expansion_level-1) * 80,0);
            new_border_4.transform.localPosition = new Vector3(0,(current_expansion_level-1) * 80,0);
            
            border_1.transform.position = new Vector3(17, 26, 2+(current_expansion_level * chunkSize));
            border_2.transform.position = new Vector3(2+(current_expansion_level * chunkSize), 26, 17);
            //border_3.transform.position = new Vector3(18, 26, 1);
            //border_4.transform.position = new Vector3(1, 26, 18);
        }
    }

}
