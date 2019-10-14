using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityFBXExporter;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CylinderCreator : MonoBehaviour
{

    public bool bCreateMash = false;
    public bool bSaveFBX = false;
    public float Radius = 10;
    public float Length = 50;
    public int SegR = 36;
    public int SegL = 50;



    public string m_SaveMeshName;


    float theta;
    float delta;

    private void Start()
    {
        //CreatePlane();
        //SaveMesh();
    }
    void SaveMesh()
    {
        if (!string.IsNullOrEmpty(m_SaveMeshName))
        {
            string path = Path.Combine(Application.dataPath, m_SaveMeshName);

            FBXExporter.ExportGameObjToFBX(gameObject, path, false, false);
        }
    }
    [ExecuteInEditMode]
    private void Update()
    {
        if(bSaveFBX)
        {
            bSaveFBX = false;
            SaveMesh();
        }
        if (bCreateMash)
        {
            bCreateMash = false;
            CreateMesh();
        }
    }


    void CreateMesh()
    {
        theta = Mathf.PI * 2 / SegR;
        delta = Length / SegL;

        var mesh = new Mesh();
        var mf = GetComponent<MeshFilter>();
        mf.mesh = mesh;



        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();

        var index = 0;
        for (var ang = 0; ang < SegR; ang++)
        {
            for (var len  = 0; len < SegL; len++)
            {


                AddVerticesCylinder(ang, len, vertices);
                index = AddTriangles(index, triangles);
                AddNormals(normals);
                AddUvs(ang,len, uvs);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }


    private void AddVerticesCylinder(int ang, int len, ICollection<Vector3> vertices)
    {
        float theta0 = (float)ang * theta;
        float theta1 = (float)(ang + 1f) * theta;

        vertices.Add(new Vector3(Mathf.Cos(theta0) * Radius,  Mathf.Sin(theta0) * Radius,len * delta));
        vertices.Add(new Vector3(Mathf.Cos(theta1) * Radius,  Mathf.Sin(theta1) * Radius, len * delta));
        vertices.Add(new Vector3(Mathf.Cos(theta1) * Radius,  Mathf.Sin(theta1) * Radius, (len + 1) * delta));
        vertices.Add(new Vector3(Mathf.Cos(theta0) * Radius,  Mathf.Sin(theta0) * Radius, (len+ 1) * delta));
    }

    private int AddTriangles(int index, ICollection<int> triangles)
    {
        triangles.Add(index + 2);
        triangles.Add(index + 1);
        triangles.Add(index);
        triangles.Add(index);
        triangles.Add(index + 3);
        triangles.Add(index + 2);
        index += 4;
        return index;
    }

    private void AddNormals(ICollection<Vector3> normals)
    {
        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);
    }

    private void AddUvs(int ang,int len, ICollection<Vector2> uvs)
    {
        float deltaX = 1.0f / SegR;
        float deltaY = 1.0f / SegL;


        uvs.Add(new Vector2(ang * deltaX, len * deltaY));
        uvs.Add(new Vector2((ang + 1) * deltaX, len * deltaY));
        uvs.Add(new Vector2((ang + 1) * deltaX, (len + 1) * deltaY));
        uvs.Add(new Vector2(ang * deltaX, (len + 1) * deltaY));
    }
}