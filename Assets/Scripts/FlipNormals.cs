using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Flip normals of an object*/
public class FlipNormals : MonoBehaviour
{

    public GameObject Sphere;


    void Start()
    {
        
    }

    void Awake()
    {
        InvertSphere();
    }

    void InvertSphere()
    {
        Vector3[] normals = Sphere.GetComponent<MeshFilter>().mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        Sphere.GetComponent<MeshFilter>().sharedMesh.normals = normals;

        int[] triangles = Sphere.GetComponent<MeshFilter>().sharedMesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int t = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = t;
        }

        Sphere.GetComponent<MeshFilter>().sharedMesh.triangles = triangles;
    }

    void Update()
    {
        Sphere.transform.Rotate(new Vector3 (1, 0, 0) * Time.deltaTime);
    }
}
