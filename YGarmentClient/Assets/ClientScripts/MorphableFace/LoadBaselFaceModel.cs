using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;


using hid_t = System.Int64;
using System.Runtime.InteropServices;
using System.Linq;

public class LoadBaselFaceModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadH5();
    }
    bool LoadH5()
    { 
        //string h5file = "D:/DevelopProj/Yuji/FaceModel/model2017-1_bfm_nomouth.h5";

        //hid_t dataSetId = 0;
        //hid_t dataSpaceId = 0;
        //hid_t typeId = 0;

        //hid_t fileId = H5F.open(h5file, H5F.ACC_RDONLY);

        //try
        //{
        //    int recordLength = 10;
        //    string[] datasetOut = null;
        //    string datasetPath = "shape";
        //    List<string> dataset = new List<string>();
        //    byte[] datasetnames;


        //    dataSetId = H5D.open(fileId, datasetPath);
        //    dataSpaceId = H5D.get_space(dataSetId);
        //    typeId = H5T.copy(H5T.C_S1);
        //    H5T.set_size(typeId, new IntPtr(recordLength));

        //    int rank = H5S.get_simple_extent_ndims(dataSpaceId);
        //    ulong[] dims = new ulong[rank];
        //    ulong[] maxDims = new ulong[rank];
        //    H5S.get_simple_extent_dims(dataSpaceId, dims, maxDims);
        //    byte[] dataBytes = new byte[dims[0] * (ulong)recordLength];

        //    GCHandle pinnedArray = GCHandle.Alloc(dataBytes, GCHandleType.Pinned);
        //    H5D.read(dataSetId, typeId, H5S.ALL, H5S.ALL, H5P.DEFAULT, pinnedArray.AddrOfPinnedObject());
        //    pinnedArray.Free();

        //    for (int i = 0; i < (int)(dims[0]); i++)
        //    {
        //        byte[] slice = dataBytes.Skip<byte>(i * recordLength).Take<byte>(recordLength).ToArray<byte>();
        //        var content = System.Text.Encoding.ASCII.GetString(slice).Trim();
        //        dataset.Add(content);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    return false;
        //}
        //finally
        //{
        //    if (typeId != 0) H5T.close(typeId);
        //    if (dataSpaceId != 0) H5S.close(dataSpaceId);
        //    if (dataSetId != 0) H5D.close(dataSetId);
        //}



        return true;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

public interface IInterface
{
    string open(string file,int mode);
}
