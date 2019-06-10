using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPointFlatten : MonoBehaviour
{
    public Color m_Color;
    // Start is called before the first frame update
    void Start()
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        foreach(var t in children)
        {
            if(t.gameObject.GetComponent<MeshFilter>() != null)
            {
                t.parent = transform;
                t.gameObject.GetComponent<MeshRenderer>().material.color = m_Color;
            }
            else if(t == transform)
            {
                continue;
            }
            else
            {
                GameObject.Destroy(t.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
