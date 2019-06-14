using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaselFaceModel : MonoBehaviour
{
    public bool m_Check = false;

    Mesh m_Mesh;
    Vector3[] m_Vertices;
    int[] m_Triangles;
    int m_PcaDimCount;
    float[] m_Coff;

    IntPtr m_NativeHandle;
    bool m_Dirty = false;


    void Start()
    {
        UInterface.LoadLibrary();
        m_Mesh = GetComponent<MeshFilter>().mesh;

        CreateBaselFaceModel();

        Debug.Log(string.Format("Vertices : {0}", m_Vertices[0]));
        Debug.Log(string.Format("Triangle:{0} {1} {2}", m_Triangles[0], m_Triangles[1], m_Triangles[2]));

        m_Mesh.vertices = m_Vertices;
        m_Mesh.triangles = m_Triangles;


        m_Coff = new float[m_PcaDimCount];
        UInterface.SetMeshVerticesMemoryAddr(m_NativeHandle, m_Vertices);

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
        if(m_Check)
        {
            m_Check = false;
            for(int i = 0; i < 199;i++)
            {
                m_Coff[i] = UnityEngine.Random.Range(-1.1f, 1.1f);
            }
            ChangeCoff();
        }


        if(m_Dirty)
        {
            m_Mesh.vertices = m_Vertices;
            m_Dirty = false;

            Debug.Log("Update Mesh OK");
        }

    }


    void CreateBaselFaceModel()
    {
        string BFMFileName = "D:/DevelopProj/Yuji/FaceModel/model2017-1_bfm_nomouth.h5";
        m_NativeHandle = UInterface.CreateBaselFaceModel(BFMFileName, ref m_Vertices, ref m_Triangles,ref m_PcaDimCount);
        Debug.Log(string.Format("Create BaselFaceModel : {0}", m_NativeHandle.ToString()));
    }
    
    void ChangeCoff()
    {
        UInterface.ChangeBaselFaceModelCoff(m_NativeHandle, m_Coff);
        m_Dirty = true;
        Debug.Log("ChangeCoff OK");
    }

}
