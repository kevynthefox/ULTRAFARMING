using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class sell : MonoBehaviour
{
    public List<GameObject> products;
    public List<GameObject> products_to_remove;
    
    public int quantity;
    public float sell_price;
    
    public rhythm_controller rhythm_game;

    public money_holder money_counter;
    
    public void sell_product()
    {
        sell_price = 0;
        quantity = 0;
        
        quantity = UnityEngine.Random.Range(1,products.Count);

        for (int i = 0; i < quantity; i++)
        {
            if (products[i].TryGetComponent(out cropGrowth crop))
            {
                sell_price += (crop.base_price * crop.local_sizeMultiplier);
            }
            
            products_to_remove.Add(products[i]);
            //Debug.Log("current product amount: " + products.Count);
        }

        foreach (GameObject g in products_to_remove)
        {
            products.Remove(g);
            Destroy(g);
        }
        products_to_remove.Clear();

        sell_price *= rhythm_game.score;
        Debug.Log("sell price: " + sell_price);
        money_counter.money_update(sell_price);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("sellable"))
        {
            if (!products.Contains(other.gameObject)) products.Add(other.gameObject);
        }
    }
}
