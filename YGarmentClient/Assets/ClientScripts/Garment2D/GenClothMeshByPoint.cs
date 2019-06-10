using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GenClothMeshByPoint : MonoBehaviour
{
    public string mSavePath;
    // Start is called before the first frame update
    void Start()
    {
        List<Vector2> points = new List<Vector2>();

        foreach(var t in transform.GetComponentsInChildren<Transform>())
        {
            if(t != transform)
            {
                float x = (t.position.x + 4) * 100;
                float y = (t.position.y + 4) * 100;
                points.Add(new Vector2(x, y));
            }
        }

        UInterface uinterface = new UInterface();
        var mesh = uinterface.GetContoursMeshByPoints(points, 800, 800);


        string path = Path.Combine(Application.dataPath, mSavePath);
        byte[] bytes = MeshSerializer.WriteMesh(mesh, true);
        File.WriteAllBytes(path, bytes);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
