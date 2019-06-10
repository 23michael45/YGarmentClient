using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothDeform : MonoBehaviour
{

    Mesh m_Mesh;
    public MeshFilter m_Filter;

    public Transform m_PPoints;
    public Transform m_QPoints;

    public GameObject m_Wrapped;
    
    // Start is called before the first frame update
    void Start()
    {
        if(m_Filter != null)
        {
            m_Mesh = m_Filter.mesh;
            MeshDeformation();

        }

    }


    void MeshDeformation()
    {
        Vector2[] ps = new Vector2[m_PPoints.childCount];
        for (int i = 0; i < ps.Length; i++)
        {
            Vector3 p = m_PPoints.GetChild(i).position;
            ps[i] = new Vector2(p.x, p.y);
        }


        Vector2[] qs = new Vector2[m_QPoints.childCount];
        for (int i = 0; i < qs.Length; i++)
        {
            Vector3 q = m_QPoints.GetChild(i).position;
            qs[i] = new Vector2(q.x, q.y);
        }


        Vector2[] vs = new Vector2[m_Mesh.vertices.Length];
        for (int i = 0; i < m_Mesh.vertices.Length; i++)
        {
            Vector3 v = m_Mesh.vertices[i];
            vs[i] = new Vector2(v.x, v.y);
        }


        UInterface uinterface = new UInterface();
        //Vector2[] lvs = uinterface.MeshDeformation(ps, qs, vs, m_Mesh.triangles);
        Vector2[] lvs = uinterface.ARAPDeformation(ps, qs, vs, m_Mesh.triangles);

        Mesh newmesh = Instantiate(m_Mesh);

        if (lvs != null)
        {

            Vector3[] newvertices = new Vector3[lvs.Length];
            for (int i = 0; i < lvs.Length; i++)
            {
                Vector2 lv = lvs[i];
                newvertices[i] = new Vector3(lv.x, lv.y, 0);
            }

            newmesh.vertices = newvertices;


            //string path = Path.Combine(Application.dataPath, "3D/CoatMesh_Wrapped");
            //byte[] bytes = MeshSerializer.WriteMesh(newmesh, true);
            //File.WriteAllBytes(path, bytes);
            m_Wrapped.GetComponent<MeshFilter>().mesh = newmesh;
        }



    }

}
