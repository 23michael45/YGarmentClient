using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DeleteChildrenWithName : MonoBehaviour
{
    public string m_ObjectName;
    public bool m_bDelete = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bDelete)
        {
            m_bDelete = false;
            DeleteChildren();
        }
    }
    void DeleteChildren()
    {
        List<Transform> listToDel = new List<Transform>();
        foreach(Transform t in transform.GetComponentsInChildren<Transform>())
        {
            if(t.gameObject.name == m_ObjectName)
            {
                listToDel.Add(t);
            }
        }

        foreach(var t in listToDel)
        {

            GameObject.DestroyImmediate(t.gameObject);
        }
    }
}
