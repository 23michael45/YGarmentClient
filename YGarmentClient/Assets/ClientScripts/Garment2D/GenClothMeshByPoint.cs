using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GenClothMeshByPoint : MonoBehaviour
{
    public string mSavePath;
    public int width = 800;
    public int height = 800;
    // Start is called before the first frame update
    void Start()
    {
        List<Vector2> points = new List<Vector2>();

        foreach(var t in transform.GetComponentsInChildren<Transform>())
        {
            if(t != transform)
            {
                float x = (t.position.x + 0.5f) * width;
                float y = (t.position.y + 0.5f) * height;
                points.Add(new Vector2(x, y));
            }
        }
        
        var mesh = UInterface.GetContoursMeshByPoints(points, width, height);


        string path = Path.Combine(Application.dataPath, mSavePath);
        byte[] bytes = MeshSerializer.WriteMesh(mesh, true);
        File.WriteAllBytes(path, bytes);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
