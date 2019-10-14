using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeSection : MonoBehaviour
{
    public int m_Num;
    public float m_RandomScale = 0.1f;
    
    

    // Start is called before the first frame update
    void Awake()
    {
        Material mat = new Material(gameObject.GetComponent<Renderer>().sharedMaterial);

        Texture2D t = Resources.Load<Texture2D>(m_Num.ToString());
        Debug.Log(t);
        mat.SetTexture("_MainTex", t);

        gameObject.GetComponent<Renderer>().sharedMaterial = mat;


        var mesh = GetComponent<MeshFilter>().mesh;

     
    }

    public void SetTexture(Texture2D tex,Vector2 scale)
    {
        gameObject.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", tex);
        gameObject.GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", scale);
    }

    // Update is called once per frame
    void Update()
    {
        var mesh = GetComponent<MeshFilter>().mesh;
        if (Input.GetKeyDown(KeyCode.R))
        {
            RandomVertex();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TopologyShow();
        }
    }

    void RandomVertex()
    {
        var mesh = GetComponent<MeshFilter>().mesh;
        var vertices = new Vector3[mesh.vertexCount];
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            vertices[i] = mesh.vertices[i] + Random.insideUnitSphere * m_RandomScale;
        }
        mesh.vertices = vertices;

    }
    void TopologyShow()
    {
        var mesh = GetComponent<MeshFilter>().mesh;

        var indices = new int[mesh.vertexCount];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = i;
        }

        mesh.SetIndices(indices, MeshTopology.Points, 0);
        mesh.RecalculateBounds();
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
