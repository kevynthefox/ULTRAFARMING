using System;
using System.Collections;
using UnityEngine;

//from https://catlikecoding.com/unity/tutorials/mesh-deformation/
public class MeshDeformerInput : MonoBehaviour {

    public float force = 10f;

    public float max_distance;
    public LayerMask layerMask;
    
    public float forceOffset = 0.1f;

    
    //public void OnCollisionEnter(Collision other)
    private void OnCollisionEnter(Collision other)
    {
        HandleInput();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, max_distance, Vector3.forward, out hit, max_distance, layerMask))
        {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            
            if (deformer) {
                Vector3 point = hit.point;
                point += hit.normal * forceOffset;
                deformer.AddDeformingForce(point, force,this.transform);
            }
            
        }

    }
    
    bool IsHeadingForCollision() {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, max_distance, Vector3.forward, out hit, layerMask)) {
            return true;
        } 
        else 
        {
            return false;
        }
    }
}