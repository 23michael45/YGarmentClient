using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityFBXExporter;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GridCreator : MonoBehaviour
{

    public bool bCreateMash = false;
    public bool bSaveFBX = false;
    public int SegX = 100;
    public int SegY = 100;



    float width;
    float height;

    public string m_SaveMeshName;

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

            //ObjExporter.MeshToFile(gameObject.GetComponent<MeshFilter>(), path);

            //byte[] bytes = MeshSerializer.WriteMesh(m_Mesh, true);
            //File.WriteAllBytes(path, bytes);

        }
    }

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
            CreatePlane();
        }
    }


    void CreatePlane()
    {
        width = 1f / SegX;
        height = 1f / SegY;
        var mesh = new Mesh();
        var mf = GetComponent<MeshFilter>();
        mf.mesh = mesh;



        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();

        var index = 0;
        for (var x = 0; x < SegX; x++)
        {
            for (var y = 0; y < SegY; y++)
            {
                //AddVertices(x,y, vertices);
                AddVerticesCylinder(x, y, vertices);
                index = AddTriangles(index, triangles);
                AddNormals(normals);
                AddUvs(x,y, uvs);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }

    private void AddVertices(int x, int y, ICollection<Vector3> vertices)
    {
        vertices.Add(new Vector3((x * width), (y * height), 0));
        vertices.Add(new Vector3((x * width) + width, (y * height), 0));
        vertices.Add(new Vector3((x * width) + width, (y * height) + height, 0));
        vertices.Add(new Vector3((x * width), (y * height) + height, 0));
    }

    private void AddVerticesCylinder(int x, int y, ICollection<Vector3> vertices)
    {
        float theta0 = (float)x * width * 2 * Mathf.PI;
        float theta1 = ((float)x + 1f) * width * 2 * Mathf.PI;
        float radius = 1f;

        vertices.Add(new Vector3(Mathf.Cos(theta0) * radius, (y * height), Mathf.Sin(theta0) * radius));
        vertices.Add(new Vector3(Mathf.Cos(theta1) * radius, (y * height), Mathf.Sin(theta1) * radius));
        vertices.Add(new Vector3(Mathf.Cos(theta1) * radius, (y * height) + height, Mathf.Sin(theta1) * radius));
        vertices.Add(new Vector3(Mathf.Cos(theta0) * radius, (y * height) + height, Mathf.Sin(theta0) * radius));
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

    private void AddUvs(int x,int y, ICollection<Vector2> uvs)
    {
        float scale = .8f;
        float offset = .1f;

        uvs.Add(new Vector2(x * width, y * height * scale + offset));
        uvs.Add(new Vector2((x + 1) * width , y * height * scale + offset));
        uvs.Add(new Vector2((x + 1) * width, (y + 1) * height * scale + offset));
        uvs.Add(new Vector2(x * width, (y + 1) * height * scale + offset));
    }
}