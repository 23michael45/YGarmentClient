using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class UDynamicInterface : MonoBehaviour
{
    [DllImport("YGarmentLib")]
    private static extern IntPtr GetRenderEventFunc();

    private void Start()
    {
        IntPtr func = GetRenderEventFunc();
        Debug.Log(string.Format("UDynamicInterface GetRenderEventFunc:{0}",func.ToInt64()));
    }
}
