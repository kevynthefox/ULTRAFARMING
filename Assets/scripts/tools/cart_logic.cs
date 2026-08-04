using System;
using System.Collections.Generic;
using UnityEngine;

public class cart_logic : MonoBehaviour
{
    public GameObject lid;
    public List<GameObject> items;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("sellable"))
        {
            if (other.TryGetComponent(out product_data_holder data_holder))
            {
                if (data_holder.being_held == false)
                {
                    if (other.TryGetComponent(out Rigidbody rb))
                    {
                        rb.isKinematic = true;
                        //Destroy(rb);
                    }
                    if (!items.Contains(other.gameObject)) items.Add(other.gameObject);
                    other.transform.parent = transform;
                }
            }
        }
    }

    /*public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("sellable"))
        {
            if (other.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
            }
        }
    }*/

    public void toggle_lid()
    {
        Debug.Log("toggling lid");
        lid.SetActive(!lid.activeSelf);
        if (lid.activeSelf)
        {
            foreach (GameObject g in items)
            {
                g.TryGetComponent(out Rigidbody rb);
                Destroy(rb);

                float randX = UnityEngine.Random.Range(-1.5f, 1.5f);
                float randZ = UnityEngine.Random.Range(-5.5f, 5.5f);
                
                g.transform.localPosition = new Vector3(randX,2.25f,randZ);
                g.transform.localEulerAngles = Vector3.zero;
            }
        }
        else
        {
            foreach (GameObject g in items)
            {
                g.AddComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    public void discharge_cargo()
    {
        Debug.Log("discharging cargo");
        foreach (GameObject g in items)
        {
            g.transform.localPosition = new Vector3(g.transform.localPosition.x, -4, g.transform.localPosition.z);
            g.transform.parent = null;
            if (g.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
            }
            else
            {
                g.AddComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
