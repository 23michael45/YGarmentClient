using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class YUV : MonoBehaviour
{
    public bool m_bLoadYUV = false;

    public int m_Width = 680;
    public int m_Height = 680;
    

    // Start is called before the first frame update
    void Load()
    {
        Texture2D Y = new Texture2D(m_Width, m_Height, TextureFormat.Alpha8, false);
        Color32[] grayColors = new Color32[m_Width * m_Height];
        for (int i = 0; i < grayColors.Length; ++i)
        {
            grayColors[i].a = 0;
        }
        Y.filterMode = FilterMode.Bilinear;
        Y.wrapMode = TextureWrapMode.Clamp;
        Y.SetPixels32(grayColors);
        Y.Apply();

        grayColors = new Color32[m_Width/2 * m_Height/2];
        for (int i = 0; i < grayColors.Length; ++i)
        {
            grayColors[i].a = 128;
        }
        Texture2D U = new Texture2D(m_Width/2, m_Height/2, TextureFormat.Alpha8,false);
        U.filterMode = FilterMode.Bilinear;
        U.wrapMode = TextureWrapMode.Clamp;
        U.SetPixels32(grayColors);
        U.Apply();
        Texture2D V = new Texture2D(m_Width/2, m_Height/2, TextureFormat.Alpha8,false);
        V.filterMode = FilterMode.Bilinear;
        V.wrapMode = TextureWrapMode.Clamp;
        V.SetPixels32(grayColors);
        V.Apply();

        UInterface.SetUnityTexturePtr("Y", Y);
        UInterface.SetUnityTexturePtr("U", U);
        UInterface.SetUnityTexturePtr("V", V);

        UInterface.DoFillUnityData();

        gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTexY", Y);
        gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTexU", U);
        gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTexV", V);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bLoadYUV)
        {
            m_bLoadYUV = false;
            Load();
        }
    }
}
