using System.Collections;
using System.Collections.Generic;
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
}
