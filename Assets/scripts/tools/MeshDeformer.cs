using System.Collections;
using UnityEngine;


//from https://catlikecoding.com/unity/tutorials/mesh-deformation/
[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour 
{
    Mesh deformingMesh;
    Vector3[] originalVertices, displacedVertices;
    Vector3[] vertexVelocities;

	void Start ()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++) {
            displacedVertices[i] = originalVertices[i];
        }
	        
        vertexVelocities = new Vector3[originalVertices.Length];
    }
    
    public void AddDeformingForce (Vector3 point, float force,Transform deformerTransform) {
        Debug.DrawLine(deformerTransform.transform.position, point);
        
        for (int i = 0; i < displacedVertices.Length; i++) {
            AddForceToVertex(i, point, force);
        }
    }
    
    void AddForceToVertex (int i, Vector3 point, float force) {
        Vector3 pointToVertex = displacedVertices[i] - point;
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        vertexVelocities[i] += pointToVertex.normalized * velocity;

        StartCoroutine(update_());
    }

    public IEnumerator update_()
    {
        for (int o = 0; o < 120; o++)
        {
            for (int i = 0; i < displacedVertices.Length; i++)
            {
                UpdateVertex(i);
            }

            deformingMesh.vertices = displacedVertices;
            deformingMesh.RecalculateNormals();
            yield return new WaitForEndOfFrame(); 
        }
        yield break; //after the for loop has been processed, stop the function to save processing power?
    }

    void UpdateVertex (int i) {
        Vector3 velocity = vertexVelocities[i];
        displacedVertices[i] += velocity * Time.deltaTime;
    }
    
}
