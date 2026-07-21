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
    
    public GameObject border_1, border_2,border_3,border_4;
    
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

            int chunkSize = terrain.worldSettings.chunkSize+1;
            
            border_1.transform.localScale = new Vector3(current_expansion_level * chunkSize, 1, 1);
            border_2.transform.localScale = new Vector3(1, 1, current_expansion_level * chunkSize);
            border_3.transform.localScale = new Vector3(current_expansion_level * chunkSize, 1, 1);
            border_4.transform.localScale = new Vector3(1, 1, current_expansion_level * chunkSize);
            
            border_1.transform.position = new Vector3((current_expansion_level * chunkSize) / 2, 26, current_expansion_level * chunkSize);
            border_2.transform.position = new Vector3(current_expansion_level * chunkSize, 26, (current_expansion_level * chunkSize) / 2);
            border_3.transform.position = new Vector3((current_expansion_level * chunkSize) / 2, 26, 1);
            border_4.transform.position = new Vector3(1, 26, (current_expansion_level * chunkSize) / 2);
        }
    }

}
