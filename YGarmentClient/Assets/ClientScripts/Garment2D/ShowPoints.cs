using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShowPoints : MonoBehaviour
{
    public GameObject m_RedPoint;

    public Transform m_ResortTarget;
    public bool m_bResort = false;
    // Start is called before the first frame update
    void Start()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in children)
        {
            GameObject pt = GameObject.Instantiate(m_RedPoint);
            pt.transform.parent = t;

            pt.transform.rotation = Quaternion.identity;
            pt.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    private void Update()
    {
        if(m_bResort == true)
        {
            Resort();
            m_bResort = false;
        }
    }

    void Resort()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            foreach (Transform t in children)
            {
                if(t.gameObject.name == i.ToString())
                {

                    GameObject pt = GameObject.Instantiate(m_RedPoint);
                    pt.transform.parent = m_ResortTarget;
                    pt.transform.rotation = Quaternion.identity;
                    pt.transform.localPosition = t.localPosition;

                    pt.gameObject.name = i.ToString();
                }
            }
        }

        
    }


}
