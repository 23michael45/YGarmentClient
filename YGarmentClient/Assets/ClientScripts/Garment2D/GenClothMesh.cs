using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GenClothMesh : MonoBehaviour
{

    public Texture2D mTexture;
    public string mSavePath;
    private void Start()
    {
        
        var mesh = UInterface.GetContoursMesh(mTexture);

        string path = Path.Combine(Application.dataPath, mSavePath);
        byte[] bytes = MeshSerializer.WriteMesh(mesh, true);
        File.WriteAllBytes(path, bytes);
    }

}
