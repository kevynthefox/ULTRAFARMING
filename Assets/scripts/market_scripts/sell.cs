using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class sell : MonoBehaviour
{
    public List<GameObject> products;
    public List<GameObject> products_to_remove;
    
    public int quantity;
    public int minimum; //minimum amount to buy
    public float sell_price;
    public List<int> max_products_per_type;
    public List<int> product_num_per_type;
    
    public rhythm_controller rhythm_game;

    public money_holder money_counter;
    public TextMeshProUGUI item_list;
    public string item_list_text;

    public int customers_served;

    
    
    public bool commit;
    public GameObject button_object;
    
    public void sell_product()
    {
        customers_served++;
        
        sell_price = 0;
        quantity = 0;
        

        if (perk_logic.current.perk11)
        {
            minimum = Mathf.RoundToInt(products.Count * (perk_logic.current.pet_rock.transform.localScale.x / 100));
            quantity = UnityEngine.Random.Range(minimum,products.Count);
        }
        else
        {
            minimum = 1;
            quantity = UnityEngine.Random.Range(minimum,products.Count);
        }
        
        for (int i = 0; i < quantity; i++)
        {
            if (products[i].TryGetComponent(out product_data_holder prod))
            {
                sell_price += (prod.base_value * prod.local_value_multiplier);
            }
            
            products_to_remove.Add(products[i]);
            //Debug.Log("current product amount: " + products.Count);
        }

        foreach (GameObject g in products_to_remove)
        {
            product_num_per_type[g.GetComponent<product_data_holder>().product_id]--;
            products.Remove(g);
            Destroy(g);
        }
        products_to_remove.Clear();

        sell_price *= rhythm_game.score;
        Debug.Log("sell price: " + sell_price);
        money_counter.money_update(sell_price);

        item_list_text = null;
        for (int i = 0; i < max_products_per_type.Count; i++)
        {
            item_list_text += i + ": " + product_num_per_type[i] + "/" +  max_products_per_type[i] + "<br>";
        }
        item_list.text = item_list_text;
    }

    public void OnTriggerStay(Collider other)
    {
        if (commit == true)
        {

            if (other.CompareTag("sellable"))
            {
                if (!products.Contains(other.gameObject))
                {
                    if (other.TryGetComponent(out product_data_holder prod))
                    {
                        if (product_num_per_type[prod.product_id] < max_products_per_type[prod.product_id])
                        {
                            if (other.transform.parent == null)
                            {
                                products.Add(other.gameObject);
                                product_num_per_type[prod.product_id]++;

                                item_list_text = null;
                                for (int i = 0; i < max_products_per_type.Count; i++)
                                {
                                    item_list_text += i + ": " + product_num_per_type[i] + "/" +
                                                      max_products_per_type[i] + "<br>";
                                }

                                item_list.text = item_list_text;
                            }
                        }
                    }
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("sellable"))
        {
            if (products.Contains(other.gameObject))
            {
                if (other.TryGetComponent(out product_data_holder prod))
                {
                
                
                    products.Remove(other.gameObject);
                    product_num_per_type[prod.product_id]--;
                    
                    item_list_text = null;
                    for (int i = 0; i < max_products_per_type.Count; i++)
                    {
                        item_list_text += i + ": " + product_num_per_type[i] + "/" +  max_products_per_type[i] + "<br>";
                    }
                    item_list.text = item_list_text;
                
                }
            }
        } 
        
    }

    public void toggle_commit()
    {
        commit = !commit;
        if (commit == true) button_object.transform.rotation = Quaternion.Euler(-45,0,0);
        if (commit == false) button_object.transform.rotation = Quaternion.Euler(0,0,0);
    }
}
