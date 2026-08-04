using System;
using UnityEngine;

public class world_faller : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rb))
        {
            other.transform.position = new Vector3(0, 30, 0);

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

}
