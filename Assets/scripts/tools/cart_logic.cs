using System;
using UnityEngine;

public class cart_logic : MonoBehaviour
{
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
                    }
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
}
