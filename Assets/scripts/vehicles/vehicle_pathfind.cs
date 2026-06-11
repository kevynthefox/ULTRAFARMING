using System;
using UnityEngine;

public class vehicle_pathfind : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;


    private void Start()
    {
        move_begin();
    }

    [ContextMenu("move_begin")]
    public void move_begin()
    {
        rb.linearVelocity = transform.forward * speed;
    }
    [ContextMenu("move_end")]
    public void move_end()
    {
        rb.linearVelocity = Vector3.zero; //should probably at some point replace this with a smooth transition to 0. same with the begin.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pathing_turn_right"))
        {
            Debug.Log("turn right");
            move_end();
            transform.position = new Vector3(other.transform.position.x,transform.position.y,other.transform.position.z);
            transform.Rotate((new Vector3(transform.rotation.x,transform.rotation.y + 90f, transform.rotation.z)));// = new Quaternion(transform.rotation.x, transform.rotation.y + 90f, transform.rotation.z,0);
            move_begin();
        }
        if (other.CompareTag("pathing_turn_left"))
        {
            Debug.Log("turn left");
            move_end();
            transform.position = new Vector3(other.transform.position.x,transform.position.y,other.transform.position.z);
            transform.Rotate((new Vector3(transform.rotation.x,transform.rotation.y - 90f, transform.rotation.z)));
            move_begin();
        }
    }
}
