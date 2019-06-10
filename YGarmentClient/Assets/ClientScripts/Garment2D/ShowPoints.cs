using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPoints : MonoBehaviour
{
    public GameObject m_RedPoint;
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


}
