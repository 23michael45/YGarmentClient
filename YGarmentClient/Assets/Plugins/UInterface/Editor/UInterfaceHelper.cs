using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class UInterfaceHelper : MonoBehaviour
{

    [MenuItem("Dll Library/Load")]
    public static void LoadLibrary()
    {
        UInterface.LoadLibrary();
    }
    [MenuItem("Dll Library/Free")]
    public static void FreeLibrary()
    {
        UInterface.FreeLibrary();
    }
    [MenuItem("Dll Library/Free All")]
    public static void FreeAllLibrary()
    {
        UInterface.FreeAllLibrary();
    }

    [DllImport("YGarmentLib")]
    private static extern IntPtr GetRenderEventFunc();
    [MenuItem("Dll Library/Load Render Plugin")]
    public static void IssuePluginEvent()
    {
        GetRenderEventFunc();
    }
}
