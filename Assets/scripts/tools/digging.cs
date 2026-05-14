using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class digging : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer;
    [SerializeField] Camera cam;
    [SerializeField] Transform player;

    [SerializeField] float playerReach;
    [SerializeField] float miningEffectivity;
    [SerializeField] float miningRange;

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hitInfo, playerReach, terrainLayer))
        { 
            TerraformTerrain(hitInfo.point, -miningEffectivity, miningRange);
            //Debug.Log("decreasing terrain at:" + hitInfo.point);
        }

        if (Input.GetMouseButton(1) && Physics.Raycast(ray, out hitInfo, playerReach, terrainLayer))
        {
            TerraformTerrain(hitInfo.point, miningEffectivity, miningRange);
            //Debug.Log("increasing terrain at:" + hitInfo.point);
        }
    }

    public Mesh mesh;
    public Vector3[] vertices;
    private void TerraformTerrain(Vector3 position, float height, float range)
    {
        mesh = meshFilter.sharedMesh;
        vertices = mesh.vertices;
        position -= meshFilter.transform.position;

        int i = 0;
        foreach (Vector3 vert in vertices)
        {
            if (Vector2.Distance(new Vector2(vert.x, vert.z), new Vector2(position.x, position.z)) <= range)
            {
                vertices[i] = vert + new Vector3(0, height, 0);
            }
            i++;
        }

        mesh.vertices = vertices;
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
}