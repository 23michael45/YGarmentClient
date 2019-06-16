using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class RenderCameraSaveImage : MonoBehaviour
{
    public bool m_bSave;
    public string m_SavePath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bSave)
        {
            m_bSave = false;
            SaveImage();
        }
    }
    void SaveImage()
    {
        RenderTexture rt = gameObject.GetComponent<Camera>().targetTexture;

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes;
        bytes = tex.EncodeToPNG();

        string path = Path.Combine(Application.dataPath, m_SavePath);
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log("Saved to " + path);
    }
}
