using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityFBXExporter;

public class ModelUVTrim : MonoBehaviour
{
    public Transform mLessVerticesMesh;
    public Transform mMoreVerticesMesh;

    public MeshFilter mFilter;
    void Start()
    {
        mFilter = gameObject.AddComponent<MeshFilter>();
    }
    void OnDestroy()
    {
        Debug.Log("Stop");
        StopAllCoroutines();
    }

    public void Trim()
    {
        StartCoroutine(Triming());
    }

    IEnumerator Triming()
    {
        Mesh lessMesh = mLessVerticesMesh.GetComponent<MeshFilter>().sharedMesh;
        Mesh moreMesh = mMoreVerticesMesh.GetComponent<MeshFilter>().sharedMesh;


        Mesh newMesh = Instantiate(mLessVerticesMesh.GetComponent<MeshFilter>().sharedMesh);




        Vector3[] lessVertices = lessMesh.vertices;
        Vector3[] moreVertices = moreMesh.vertices;
        Vector2[] moreUV = moreMesh.uv;

        Vector2[] lessUV = lessMesh.uv;


        Matrix4x4 l2wLess = mLessVerticesMesh.transform.localToWorldMatrix;
        Matrix4x4 l2wMore = mMoreVerticesMesh.transform.localToWorldMatrix;


        int lessCount = lessVertices.Length;
        for (int i = 0; i < lessCount; i++)
        {
            bool exist = false;

            int moreCount = moreVertices.Length;
            for (int j = 0; j < moreCount; j++)
            {
                float dist = Vector3.Distance(l2wLess.MultiplyPoint(lessVertices[i]) ,l2wMore.MultiplyPoint(moreVertices[j]));
                if ( dist < 0.001f)
                //if (lessVertices[i] == moreVertices[j])
                {

                    //Debug.Log("Dist:" + dist);
                    exist = true;
                    lessUV[i] = moreUV[j];
                    break;
                }
            }

            if (exist == false)
            {
                Debug.LogError(string.Format("Vertex not exsit in LessMesh {0}", i));
            }

            if (i % 100 == 0)
            {
                Debug.Log("Triming:" + i);
                yield return new WaitForEndOfFrame();

            }
        }

        newMesh.uv = lessUV;
        mFilter.sharedMesh = newMesh;
    }

    public void Save(string path)
    {
        if (!string.IsNullOrEmpty(path))
        {
            string fullpath = Path.Combine(Application.dataPath, path);

            FBXExporter.ExportGameObjToFBX(gameObject, fullpath, false, false);

            //ObjExporter.MeshToFile(gameObject.GetComponent<MeshFilter>(), path);

            //byte[] bytes = MeshSerializer.WriteMesh(m_Mesh, true);
            //File.WriteAllBytes(path, bytes);

        }

    }
}
