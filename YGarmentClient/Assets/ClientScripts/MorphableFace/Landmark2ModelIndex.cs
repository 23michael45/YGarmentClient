using DlibFaceLandmarkDetector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Landmark2ModelIndex : MonoBehaviour
{
    Mesh m_Mesh;
    RenderTexture m_RenderTexture;
    public Camera m_RenderCamera;

    public int m_Width = 1024;
    public int m_Height = 1024;

    public RawImage m_ResultImage;

    public GameObject m_WorldLandmarkPrefab;
    public GameObject m_VertexLandmarkPrefab;

    public Transform m_WorldLandmarkContainer;
    public Transform m_VertexLandmarkContainer;

    public MeshCollider m_MeshCollider;

    // Start is called before the first frame update
    IEnumerator Start()
    {

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        m_MeshCollider = GetComponent<MeshCollider>();
        m_Mesh = GetComponent<MeshFilter>().mesh;
        m_MeshCollider.sharedMesh = m_Mesh;

        m_RenderTexture = new RenderTexture(m_Width, m_Height, 16, RenderTextureFormat.ARGB32);
        m_RenderCamera.targetTexture = m_RenderTexture;


        yield return new WaitForEndOfFrame();


        Texture2D tex = new Texture2D(m_RenderTexture.width, m_RenderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = m_RenderTexture;
        tex.ReadPixels(new Rect(0, 0, m_RenderTexture.width, m_RenderTexture.height), 0, 0);
        tex.Apply();


        Texture2D dstTexture2D = new Texture2D(tex.width, tex.height, tex.format, false);
        Graphics.CopyTexture(tex, dstTexture2D);

        string dlibShapePredictorFilePath = Application.dataPath + "/DlibFaceLandmarkDetector/StreamingAssets/sp_human_face_68.dat";


        FaceLandmarkDetector faceLandmarkDetector = new FaceLandmarkDetector(dlibShapePredictorFilePath);
        faceLandmarkDetector.SetImage(tex);
        List<Rect> detectResult = faceLandmarkDetector.Detect();


        foreach (var rect in detectResult)
        {
            Debug.Log("face : " + rect);

            //detect landmark points
            List<Vector2> points = faceLandmarkDetector.DetectLandmark(rect);

            Debug.Log("face points count : " + points.Count);
            foreach (var point in points)
            {
                Debug.Log("face point : x " + point.x + " y " + point.y);
            }

            //draw landmark points
            faceLandmarkDetector.DrawDetectLandmarkResult(dstTexture2D, 0, 255, 0, 255);

            WorldPoints(points);


        }


        m_ResultImage.texture = dstTexture2D;
    }

    void WorldPoints(List<Vector2> points)
    {
        float size = m_RenderCamera.orthographicSize;


        Dictionary<int, int> LandmarkMapper = new Dictionary<int, int>();
        for(int landmarkIndex = 0; landmarkIndex < points.Count;landmarkIndex++)
        {
            var point = points[landmarkIndex];
            float x = point.x / m_Width * size * 2 - size;
            float y = -( point.y / m_Height * size * 2 - size);

            GameObject landmark3d = GameObject.Instantiate(m_WorldLandmarkPrefab);
            landmark3d.SetActive(true);
            landmark3d.transform.parent = m_WorldLandmarkContainer;
            

            Vector3 from = new Vector3(x, y, 1000);


            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(from, -Vector3.forward, out hit, Mathf.Infinity))
            {


                landmark3d.transform.position = hit.point;


                if (hit.collider.gameObject.name == "BaselFaceModel2017")
                {


                    Vector3[] vertices = m_Mesh.vertices;
                    int[] triangles = m_Mesh.triangles;
                    Vector3[] vers = new Vector3[3];
                    vers[0] = vertices[triangles[hit.triangleIndex * 3 + 0]];
                    vers[1] = vertices[triangles[hit.triangleIndex * 3 + 1]];
                    vers[2] = vertices[triangles[hit.triangleIndex * 3 + 2]];

                    float dist = float.MaxValue;
                    int index = -1;
                    for (int i = 0; i < 3; i++)
                    {
                        float tempdist = Vector3.Distance(vers[i], hit.point);
                        if (tempdist < dist)
                        {
                            index = triangles[hit.triangleIndex * 3 + i];
                            dist = tempdist;
                        }

                    }

                    LandmarkMapper[landmarkIndex] = index;

                    GameObject vertexLandmark3d = GameObject.Instantiate(m_VertexLandmarkPrefab);
                    vertexLandmark3d.SetActive(true);
                    vertexLandmark3d.transform.parent = m_VertexLandmarkContainer;
                    vertexLandmark3d.transform.position = vertices[index];

                }
                else
                {

                    Vector3[] vertices = m_Mesh.vertices;
                    float dist = float.MaxValue;
                    int index = -1;
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        float tempdist = Vector3.Distance(vertices[i], hit.point);
                        if (tempdist < dist)
                        {
                            index = i;
                            dist = tempdist;
                        }

                    }

                    LandmarkMapper[landmarkIndex] = index;


                    GameObject vertexLandmark3d = GameObject.Instantiate(m_VertexLandmarkPrefab);
                    vertexLandmark3d.SetActive(true);
                    vertexLandmark3d.transform.parent = m_VertexLandmarkContainer;
                    vertexLandmark3d.transform.position = vertices[index];
                }

            }
            else
            {
                Debug.Log("Did not Hit");
            }

        }

        Debug.Log("-----------------------------------------------------------");

        string arraycode = "";
        foreach (var kv in LandmarkMapper)
        {

            Debug.Log(string.Format("Landmark {0}  to  {1} ",kv.Key,kv.Value));

            arraycode += (string.Format("[{0},{1}],", kv.Key, kv.Value));
        }

        Debug.Log(arraycode);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 from = new Vector3(0,0, 1000);
        //RaycastHit hit;
        //if (Physics.Raycast(from, -Vector3.forward, out hit, Mathf.Infinity))
        //{
        //    Debug.Log("Did Hit");
        //}
        //else
        //{
        //    Debug.Log("Did not Hit");

        //    Debug.Log("Did not Hit");
        //}
    }
}
