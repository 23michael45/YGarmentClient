using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using HDF.PInvoke;
public class LoadModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        H5F.create("1.h5", 0);
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
