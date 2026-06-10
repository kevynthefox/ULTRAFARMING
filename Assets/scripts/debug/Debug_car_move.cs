using UnityEngine;

public class Debug_car_move : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public float speed;
    void Start()
    {
        this.GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
