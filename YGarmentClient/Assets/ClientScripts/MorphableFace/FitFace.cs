//using DlibFaceLandmarkDetector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FitFace : MonoBehaviour
{

    public Texture2D m_SrcTexture;

    public bool m_bDebug = false;
    public bool m_bFit_3448 = false;
    public bool m_bFit_53149 = false;
    public bool m_bFit_28588 = false;

    public bool m_PrjectVertexOrGameObject = true;

    //FaceLandmarkDetector m_FaceLandmarkDetector;
    // Start is called before the first frame update
    IEnumerator Start()
    {


        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();


        string dlibShapePredictorFilePath = Application.dataPath + "/DlibFaceLandmarkDetector/StreamingAssets/sp_human_face_68.dat";


        //m_FaceLandmarkDetector = new FaceLandmarkDetector(dlibShapePredictorFilePath);

    }


    private void Update()
    {
        if(m_bFit_3448)
        {
            m_bFit_3448 = false;
            Fit(m_bDebug,3448);
        }
        if (m_bFit_53149)
        {
            m_bFit_53149 = false;
            Fit(m_bDebug,53149);
        }
        if (m_bFit_28588)
        {
            m_bFit_28588 = false;
            Fit(m_bDebug, 28588);
        }
    }

    void Fit(bool bDebug,int vCount)
    {
        //m_FaceLandmarkDetector.SetImage(m_SrcTexture);

        List<Vector2> points = new List<Vector2>();
        //if (usedlib)
        //{
        //    List<Rect> detectResult = m_FaceLandmarkDetector.Detect();


        //    foreach (var rect in detectResult)
        //    {
        //        points = m_FaceLandmarkDetector.DetectLandmark(rect);

               

        //        break;
        //    }
        //}
        //else
        //{
        //    points = LoadLandmarkFromFile();
        //}


        //if (points.Count > 0)
        {
            int w = m_SrcTexture.width;
            int h = m_SrcTexture.height;
            Debug.Log(string.Format("w {0} h {1}", w, h));

            //UInterface.ChangeBaselFaceModelShapeCoffFromLandmark(BaselFaceModel.m_NativeHandle, points, w, h, !usedlib);
            Matrix4x4 cameraMatrix = new Matrix4x4();
            UInterface.ChangeBaselFaceModelShapeCoffFromImage(BaselFaceModel.m_NativeHandle, m_SrcTexture, w, h, vCount, bDebug,ref cameraMatrix);

            gameObject.GetComponent<BaselFaceModel>().ExternalUpdateShape();
            gameObject.GetComponent<BaselFaceModel>().ExternalUpdateTexture();

            
            //Project Vertex 
            if(m_PrjectVertexOrGameObject)
            {


                Vector3[] vertices = gameObject.GetComponent<MeshFilter>().mesh.vertices;

                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = cameraMatrix.MultiplyPoint(vertices[i]);
                }

                gameObject.GetComponent<MeshFilter>().mesh.vertices = vertices;


                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.position = Vector3.zero;
                gameObject.transform.localScale = Vector3.one;
            }
            //Project GameObject
            else
            {


                Quaternion rot = cameraMatrix.rotation;
                Vector3 trans = new Vector3(cameraMatrix.m03 / cameraMatrix.m33, cameraMatrix.m13 / cameraMatrix.m33, 0);
                Vector3 scale = new Vector3(1 / cameraMatrix.m33, 1 / cameraMatrix.m33, 1 / cameraMatrix.m33);

                gameObject.transform.rotation = rot;
                gameObject.transform.position = trans;
                gameObject.transform.localScale = scale;
            }

        }
    }

    List<Vector2> LoadLandmarkFromFile()
    {
        string fileLandmark = Application.dataPath + "/2D/Face/image_0010.pts";
        string fileMap = Application.dataPath + "/2D/Face/ibug_to_sfm.txt";


        List<Vector2> points = new List<Vector2>();

        foreach (string line in File.ReadLines(fileLandmark))
        {
            string[] xy = line.Split(' ');

            float x = Convert.ToSingle(xy[0]);
            float y = Convert.ToSingle(xy[1]);
            points.Add(new Vector2(x, y));
        }

        List<Vector2> pointsInMap = new List<Vector2>();
        string mapCode = "";
        foreach (string line in File.ReadLines(fileMap))
        {
            string[] kv = line.Split(' ');

            int key = Convert.ToInt32(kv[0]);
            int value = Convert.ToInt32(kv[1]);

            pointsInMap.Add(points[key-1]);

            mapCode += string.Format("<{0},{1}>,",key-1,value-1);
        }
        Debug.Log(mapCode);
        return points;
    }
}
