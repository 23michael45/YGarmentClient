using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCalibration : MonoBehaviour
{
    Camera m_Camera;

    public Matrix4x4 originalProjection;
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = gameObject.GetComponent<Camera>();

        var w2c = m_Camera.worldToCameraMatrix;
        Debug.Log(w2c);

        var c2w = m_Camera.cameraToWorldMatrix;
        Debug.Log(c2w);



        Debug.Log(c2w * w2c);

        var proj = m_Camera.projectionMatrix;
        Debug.Log(proj);

        originalProjection = proj;

        var wpos = new Vector4(0.5f, 0.5f, 10f, 1f);
        //var wpos = new Vector4(0.0f, 0.0f, 1f, 1f);

        var cpos = w2c.MultiplyPoint(wpos);


        var ipos = originalProjection * w2c* wpos;
        var iposx = (originalProjection * w2c).MultiplyPoint(wpos);
        Debug.Log(ipos);

        var spos = new Vector3(ipos.x / ipos.w, ipos.y / ipos.w , ipos.z / ipos.w);
        spos = spos * +0.5f + Vector3.one * 0.5f;
        Debug.Log(spos);
        var rwpos = m_Camera.ViewportToWorldPoint(spos);
        Debug.Log(rwpos);
        GameObject gonew = GameObject.Instantiate(prefab);
        gonew.SetActive(true);
        gonew.transform.position = rwpos;

    }

    // Update is called once per frame
    void Update()
    {
        //Matrix4x4 p = originalProjection;
        //p.m01 += Mathf.Sin(Time.time * 1.2F) * 0.1F;
        //p.m10 += Mathf.Sin(Time.time * 1.5F) * 0.1F;
        m_Camera.projectionMatrix = originalProjection;
    }
}
