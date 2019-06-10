using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanModel : MonoBehaviour
{
    public Transform m_Bones;
    public Transform m_Contour;
    public Transform m_HumanModel;

    public GameObject m_RedPoint;
    public GameObject m_GreenPoint;

    public Transform m_BonePointsContainer;
    public Transform m_ContourPointsContainer;

    public float m_PointZ = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        AddBonesPoints();
        AddContourPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddBonesPoints()
    {
        Transform[] children = m_Bones.GetComponentsInChildren<Transform>();
        foreach(Transform t in children)
        {
            GameObject pt = GameObject.Instantiate(m_RedPoint);
            pt.transform.parent = t;

            pt.transform.rotation = Quaternion.identity;
            pt.transform.localPosition = new Vector3(0, 0, 0);
        }

    }
    public void AddContourPoints()
    {
        Transform[] children = m_Contour.GetComponentsInChildren<Transform>();
        foreach (Transform t in children)
        {
            GameObject pt = GameObject.Instantiate(m_GreenPoint);
            pt.transform.parent = t;

            pt.transform.rotation = Quaternion.identity;
            pt.transform.localPosition = new Vector3(0,0,0);
        }

    }
}
