using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Draw3448Keypoint : MonoBehaviour
{
    public BaselFaceModel.EBFMTYPE m_BFBType = BaselFaceModel.EBFMTYPE.EBT_53149;
    Dictionary<int, int> m_KVDic3448 = new Dictionary<int, int> { { 8, 32 }, { 17, 224 }, { 18, 228 }, { 19, 232 }, { 20, 2085 }, { 21, 156 }, { 22, 589 }, { 23, 2090 }, { 24, 665 }, { 25, 661 }, { 26, 657 }, { 27, 2841 }, { 28, 378 }, { 29, 271 }, { 30, 113 }, { 31, 99 }, { 32, 2793 }, { 33, 269 }, { 34, 2796 }, { 35, 536 }, { 36, 176 }, { 37, 171 }, { 38, 190 }, { 39, 180 }, { 40, 172 }, { 41, 173 }, { 42, 613 }, { 43, 623 }, { 44, 604 }, { 45, 609 }, { 46, 606 }, { 47, 605 }, { 48, 397 }, { 49, 314 }, { 50, 412 }, { 51, 328 }, { 52, 824 }, { 53, 735 }, { 54, 811 }, { 55, 840 }, { 56, 692 }, { 57, 410 }, { 58, 263 }, { 59, 430 }, { 61, 415 }, { 62, 422 }, { 63, 827 }, { 65, 816 }, { 66, 441 }, { 67, 403 } };

    Dictionary<int, int> m_KVDic28588 = new Dictionary<int, int> { { 0, 16208 }, { 1, 16356 }, { 2, 16377 }, { 3, 16401 }, { 4, 16556 }, { 5, 25923 }, { 6, 26307 }, { 7, 26643 }, { 8, 27051 }, { 9, 27507 }, { 10, 27843 }, { 11, 28059 }, { 12, 22104 }, { 13, 22465 }, { 14, 22313 }, { 15, 22421 }, { 16, 22401 }, { 17, 18239 }, { 18, 23086 }, { 19, 23446 }, { 20, 23638 }, { 21, 23835 }, { 22, 24243 }, { 23, 24407 }, { 24, 24623 }, { 25, 24983 }, { 26, 20435 }, { 27, 8125 }, { 28, 8140 }, { 29, 8149 }, { 30, 8156 }, { 31, 6632 }, { 32, 7342 }, { 33, 8171 }, { 34, 8994 }, { 35, 9700 }, { 36, 2991 }, { 37, 4404 }, { 38, 5308 }, { 39, 6340 }, { 40, 4932 }, { 41, 4157 }, { 42, 10262 }, { 43, 11160 }, { 44, 11932 }, { 45, 13359 }, { 46, 12072 }, { 47, 11299 }, { 48, 5262 }, { 49, 6155 }, { 50, 7354 }, { 51, 8182 }, { 52, 8889 }, { 53, 10077 }, { 54, 10729 }, { 55, 9260 }, { 56, 8552 }, { 57, 8198 }, { 58, 7726 }, { 59, 7018 }, { 60, 5906 }, { 61, 7837 }, { 62, 8191 }, { 63, 8545 }, { 64, 9726 }, { 65, 8545 }, { 66, 8191 }, { 67, 7837 } };
    Mesh m_Mesh;


    List<int> m_RightContour = new List<int> { 380,
            373,
            356,
            358,
            359,
            360,
            365,
            363,
            364,
            388,
            391,
            392,
            393,
            11,
            21,
            25,
            22};
    List<int> m_LeftContour = new List<int> {
             795,
            790,
            773,
            775,
            776,
            777,
            782,
            780,
            781,
            802,
            805,
            806,
            807,
            454,
            464,
            466,
            465 };
    [Serializable]
    public class EdgeTopoloty
    {
        public edge_topology edge_topology;
    }
    [Serializable]
    public class edge_topology
    {
        public item[] adjacent_faces;
        public item[] adjacent_vertices;
    }
    [Serializable]
    public class item
    {
        public int value0;
        public int value1;
    }

    public GameObject m_Prefab;

    EdgeTopoloty m_etData;

    public bool m_bEdge = false;
    public bool m_bTopology = false;
    public bool m_bKeypoint = false;



    void LoadJsonTopology()
    {
        string jsonStr = File.ReadAllText("D:/DevelopProj/Yuji/Thirdparty/eos/share/sfm_3448_edge_topology.json");
        m_etData = JsonUtility.FromJson<EdgeTopoloty>(jsonStr);

        Debug.Log(m_etData.edge_topology.adjacent_faces.Length);
        Debug.Log(m_etData.edge_topology.adjacent_vertices.Length);

        //item item = new item();
        //item.value0 = 0; item.value1 = 1;
        //edge_topology et = new edge_topology();
        //et.adjacent_faces = new item[1];
        //et.adjacent_faces[0] = item;
        //EdgeTopoloty dtD = new EdgeTopoloty();
        //dtD.edge_topology = et;

        //string js = JsonUtility.ToJson(dtD);
        //Debug.Log(js);
    }
    void DrawTopology()
    {
        //int i = 0;
        //foreach(var item in m_etData.edge_topology.adjacent_vertices)
        //{

        //    Debug.Log(item.value0);
        //    Debug.Log(item.value1);
        //    AddPointAtVertex(i,item.value0);
        //    AddPointAtVertex(i,item.value1);

        //    i++;
        //    if(i > 200)
        //    {

        //        break;
        //    }
        //}


        var indices = new int[m_etData.edge_topology.adjacent_vertices.Length * 2];
        //for (int i = 0; i < indices.Length; i++)
        //{
        //    indices[i] = i;
        //}

        int i = 0;
        foreach (var item in m_etData.edge_topology.adjacent_vertices)
        {

            Debug.Log(item.value0);
            Debug.Log(item.value1);

            indices[i] = item.value0;
            indices[i + 1] = item.value1;

            i += 2;
        }

        m_Mesh.SetIndices(indices, MeshTopology.Points, 0);
        m_Mesh.RecalculateBounds();
        GetComponent<MeshFilter>().mesh = m_Mesh;
    }
    void DrawEdge()
    {
        for(int i = 0; i < m_LeftContour.Count;i++)
        {
            AddPointAtVertex(i, m_LeftContour[i]);

        }
        for (int i = 0; i < m_RightContour.Count; i++)
        {

            AddPointAtVertex(i, m_RightContour[i]);
        }
    }
    void DrawKeyPoint()
    {
        Dictionary<int, int> m_KVDic = null;
        if (m_BFBType == BaselFaceModel.EBFMTYPE.EBT_28588)
        {
            m_KVDic = m_KVDic28588;

            foreach (var kv in m_KVDic)
            {
                int key = kv.Key;
                int val = kv.Value;

                AddPointAtVertex(key, val);
            }
        }
        else if (m_BFBType == BaselFaceModel.EBFMTYPE.EBT_3448)
        {
            m_KVDic = m_KVDic3448;

            foreach (var kv in m_KVDic)
            {
                int key = kv.Key;
                int val = kv.Value+1;

                AddPointAtVertex(key, val);
            }
        }

    }

    void AddPointAtVertex(int key,int verIndex)
    {
        Vector3 pos = m_Mesh.vertices[verIndex];


        GameObject keypoint = GameObject.Instantiate(m_Prefab);
        keypoint.SetActive(true);
        keypoint.transform.parent = transform;

        keypoint.transform.localPosition = pos;
        keypoint.gameObject.name = key.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadJsonTopology();

        m_Mesh = GetComponentInChildren<MeshFilter>().mesh;

    
        if(m_bEdge)
        {
            DrawEdge();
        }
        if(m_bKeypoint)
        {
            DrawKeyPoint();
        }
        if(m_bTopology)
        {
            DrawTopology();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
