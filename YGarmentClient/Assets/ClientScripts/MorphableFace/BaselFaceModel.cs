using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityFBXExporter;

public class BaselFaceModel : MonoBehaviour
{
    public bool m_bChangeShape = false;
    public bool m_bChangeColor = false;

    Mesh m_Mesh;
    Vector3[] m_Vertices;
    Vector2[] m_Uvs;
    Color[] m_Colors;
    int[] m_Triangles;
    int m_PcaDimCount;
    float[] m_ShapeCoff;
    float[] m_ExpCoff;

    public int m_FinalTextureWidth = 1024;
    public int m_FinalTextureHeight = 1024;
    Texture2D m_FinalTexture;
    Color32[] m_FinalTextureRawData;

    public Mesh m_UVMesh;
    

    public enum EBFMTYPE
    {
        EBT_3448,
        EBT_53149,
        EBT_28588,
    }
    public EBFMTYPE m_BFBType = EBFMTYPE.EBT_53149;

    public string mModelFile_28588 = "D:/DevelopProj/Yuji/FaceModel/model2017-1_face12_nomouth_uv.h5";
    public string mModelFile_3448 = "D:/DevelopProj/Yuji/FaceModel/sfm_shape_3448.h5";
    public string mModelFile_53149 = "D:/DevelopProj/Yuji/FaceModel/model2017-1_bfm_nomouth_uv.h5"; 
    public string mFaceShpaeFileName = "D:/DevelopProj/Yuji/FaceModel/shape_predictor_68_face_landmarks.dat";

    public static IntPtr m_NativeHandle;

    public bool bSaveFBX = false;
    public string m_SaveMeshName;

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

    IEnumerator Start()
    {
        UInterface.LoadLibrary();
        m_Mesh = GetComponent<MeshFilter>().mesh;
        
        CreateBaselFaceModel();


        Debug.Log(string.Format("Vertices : {0}", m_Vertices[0]));
        Debug.Log(string.Format("Triangle:{0} {1} {2}", m_Triangles[0], m_Triangles[1], m_Triangles[2]));

 


        if(m_UVMesh == null || m_Uvs.Length != m_Vertices.Length)
        {
            m_Uvs = new Vector2[m_Vertices.Length];
            for (int i = 0; i < m_Uvs.Length; i++)
            {
                m_Uvs[i] = new Vector2(0f, 0f);
            }
        }
        else
        {
            m_Uvs = m_UVMesh.uv;
            Debug.Log("Texcoord:" + m_Uvs[0].x + "  :  " + m_Uvs[0].y);
        }


        m_Mesh.vertices = m_Vertices;
        m_Mesh.colors = m_Colors;
        m_Mesh.triangles = m_Triangles;


        m_ShapeCoff = new float[m_PcaDimCount];
        UInterface.SetMeshVerticesMemoryAddr(m_NativeHandle, m_Vertices, m_Uvs, m_Colors, false);


        m_FinalTexture = new Texture2D(m_FinalTextureWidth, m_FinalTextureHeight, TextureFormat.RGBA32,false);
        m_FinalTextureRawData = new Color32[m_FinalTextureWidth * m_FinalTextureHeight];
        UInterface.SetTextureMemoryAddr(m_NativeHandle, m_FinalTextureRawData, m_FinalTextureWidth, m_FinalTextureHeight);


        UInterface.SaveBFMH5(m_NativeHandle, "D:/DevelopProj/Yuji/FaceModel/53149_uv.h5");

        gameObject.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", m_FinalTexture);

        m_Mesh.uv = m_Uvs;
        //SaveMesh();
        
        yield return new WaitForEndOfFrame();
        

    }
    private void OnDestroy()
    {
        Debug.Log(string.Format("Destroy BaselFaceModel : {0}", m_NativeHandle.ToString()));
        UInterface.DestroyBaselFaceModel(m_NativeHandle);

        UInterface.FreeLibrary();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bChangeShape)
        {
            m_bChangeShape = false;
            for(int i = 0; i < m_PcaDimCount; i++)
            {
                m_ShapeCoff[i] = UnityEngine.Random.Range(-1.1f, 1.1f);
            }
            ChangeShapeCoff();

            m_Mesh.vertices = m_Vertices;
            m_Mesh.uv = m_Uvs;
        }
        if(m_bChangeColor)
        {
            m_bChangeColor = false;

            for (int i = 0; i < m_PcaDimCount; i++)
            {
                m_ShapeCoff[i] = UnityEngine.Random.Range(-1.1f, 1.1f);
            }

            ChangeColorCoff();



            //m_Colors = new Color[m_Mesh.vertexCount];
            //for (int i = 0; i < m_Colors.Length; i++)
            //{
            //    float r = UnityEngine.Random.Range(0f, 1f);
            //    float g = UnityEngine.Random.Range(0f, 1f);
            //    float b = UnityEngine.Random.Range(0f, 1f);
            //    m_Colors[i] = new Color(r, g, b);

            //    //Debug.Log(string.Format("{0} {1} {2}", r, g, b));
            //    //Debug.Log(m_Colors[i]);
            //}
            m_Mesh.colors = m_Colors;

            //Debug.Log(m_Colors[100]);
            //Debug.Log(m_Colors[101]);
            //int start = 20000;
            //int end = start +5000;
            //for(int i = start; i < end; i++)
            //{
            //    Debug.Log(string.Format("Change Color {0}: {1}",i, m_Colors[i].ToString()));

            //}
        }
        if (bSaveFBX)
        {
            bSaveFBX = false;
            SaveMesh();
        }

    }


    void CreateBaselFaceModel()
    {
        string BFMFileName = "";
        if (m_BFBType == EBFMTYPE.EBT_53149)
        {
            BFMFileName = mModelFile_53149;
        }
        else if(m_BFBType == EBFMTYPE.EBT_3448)
        {
            BFMFileName = mModelFile_3448;

        }
        else if (m_BFBType == EBFMTYPE.EBT_28588)
        {
            BFMFileName = mModelFile_28588;
        } 


        
        m_NativeHandle = UInterface.CreateBaselFaceModel(BFMFileName, mFaceShpaeFileName,ref m_Vertices, ref m_Triangles,ref m_PcaDimCount,ref m_Colors);
        Debug.Log(string.Format("Create BaselFaceModel : {0}", m_NativeHandle.ToString()));
    }
    
    void ChangeShapeCoff()
    {
        UInterface.ChangeBaselFaceModelShapeCoff(m_NativeHandle, m_ShapeCoff);
        Debug.Log("ChangeShapeCoff OK");
    }
    void ChangeExpressionCoff()
    {
        UInterface.ChangeBaselFaceModelExpressionCoff(m_NativeHandle, m_ShapeCoff,m_ExpCoff);
        Debug.Log("ChangeExpressionCoff OK");
    }
    void ChangeColorCoff()
    {
        UInterface.ChangeBaselFaceModelColorCoff(m_NativeHandle, m_ShapeCoff);
        Debug.Log("ChangeColorCoff OK");
    }

    public void ExternalUpdateShape()
    {
        m_Mesh.vertices = m_Vertices;
        m_Mesh.colors = m_Colors;
        m_Mesh.uv = m_Uvs;
    }
    public void ExternalUpdateTexture()
    {
        m_FinalTexture.SetPixels32(m_FinalTextureRawData);
        m_FinalTexture.Apply();
    }
}
