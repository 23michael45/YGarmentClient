using mattatz.Triangulation2DSystem;
using mattatz.Triangulation2DSystem.Example;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Wrapping : MonoBehaviour
{
    public Transform mTarget;
    public Texture2D mTexture;

    public int X_POINT_COUNT = 40;
    public int Y_POINT_COUNT = 40;
    public int X_SPACING = 1;
    public int Y_SPACING = 1;

    public float threshold = .01f;
    public float angle = 20f;

    public GameObject prefab;

    [SerializeField] Material lineMat;

    public string mSavePath;

    List<Vector2> mPoints = new List<Vector2>();

    private void Start_Obs()
    {

        UInterface uinterface = new UInterface();
        var dst = uinterface.DetectContoursImage(mTexture);
        var PArray = uinterface.DetectContours(dst);

        Vector2[] NormalPArray = new Vector2[PArray.Length];
        for(int i = 0; i < PArray.Length;i++)
        {
            float x = PArray[i].x;
            float y = PArray[i].y;
            NormalPArray[i] = new Vector2(x / mTexture.width -0.5f,y / mTexture.height - 0.5f);
        }


        mPoints.AddRange(NormalPArray);

        Build();
    }
    private void Start()
    {

        UInterface uinterface = new UInterface();
        var mesh = uinterface.GetContoursMesh(mTexture);

        string path = Path.Combine(Application.dataPath, mSavePath);
        byte[] bytes = MeshSerializer.WriteMesh(mesh, true);
        File.WriteAllBytes(path, bytes);
    }

    void Build()
    {
        mPoints = Utils2D.Constrain(mPoints, threshold);
        var polygon = Polygon2D.Contour(mPoints.ToArray());

        var vertices = polygon.Vertices;
        if (vertices.Length < 3) return; // error

        var triangulation = new Triangulation2D(polygon, angle);
        Mesh mesh = triangulation.Build();

        Vector2[] uvs = new Vector2[mesh.vertices.Length];
        
        for(int i = 0; i< uvs.Length;i++)
        {
            Vector2 uv = new Vector2();
            uv.x = mesh.vertices[i].x + 0.5f;
            uv.y = mesh.vertices[i].y + 0.5f;

            uvs[i] = uv;
        }
        mesh.uv = uvs;

        string path = Path.Combine(Application.dataPath, mSavePath);
        byte[] bytes = MeshSerializer.WriteMesh(mesh, true);
        File.WriteAllBytes(path, bytes);

        Clear();
    }

    void Clear()
    {
        mPoints.Clear();
    }

    void OnRenderObject()
    {
        if (mPoints != null)
        {
            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);
            lineMat.SetColor("_Color", Color.white);
            lineMat.SetPass(0);
            GL.Begin(GL.LINES);
            for (int i = 0, n = mPoints.Count - 1; i < n; i++)
            {
                GL.Vertex(mPoints[i]); GL.Vertex(mPoints[i + 1]);
            }
            GL.End();
            GL.PopMatrix();
        }
    }


    List<Vector2> GenPlaneVertices(int imgWidth,int imgHeight)
    {
        double width = X_SPACING * (X_POINT_COUNT - 1);
        double height = Y_SPACING * (Y_POINT_COUNT - 1);
        double minX = -width / 2;
        double minY = -height / 2;

        List<Vector2> points = new List<Vector2>();
        //Vector3[] vertices = new Vector3[X_POINT_COUNT * Y_POINT_COUNT];

        for (int i = 0; i < X_POINT_COUNT; i++)
        {
            for (int j = 0; j < Y_POINT_COUNT; j++)
            {
                double x = minX + i * X_SPACING;
                double y = minY + j * Y_SPACING;
                double z = 0;
                //if draw vertex of grid, uncomment below
                //glVertex3f(x, y, z);

                //save the mat points to get v
                //vertices[i * Y_POINT_COUNT + j].x = (float)x;
                //vertices[i * Y_POINT_COUNT + j].y = (float)y;

                points.Add(new Vector2((float)x,(float)y));
            }
        }
        return points;

      
    }
    
    
}
