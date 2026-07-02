using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class sell : MonoBehaviour
{
    public List<GameObject> products;
    
    public int quantity;
    public float sell_price;
    
    public rhythm_controller rhythm_game;

    public money_holder money_counter;
    
    public void sell_product()
    {
        quantity = UnityEngine.Random.Range(1,products.Count);

        for (int i = 0; i < quantity; i++)
        {
            if (products[i].TryGetComponent(out cropGrowth crop))
            {
                sell_price += (crop.base_price * crop.local_sizeMultiplier);
            }
            Destroy(products[i]);
            products.RemoveAt(i);
        }

        sell_price *= rhythm_game.score;
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
