using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int l = gameObject.GetComponent<MeshFilter>().mesh.uv.Length;
        Debug.Log("UV Len:" + l);


        for(int i = 0; i < Mathf.Min(20,l);i++)
        {
            Vector2 uv = gameObject.GetComponent<MeshFilter>().mesh.uv[i];
            Debug.Log(string.Format("UV {0} x : {1}  y : {2}" ,i, uv.x,uv.y));

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
