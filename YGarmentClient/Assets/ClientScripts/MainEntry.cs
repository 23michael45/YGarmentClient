using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEntry : MonoBehaviour
{
    public Texture2D m_Texture;
    public Texture2D m_DstTexture;

    public Vector2[] m_Contours;

    [SerializeField] Material lineMat;

    int DrawCount = 0;

    public int Speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        UInterface uinterface = new UInterface();
        m_DstTexture = uinterface.DetectContoursImage(m_Texture);

        m_Contours = uinterface.DetectContours(m_DstTexture);

        StartCoroutine(AddCount());

    }
    IEnumerator AddCount()
    {
        while (DrawCount < m_Contours.Length)
        {

            yield return new WaitForSeconds(.01f);
            Debug.Log(DrawCount);
            DrawCount+= Speed;
        }
    }


    void OnRenderObject()
    {
        if (m_Contours != null)
        {
            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);
            lineMat.SetColor("_Color", Color.white);
            //lineMat.SetColor("_Color", new Color(
            //     (float)Random.Range(0, 255),
            //     (float)Random.Range(0, 255),
            //     (float)Random.Range(0, 255)));


            lineMat.SetPass(0);
            GL.Begin(GL.LINES);


            int len = Mathf.Min(m_Contours.Length - 1, DrawCount);
            for (int i = 0, n = len; i < n; i++)
            {
                GL.Vertex(m_Contours[i]); GL.Vertex(m_Contours[i + 1]);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}
