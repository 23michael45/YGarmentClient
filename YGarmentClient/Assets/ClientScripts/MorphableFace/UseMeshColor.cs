using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseMeshColor : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        Mesh mesh = filter.mesh;

        Color[] colors = new Color[mesh.vertexCount];
        
        for(int i = 0; i< colors.Length;i++)
        {
            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);
            colors[i] = new Color(r,g,b);

            Debug.Log(string.Format("{0} {1} {2}", r, g, b));
            Debug.Log(colors[i]);
        }
        
        mesh.colors = colors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
