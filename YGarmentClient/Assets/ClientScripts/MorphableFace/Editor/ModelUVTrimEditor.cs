using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModelUVTrim))]
public class ModelUVTrimEditor : Editor
{
    string mSavePath;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ModelUVTrim parentObj = (ModelUVTrim)target;


        EditorGUILayout.Separator();

        if (GUILayout.Button("Trim UV Vertices", EditorStyles.miniButtonRight))
        {
            parentObj.Trim();
        }



        EditorGUILayout.LabelField("Save Mesh With UV to FBX");
        mSavePath = EditorGUILayout.TextField(mSavePath);
        EditorGUILayout.Separator();
        if (GUILayout.Button("Save FBX", EditorStyles.miniButtonRight))
        {
            parentObj.Save(mSavePath);
        }

    }
}
