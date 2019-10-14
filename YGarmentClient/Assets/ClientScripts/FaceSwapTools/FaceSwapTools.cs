using DlibFaceLandmarkDetector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FaceSwapTools : MonoBehaviour
{
    string m_INIFile;

    FaceLandmarkDetector m_FaceLandmarkDetector;
    public Texture2D m_Texture;

    public Transform m_ParentTransform;
    public Image m_Prefab;

    INIParser m_INIParser = new INIParser();


    PointerEventData m_PointerEventData;

    public GraphicRaycaster m_Raycaster;
    public EventSystem m_EventSystem;


    int m_CurrentIndex;
    public RawImage m_RawImage;
    public Button m_SaveBtn;
    public Toggle[] m_Toggles;
    public GameObject[] m_Marks;
    public string[] m_PointIndices = new string[3];


    public Texture2D LoadImage(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
    // Start is called before the first frame update
    void Start()
    {
        var tex = LoadImage(Application.dataPath + "/bk.jpg");

        if(tex)
        {
            m_Texture = tex;
        }
        m_RawImage.texture = m_Texture;
        m_RawImage.SetNativeSize();

       m_INIFile = Application.dataPath + "/AppConfig.ini"; 

        m_SaveBtn.onClick.AddListener(OnSave);
        m_Toggles[0].onValueChanged.AddListener(OnToggle1);
        m_Toggles[1].onValueChanged.AddListener(OnToggle2);
        m_Toggles[2].onValueChanged.AddListener(OnToggle3);
        

        string dlibShapePredictorFilePath = Application.dataPath + "/DlibFaceLandmarkDetector/StreamingAssets/sp_human_face_68.dat";
        m_FaceLandmarkDetector = new FaceLandmarkDetector(dlibShapePredictorFilePath);



        m_FaceLandmarkDetector.SetImage(m_Texture);
        List<Rect> detectResult = m_FaceLandmarkDetector.Detect();
        foreach (var rect in detectResult)
        {
            var points = m_FaceLandmarkDetector.DetectLandmark(rect);

            int i = 1;
            foreach(var point in points)
            {
                GameObject gonew = GameObject.Instantiate(m_Prefab.gameObject);
                gonew.SetActive(true);
                gonew.transform.parent = m_ParentTransform;
                gonew.name = i.ToString();


                RectTransform rectTrans = gonew.GetComponent<RectTransform>();
                rectTrans.anchoredPosition3D = new Vector3(point.x - m_Texture.width / 2, m_Texture.height - point.y - m_Texture.height / 2,1);
                rectTrans.localScale = Vector2.one;

                i++;
            }
            break;
        }


        LoadINI();
    }

    private void OnDestroy()
    {
        m_INIParser.Close();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {

                if (result.gameObject.layer == LayerMask.NameToLayer("UIRaycast"))
                {
                    Debug.Log("Hit:" + result.gameObject.name);

                    m_Marks[m_CurrentIndex].GetComponent<RectTransform>().anchoredPosition3D = result.gameObject.GetComponent<RectTransform>().anchoredPosition3D;

                    m_PointIndices[m_CurrentIndex] = result.gameObject.name;
                }
                break;
            }
        }
    }

    void OnToggle1(bool b)
    {
        if(b)
        {
            m_CurrentIndex = 0;

        }
    }
    void OnToggle2(bool b)
    {
        if (b)
        {
            m_CurrentIndex = 1;

        }
    }
    void OnToggle3(bool b)
    {
        if (b)
        {
            m_CurrentIndex = 2;

        }
    }

    void LoadINI()
    {
        m_INIParser.Open(m_INIFile);
        m_PointIndices[0] = m_INIParser.ReadValue("points", "1", "37");
        m_PointIndices[1] = m_INIParser.ReadValue("points", "2", "46");
        m_PointIndices[2] = m_INIParser.ReadValue("points", "3", "9");


        for (int i = 0; i < m_PointIndices.Length;i++)
        {

            for (int j = 0; j < m_ParentTransform.childCount; j++)
            {
                Transform t = m_ParentTransform.GetChild(j);
           
                if(t.gameObject.name ==  m_PointIndices[i])
                {
                    Debug.Log(t.gameObject.name);

                    m_Marks[i].GetComponent<RectTransform>().anchoredPosition3D = t.gameObject.GetComponent<RectTransform>().anchoredPosition3D;
                    break;
                }
            }
            
        }

        m_INIParser.Close();
    }

    void OnSave()
    {

        m_INIParser.Open(m_INIFile);
        m_INIParser.WriteValue("points", "1", m_PointIndices[0]);
        m_INIParser.WriteValue("points", "2", m_PointIndices[1]);
        m_INIParser.WriteValue("points", "3", m_PointIndices[2]);

        m_INIParser.Close();
    }
}
