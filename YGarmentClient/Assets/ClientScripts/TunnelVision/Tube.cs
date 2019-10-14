using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public GameObject m_Prefab;
    public int m_Count = 10;
    public int m_Interval = 10;
    public float m_Dist = 10;
    // Start is called before the first frame update
    void Start()
    {

        Texture2D t = Resources.Load<Texture2D>("10");

        for (int i = 1; i< m_Count;i++)
        {
            GameObject gonew = GameObject.Instantiate(m_Prefab);
            gonew.SetActive(true);
            gonew.transform.parent = transform;
            gonew.transform.position = new Vector3( -i * m_Dist + 3*m_Dist,0,0);

            gonew.GetComponent<TubeSection>().m_Num = i * m_Interval;

            Vector2 scale = Vector2.one;
            scale.y = (i % 2 == 1) ? -1 : 1;

            gonew.GetComponent<TubeSection>().SetTexture(t, scale);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
