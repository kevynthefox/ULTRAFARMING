using UnityEngine;

public class Move_down_terrain : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y -41.5f, this.transform.position.z);
    }

    
}
