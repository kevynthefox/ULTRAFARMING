using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class expand_land : MonoBehaviour
{

    public float expansion_cost;
    public int current_expansion_level;

    public money_holder moneyHolder;
    
    public InfiniteTerrain infinite_terrain;

    public TextMeshProUGUI cost_display;

    public Rigidbody rb;
    
    public void expand()
    {
        if (moneyHolder.money >= expansion_cost)
        {
            moneyHolder.money_update(-expansion_cost);

            infinite_terrain.worldSettings.renderDistance++;

            expansion_cost += (
                ((infinite_terrain.worldSettings.renderDistance+1) * (infinite_terrain.worldSettings.renderDistance+1))
                -
                ((infinite_terrain.worldSettings.renderDistance) * (infinite_terrain.worldSettings.renderDistance))
            );
            current_expansion_level++;
            cost_display.text = "$" + expansion_cost + "<br> expansion level: " + (current_expansion_level+1) +"x"+(current_expansion_level+1);



            //this.AddComponent<Rigidbody>();
            //rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.linearVelocity = new Vector3(infinite_terrain.worldSettings.chunkSize*current_expansion_level, 0, infinite_terrain.worldSettings.chunkSize*current_expansion_level);

            //infinite_terrain.ShutDown();
            infinite_terrain.OnStart();

            StartCoroutine(turn_off_velocity());
            //transform.position =  new Vector3(0, 26, 0);
        }
    }

    public IEnumerator turn_off_velocity()
    {
        yield return new WaitForSeconds(3f);
        rb.linearVelocity = rb.linearVelocity =
            new Vector3(-infinite_terrain.worldSettings.chunkSize * current_expansion_level, 0,
                -infinite_terrain.worldSettings.chunkSize * current_expansion_level);
        yield return new WaitForSeconds(3);
        rb.isKinematic = true;
        transform.position = new Vector3(0, 26, 0);
        transform.rotation = Quaternion.identity;
    }

}
