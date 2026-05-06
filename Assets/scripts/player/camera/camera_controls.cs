using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class camera_controls : MonoBehaviour
{

    public GameObject camera_holder;

    public Camera cam1;
    public Camera cam2;
    public Vector2 look;

    //public Transform first_person_target;
    //public Transform third_person_target;

    public GameObject player;

    public bool first_or_third; //false is first, true is third.

    //[SerializeField] private CinemachineCamera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam1.enabled = true;
    	cam2.enabled = false;
    }

    // Update is called once per frame
    /*void Update()
    {
	    if (Input.GetKeyDown(KeyCode.F))
	    {
            cam1.enabled = !cam1.enabled;
		    cam2.enabled = !cam2.enabled;
            cam2.GetComponent<FreeFlyCamera>().enabled = cam2.enabled;

            first_or_third = !first_or_third;
	    }

    }*/

    private void Update()
    {
	    
	    if (Input.GetKeyDown(KeyCode.F))
	    {
		    cam1.enabled = !cam1.enabled;
		    cam2.enabled = !cam2.enabled;
		    
		    cam2.transform.position = player.transform.position;
		    cam2.transform.rotation = player.transform.rotation;

		    player.GetComponent<FirstPersonMovement>().enabled = cam1.enabled;
		    
		    cam2.GetComponent<FreeFlyCamera>().enabled = cam2.enabled;

		    first_or_third = !first_or_third;
	    }
	    
    }
    

    void OnLook(InputValue value )
    {
        look = value.Get<Vector2>();
    }
}
