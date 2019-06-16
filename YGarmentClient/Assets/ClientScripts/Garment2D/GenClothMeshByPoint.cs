using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class GenClothMeshByPoint : MonoBehaviour
{

    public bool m_bGen = false;
    public string mSavePath;
    public int width = 800;
    public int height = 800;
    public GameObject imgPlane;

    public int CompareVectorX(Vector3 l, Vector3 r)
    {
        if (l.x < r.x)
        {
            return 1;
        }
        return -1;
    }
    public int CompareVectorY(Vector3 l, Vector3 r)
    {
        if (l.y < r.y)
        {
            return 1;
        }
        return -1;
    }
    void GetPoints(ref List<Vector2> points)
    {


        MeshFilter imgMf = imgPlane.GetComponent<MeshFilter>();
        Mesh imgMesh = imgMf.mesh;

        Vector3[] verticesXSort = imgMesh.vertices;
        Vector3[] verticesYSort = imgMesh.vertices;

        Array.Sort<Vector3>(verticesXSort, CompareVectorX);
        Array.Sort<Vector3>(verticesYSort, CompareVectorY);

        Vector2 rt = new Vector2(verticesXSort[0].x, verticesYSort[0].y) ;
        Vector2 lb = new Vector2(verticesXSort[verticesXSort.Length - 1].x, verticesYSort[verticesYSort.Length - 1].y);

        
        float meshWidth = (rt.x - lb.x) * imgPlane.transform.localScale.x;
        float meshHeight = (rt.y - lb.y) * imgPlane.transform.localScale.y;
        
       

        float ratioX = meshWidth / width;
        float ratioY = meshHeight / height;

        Debug.Log(string.Format("{0} {1}", ratioX, ratioY));
        foreach (var t in transform.GetComponentsInChildren<Transform>())
        {
            if (t != transform)
            {

                float x = (t.position.x + meshWidth / 2) / ratioX;
                float y = (t.position.y ) / ratioY;
                points.Add(new Vector2(x, y));

                //Debug.Log(string.Format("{0} {1}",x,y));
            }
        }
    }

    // Start is called before the first frame update
    void Gen()
    {
        List<Vector2> points = new List<Vector2>();
        GetPoints(ref points);
       

        var mesh = UInterface.GetContoursMeshByPoints(points, width, height,20,20);


        string path = Path.Combine(Application.dataPath, mSavePath);
        byte[] bytes = MeshSerializer.WriteMesh(mesh, true);
        File.WriteAllBytes(path, bytes);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bGen)
        {
            m_bGen = false;
            Gen();
        }
        
    }
}
