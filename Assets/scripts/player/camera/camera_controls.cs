using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class camera_controls : MonoBehaviour
{


    public Camera cam1;
    public Camera cam2;


    public GameObject player;
    public GameObject head;

    public bool first_or_third; //false is first, true is third.


    void Start()
    {
        cam1.enabled = true;
    	cam2.enabled = false;
    }

    

    private void Update()
    {
	    
	    if (Input.GetKeyDown(KeyCode.F))
	    {
		    cam1.enabled = !cam1.enabled;
		    cam2.enabled = !cam2.enabled;
		    
		    cam2.transform.position = head.transform.position;
		    cam2.transform.rotation = head.transform.rotation;

		    player.GetComponent<FirstPersonMovement>().enabled = cam1.enabled;
		    head.GetComponent<FirstPersonLook>().enabled = cam1.enabled;
		    
		    cam2.GetComponent<FreeFlyCamera>().enabled = cam2.enabled;

		    first_or_third = !first_or_third;
	    }
	    
    }
}
