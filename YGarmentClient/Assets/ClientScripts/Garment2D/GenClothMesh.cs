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

        UInterface uinterface = new UInterface();
        var mesh = uinterface.GetContoursMesh(mTexture);

        string path = Path.Combine(Application.dataPath, mSavePath);
        byte[] bytes = MeshSerializer.WriteMesh(mesh, true);
        File.WriteAllBytes(path, bytes);
    }

}
