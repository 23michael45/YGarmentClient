using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDataPrint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var vertices = gameObject.GetComponent<MeshFilter>().mesh.vertices;
        for(int i = 0; i< vertices.Length;i++)
        {
            Debug.Log(vertices[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
