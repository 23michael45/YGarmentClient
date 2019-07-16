using DlibFaceLandmarkDetector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FitFace : MonoBehaviour
{

    public Texture2D m_SrcTexture;

    public bool m_bFit = false;
    FaceLandmarkDetector m_FaceLandmarkDetector;
    // Start is called before the first frame update
    IEnumerator Start()
    {


        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();


        string dlibShapePredictorFilePath = Application.dataPath + "/DlibFaceLandmarkDetector/StreamingAssets/sp_human_face_68.dat";


        m_FaceLandmarkDetector = new FaceLandmarkDetector(dlibShapePredictorFilePath);

    }


    private void Update()
    {
        if(m_bFit)
        {
            m_bFit = false;


            Fit(true);


        }
        
    }

    void Fit(bool usedlib = false)
    {
        m_FaceLandmarkDetector.SetImage(m_SrcTexture);

        List<Vector2> points = new List<Vector2>();
        if (usedlib)
        {
            List<Rect> detectResult = m_FaceLandmarkDetector.Detect();


            foreach (var rect in detectResult)
            {
                points = m_FaceLandmarkDetector.DetectLandmark(rect);

               

                break;
            }
        }
        else
        {
            points = LoadLandmarkFromFile();
        }


        if (points.Count > 0)
        {
            int w = m_SrcTexture.width;
            int h = m_SrcTexture.height;
            Debug.Log(string.Format("w {0} h {1}", w, h));

            UInterface.ChangeBaselFaceModelShapeCoffFromLandmark(BaselFaceModel.m_NativeHandle, points, w, h);

            gameObject.GetComponent<BaselFaceModel>().ExternalUpdateShape();
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
