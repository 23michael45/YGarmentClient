using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaselFaceModel : MonoBehaviour
{
    public bool m_bChangeShape = false;
    public bool m_bChangeColor = false;

    Mesh m_Mesh;
    Vector3[] m_Vertices;
    Color[] m_Colors;
    int[] m_Triangles;
    int m_PcaDimCount;
    float[] m_Coff;


    public static IntPtr m_NativeHandle;

    IEnumerator Start()
    {
        UInterface.LoadLibrary();
        m_Mesh = GetComponent<MeshFilter>().mesh;

        CreateBaselFaceModel();

        Debug.Log(string.Format("Vertices : {0}", m_Vertices[0]));
        Debug.Log(string.Format("Triangle:{0} {1} {2}", m_Triangles[0], m_Triangles[1], m_Triangles[2]));

        m_Colors = new Color[m_Vertices.Length];
        for (int i = 0; i < m_Colors.Length; i++)
        {
            m_Colors[i] = new Color(0.650f, 0.443f, 0.313f, 1.000f);
        }

        m_Mesh.vertices = m_Vertices;
        m_Mesh.colors = m_Colors;
        m_Mesh.triangles = m_Triangles;


        m_Coff = new float[m_PcaDimCount];
        UInterface.SetMeshVerticesMemoryAddr(m_NativeHandle, m_Vertices,m_Colors);


        yield return new WaitForEndOfFrame();

        ChangeColorCoff();
        m_Mesh.colors = m_Colors;

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
            for(int i = 0; i < 199;i++)
            {
                m_Coff[i] = UnityEngine.Random.Range(-1.1f, 1.1f);
            }
            ChangeShapeCoff();

            m_Mesh.vertices = m_Vertices;
        }
        if(m_bChangeColor)
        {
            m_bChangeColor = false;

            for (int i = 0; i < 199; i++)
            {
                m_Coff[i] = UnityEngine.Random.Range(-1.1f, 1.1f);
            }

            ChangeColorCoff();

            m_Mesh.colors = m_Colors;

            Debug.Log(m_Colors[100]);
            Debug.Log(m_Colors[101]);
            //int start = 20000;
            //int end = start +5000;
            //for(int i = start; i < end; i++)
            //{
            //    Debug.Log(string.Format("Change Color {0}: {1}",i, m_Colors[i].ToString()));

            //}
        }
        

    }


    void CreateBaselFaceModel()
    {
        string BFMFileName = "D:/DevelopProj/Yuji/FaceModel/model2017-1_bfm_nomouth.h5";
        //string BFMFileName = "D:/DevelopProj/Yuji/FaceModel/sfm_shape_3448.h5";
        m_NativeHandle = UInterface.CreateBaselFaceModel(BFMFileName, ref m_Vertices, ref m_Triangles,ref m_PcaDimCount);
        Debug.Log(string.Format("Create BaselFaceModel : {0}", m_NativeHandle.ToString()));
    }
    
    void ChangeShapeCoff()
    {
        UInterface.ChangeBaselFaceModelShapeCoff(m_NativeHandle, m_Coff);
        Debug.Log("ChangeShapeCoff OK");
    }
    void ChangeExpressionCoff()
    {
        UInterface.ChangeBaselFaceModelExpressionCoff(m_NativeHandle, m_Coff);
        Debug.Log("ChangeExpressionCoff OK");
    }
    void ChangeColorCoff()
    {
        UInterface.ChangeBaselFaceModelColorCoff(m_NativeHandle, m_Coff);
        Debug.Log("ChangeColorCoff OK");
    }

    public void ExternalUpdateShape()
    {
        m_Mesh.vertices = m_Vertices;
    }
}
