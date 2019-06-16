using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class ClothLoader : MonoBehaviour
{
    public bool m_bLoad = false;
    public string m_MeshPath = "";

    Mesh m_Mesh;

    // Start is called before the first frame update
    void Load()
    {
        string path = Path.Combine(Application.dataPath, m_MeshPath);
        if (File.Exists(path) == true)
        {
            byte[] bytes = File.ReadAllBytes(path);
            m_Mesh = MeshSerializer.ReadMesh(bytes);

            gameObject.GetComponent<MeshFilter>().mesh = m_Mesh;

        }
        else
        {
            Debug.LogError("Mesh Not Found");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bLoad)
        {
            m_bLoad = false;
            Load();
        }
        
    }
}
