using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityFBXExporter;

[ExecuteInEditMode]
public class SimplifyFaceModel : MonoBehaviour
{
    [Serializable]
    public class item
    {
        public int index;
        public int highIndex;
        public int lowIndex;
    }
    [Serializable]
    public class HLVertexMap
    {
        public item[] items;
    }


    public float m_Scale = 0.001f;
    public Transform m_HighMeshDeformedTransform;
    public Transform m_HighMeshTransform;
    public Transform m_LowMeshTransform;
    public Transform m_TargetLowMeshTransform;
    Mesh m_TargetMesh;

    Dictionary<int, int> m_low2highDict = new Dictionary<int, int>();

    public bool m_bRaycast = false;
    public bool m_bSimplify = false;
    public bool m_bFit = false;
    public bool m_bDraw = false;
    public bool m_bSave = false;
    public bool m_bLoad = false;


    public string m_SaveMeshName = "";
    public bool m_bSaveLowMesh = false;
    void Start()
    {
        
    }

    void SaveLowMesh()
    {
        string path = Path.Combine(Application.dataPath, m_SaveMeshName);


        Vector2[] lowUV2 = m_LowMeshTransform.gameObject.GetComponent<MeshFilter>().mesh.uv2;
        m_LowMeshTransform.gameObject.GetComponent<MeshFilter>().mesh.uv = lowUV2;

        FBXExporter.ExportGameObjToFBX(m_LowMeshTransform.gameObject, path, false, false);
    }
    // Update is called once per frame
    void Update()
    {
        if (m_bFit)
        {
            DoFit();
            m_bFit = false;
        }
        if (m_bSimplify)
        {
            DoSimplify(m_bRaycast);
            m_bSimplify = false;
        }
        if (m_bDraw)
        {
            DrawTopology();
            m_bDraw = false;
        }
        if (m_bSave)
        {
            SaveJson();
            m_bSave = false;
        }
        if (m_bLoad)
        {
            LoadJson();
            m_bLoad = false;
        }
        if (m_bSaveLowMesh)
        {
            SaveLowMesh();
            m_bSaveLowMesh = false;
        }
    }
    void DoFit()
    {
        LoadJson();

        Vector3[] DeformedVertices = m_HighMeshDeformedTransform.gameObject.GetComponent<MeshFilter>().mesh.vertices;
        Vector2[] DeformedUVs = m_HighMeshDeformedTransform.gameObject.GetComponent<MeshFilter>().mesh.uv;
        //Vector3[] TargetSizeVertices = m_HighMeshTransform.gameObject.GetComponent<MeshFilter>().sharedMesh.vertices;
        Vector3[] lowVertices = m_LowMeshTransform.gameObject.GetComponent<MeshFilter>().mesh.vertices;
        Vector2[] lowUVs = new Vector2[lowVertices.Length];


        float lowMeshScale = m_LowMeshTransform.lossyScale.x;
        for (int i = 0; i < lowVertices.Length; i++)
        {

            if( m_low2highDict.ContainsKey(i))
            {
                int indexFromHigh = m_low2highDict[i];

                Vector3 pos = m_HighMeshDeformedTransform.localToWorldMatrix.MultiplyPoint(DeformedVertices[indexFromHigh]);
                //pos = pos + m_HighMeshTransform.position / lowMeshScale;


                lowVertices[i] = pos;
                lowUVs[i] = DeformedUVs[indexFromHigh];
            }
            
        }

        m_TargetLowMeshTransform.gameObject.GetComponent<MeshFilter>().mesh.vertices = lowVertices;
        m_TargetLowMeshTransform.gameObject.GetComponent<MeshFilter>().mesh.uv2 = lowUVs;
        m_TargetLowMeshTransform.gameObject.GetComponent<MeshFilter>().mesh.uv = lowUVs;
        m_TargetLowMeshTransform.gameObject.GetComponent<MeshFilter>().mesh.triangles = m_LowMeshTransform.gameObject.GetComponent<MeshFilter>().mesh.triangles;

        //Debug.Log(string.Format("org: {0} {1} {2} tar: {3} {4} {5}", ov.x, ov.y, ov.z, tv.x, tv.y, tv.z));
    }

    void DoSimplify(bool raycast = false)
    {

        Vector3[] highVertices = m_HighMeshTransform.gameObject.GetComponent<MeshFilter>().sharedMesh.vertices;
        Vector3[] lowVertices = m_LowMeshTransform.gameObject.GetComponent<MeshFilter>().sharedMesh.vertices;

        Dictionary<int, float> high2lowDistDict = new Dictionary<int, float>();
        Dictionary<int, int> high2lowDict = new Dictionary<int, int>();

        Dictionary<int, float> low2highDistDict = new Dictionary<int, float>();
        Dictionary<int, int> low2highDict = new Dictionary<int, int>();
        if (raycast)
        {


            for (int h = 0; h < highVertices.Length; h++)
            {
                Vector3 hv = m_HighMeshTransform.localToWorldMatrix.MultiplyPoint(highVertices[h]);
                Vector3 from = hv + Vector3.forward * 1000;
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(from, -Vector3.forward, out hit, Mathf.Infinity))
                {

                    Vector3 hitPos = hit.point;


                    for (int l = 0; l < lowVertices.Length; l++)
                    {
                        Vector3 lv = lowVertices[l];

                        Vector3 whv = hitPos;
                        Vector3 wlv = m_LowMeshTransform.localToWorldMatrix.MultiplyPoint(lv);

                        float dist = Vector3.Distance(whv, wlv);

                        if (high2lowDistDict.ContainsKey(h))
                        {
                        }
                        else
                        {
                            high2lowDistDict[h] = float.MaxValue; ;
                        }

                        float minDist = high2lowDistDict[h];
                        if (minDist > dist)
                        {
                            high2lowDistDict[h] = dist;
                            high2lowDict[h] = l;
                        }

                    }
                }
            }
        }
        else
        {
            bool H_L = false;
            if (H_L)
            {
                for (int h = 0; h < highVertices.Length; h++)
                {
                    Vector3 hv = highVertices[h];

                    for (int l = 0; l < lowVertices.Length; l++)
                    {
                        Vector3 lv = lowVertices[l];
                        Vector3 whv = m_HighMeshTransform.localToWorldMatrix.MultiplyPoint(hv);
                        Vector3 wlv = m_LowMeshTransform.localToWorldMatrix.MultiplyPoint(lv);

                        float dist = Vector3.Distance(whv, wlv);

                        if (high2lowDistDict.ContainsKey(h))
                        {
                        }
                        else
                        {
                            high2lowDistDict[h] = float.MaxValue; ;
                        }

                        float minDist = high2lowDistDict[h];
                        if (minDist > dist)
                        {
                            high2lowDistDict[h] = dist;
                            high2lowDict[h] = l;
                        }
                    }
                }
                
                foreach (var pair in high2lowDistDict)
                {
                    int h = pair.Key;
                    float dist = pair.Value;
                    int l = high2lowDict[h];

                    if (low2highDistDict.ContainsKey(l))
                    {
                    }
                    else
                    {
                        low2highDistDict[l] = float.MaxValue;
                    }

                    if (low2highDistDict[l] > dist)
                    {
                        low2highDistDict[l] = dist;
                        low2highDict[l] = h;
                    }
                }

                m_low2highDict = low2highDict;
            }
            else
            {
                for (int l = 0; l < lowVertices.Length; l++)
                {
                    Vector3 lv = lowVertices[l];

                    for (int h = 0; h < highVertices.Length; h++)
                    {
                        Vector3 hv = highVertices[h];
                        Vector3 whv = m_HighMeshTransform.localToWorldMatrix.MultiplyPoint(hv);
                        Vector3 wlv = m_LowMeshTransform.localToWorldMatrix.MultiplyPoint(lv);

                        float dist = Vector3.Distance(whv, wlv);

                        if (low2highDistDict.ContainsKey(l))
                        {
                        }
                        else
                        {
                            low2highDistDict[l] = float.MaxValue; ;
                        }

                        float minDist = low2highDistDict[l];
                        if (minDist > dist)
                        {
                            low2highDistDict[l] = dist;
                            low2highDict[l] = h;
                        }
                    }
                }

                m_low2highDict = low2highDict;
            }
        }

    }

    void DrawTopology()
    {
        var indices = new int[m_low2highDict.Count];
        int i = 0;
        foreach (var pair in m_low2highDict)
        { 
            indices[i] = pair.Key;
            i++;
        }
        m_TargetMesh = new Mesh();
        m_TargetMesh.vertices = m_LowMeshTransform.gameObject.GetComponent<MeshFilter>().sharedMesh.vertices;
        m_TargetMesh.SetIndices(indices, MeshTopology.Points, 0);

        gameObject.GetComponent<MeshFilter>().sharedMesh = m_TargetMesh;

    }

    void SaveJson()
    {

        HLVertexMap hlMap = new HLVertexMap();

        Debug.Log("LH Dict : " + m_low2highDict.Count);
        hlMap.items = new item[m_low2highDict.Count];

        int i = 0;
        foreach (var kv in m_low2highDict)
        {
            hlMap.items[i] = new item();
            hlMap.items[i].lowIndex = kv.Key;
            hlMap.items[i].highIndex = kv.Value;
            hlMap.items[i].index = i;
            i++;
        }

        string jstr = JsonUtility.ToJson(hlMap);
        File.WriteAllText(Application.dataPath + "/LowHighMap.json",jstr);
    }
    void LoadJson()
    {
        string jstr = File.ReadAllText(Application.dataPath + "/LowHighMap.json");
        HLVertexMap hlMap  = JsonUtility.FromJson<HLVertexMap>(jstr);

        m_low2highDict = new Dictionary<int, int>();
        foreach (var item in hlMap.items)
        {
            m_low2highDict[item.lowIndex] = item.highIndex;
        }
    }
}
