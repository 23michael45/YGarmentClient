using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.IO;
using System;

public class UploadPhoto : MonoBehaviour
{

    public string url = "https://m.showonme.com/fac/usr/upUsrPicFor3D";

    private Texture2D usrfacephoto;
    [Serializable]
    class JData
    {
        public int ret;
        public string retMsg;
        [Serializable]
        public class JInfo
        {
            public string meshFile;
            public string TextureFile;
        };

        public JInfo info;
    }

    private void Start()
    {
        string jstr = "{\"ret\":0,\"retMsg\":\"操作成功\",\"info\": {\"meshFile\":\"https://yjkj-0508.oss-cn-shenzhen.aliyuncs.com/FAC:553418fd01d14377bd6bc590cdcac1d5.tmp\",\"TextureFile\":\"https://yjkj-0508.oss-cn-shenzhen.aliyuncs.com/FAC:246855da2d3f4094a3db87edc53a6119.tmp\"}}";
        var jsondata = JsonUtility.FromJson<JData>(jstr);
        
        var info = jsondata.ret.ToString();
        var retMsg = jsondata.retMsg.ToString();

        var meshFile = jsondata.info.meshFile.ToString();
        Debug.Log(info);
        Debug.Log(retMsg);
        Debug.Log(meshFile);
    }

    public void uploadImgClick()
    {
        StartCoroutine(UploadPNG());

    }


    private IEnumerator UploadPNG()
    {

        usrfacephoto = Resources.Load("test") as Texture2D;   
        byte[] bytes = usrfacephoto.EncodeToJPG();
        print(usrfacephoto.width);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("header-name", "header content");
        



        WWWForm form = new WWWForm();
        form.AddField("name", "value");
        form.AddBinaryData("fileUpload", bytes, "test", "image/jpg");

        print(bytes);

        using (var w = UnityWebRequest.Post(url, form))
        {
            yield return w.SendWebRequest();

            if (w.isNetworkError)
            {
                   print(w.error);
                print("isNetworkError");
            }
            else if (w.isHttpError) {

                print(w.error);
                print("isHttpError");
            }
            else
            {
                print("Finished Uploading Screenshot");
                print(w.downloadHandler.text);



                string data = w.downloadHandler.text.ToString();
                print(data);

                getmodel(data);


            }


        }

    }



    void getmodel(string data ) {


        //string jstr = "{ \"ret\":0,\"retMsg\":\"操作成功\",\"info\":\"{\"meshFile\":\"https://yjkj-0508.oss-cn-shenzhen.aliyuncs.com/FAC:553418fd01d14377bd6bc590cdcac1d5.tmp\",\"TextureFile\":\"https://yjkj-0508.oss-cn-shenzhen.aliyuncs.com/FAC:246855da2d3f4094a3db87edc53a6119.tmp\"}\"}";
        //var jsondata = JsonUtility.FromJson(data);

        //print(jsondata);

        //var info = jsondata["info"].ToString();

        //Debug.Log(info);
    }










}




















