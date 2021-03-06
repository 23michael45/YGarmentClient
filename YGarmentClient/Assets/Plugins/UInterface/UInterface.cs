﻿using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

[StructLayout(LayoutKind.Sequential)]
public struct Point2D
{
    public float x;
    public float y;
}


public static class UInterface
{
    static IntPtr m_PluginDll;





#if UNITY_EDITOR
    static class NativeMethods
    {
        [DllImport("kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
    }
    
    public static void GetProcAddress<T>(ref T func,string funcname) where T : Delegate
    {
        IntPtr pAddressOfFunctionToCall = NativeMethods.GetProcAddress(m_PluginDll, funcname);

        UnityEngine.Debug.Log(string.Format("Load Function: {0} Addr: {1}", funcname, pAddressOfFunctionToCall.ToInt64()));

        func = (T)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(T));

    }


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr Native_CreateBaselFaceModel_Delegate([MarshalAs(UnmanagedType.LPStr)] string modelFileName, [MarshalAs(UnmanagedType.LPStr)] string shapeFileName, ref IntPtr verBuffer,ref int vLen,ref IntPtr triBuffer,ref int tLen,ref int pcaDimCount,ref IntPtr colBuffer, ref int colLen);
    static Native_CreateBaselFaceModel_Delegate Native_CreateBaselFaceModel;


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Native_DestroyBaselFaceModel_Delegate(IntPtr handle);
    static Native_DestroyBaselFaceModel_Delegate Native_DestroyBaselFaceModel;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Native_SetMeshVerticesMemoryAddr_Delegate(IntPtr handle, IntPtr verBuffer, int verLen, IntPtr uvBuffer, int uvLen, IntPtr colBuffer, int colLen,bool saveh5);
    static Native_SetMeshVerticesMemoryAddr_Delegate Native_SetMeshVerticesMemoryAddr;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Native_SetTextureMemoryAddr_Delegate(IntPtr handle, IntPtr texBuffer,int width,int height);
    static Native_SetTextureMemoryAddr_Delegate Native_SetTextureMemoryAddr;


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Native_SaveBFMH5_Delegate(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string h5FileName);
    static Native_SaveBFMH5_Delegate Native_SaveBFMH5;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Native_ChangeBaselFaceModelCoff_Delegate(IntPtr handle, IntPtr coffBuffer,int coffLen);
    static Native_ChangeBaselFaceModelCoff_Delegate Native_ChangeBaselFaceModelShapeCoff;
    static Native_ChangeBaselFaceModelCoff_Delegate Native_ChangeBaselFaceModelColorCoff;


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Native_ChangeBaselFaceModelCoff2_Delegate(IntPtr handle, IntPtr shapeCoffBuffer, int sLen,IntPtr expCoffBuffer, int eLen);
    static Native_ChangeBaselFaceModelCoff2_Delegate Native_ChangeBaselFaceModelExpressionCoff;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Native_ChangeBaselFaceModelShapeCoffFromLandmark_Delegate(IntPtr handle, IntPtr landmarkBuffer, int coffLen,int width,int height, int bfm_point_count, bool bDebug);
    static Native_ChangeBaselFaceModelShapeCoffFromLandmark_Delegate Native_ChangeBaselFaceModelShapeCoffFromLandmark;


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Native_ChangeBaselFaceModelShapeCoffFromImage_Delegate(IntPtr handle, IntPtr texData, int width, int height, int bfm_point_count, bool bDebug,IntPtr cameraCoff);
    static Native_ChangeBaselFaceModelShapeCoffFromImage_Delegate Native_ChangeBaselFaceModelShapeCoffFromImage;


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Native_GetContoursMesh_Delegate(IntPtr texData, int width, int height, int intervalX, int intervalY, float thresh, ref int size, ref IntPtr pVertices, ref int vsize, ref IntPtr pTriangles, ref int tsize);
    static Native_GetContoursMesh_Delegate Native_GetContoursMesh;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Native_GetContoursMeshByPoints_Delegate(IntPtr plist, int pl, int width, int height, int intervalX, int intervalY, ref int size, ref IntPtr pVertices, ref int vsize, ref IntPtr pTriangles, ref int tsize);
    static Native_GetContoursMeshByPoints_Delegate Native_GetContoursMeshByPoints;


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Native_SetUnityTexturePtr_Delegate([MarshalAs(UnmanagedType.LPStr)] string texName, IntPtr ptr);
    static Native_SetUnityTexturePtr_Delegate Native_SetUnityTexturePtr;

    

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void Native_FillUnityData_Delegate([MarshalAs(UnmanagedType.LPStr)] string fileName);
    static Native_FillUnityData_Delegate Native_FillUnityData;
    

    
#else
    

    [DllImport("YGarmentLib")]
    private static extern void Native_SetMeshMemAddr(IntPtr verBuffer, int len);

    [DllImport("YGarmentLib")]
    private static extern bool Native_SetBFMCoff(IntPtr coffBuffer, int len);

    [DllImport("YGarmentLib")]
    private static extern void GetContoursMesh(IntPtr texData, int width, int height,int intervalX,int intervalY, float thresh, ref int size, ref IntPtr pVertices, ref int vsize, ref IntPtr pTriangles,ref int tsize);

    [DllImport("YGarmentLib")]
    private static extern void GetContoursMeshByPoints(IntPtr plist, int pl, int width, int height, int intervalX, int intervalY, float thresh, ref int size, ref IntPtr pVertices, ref int vsize, ref IntPtr pTriangles, ref int tsize);


#endif
    public static void LoadFunctions()
    {

        GetProcAddress(ref Native_CreateBaselFaceModel, "CreateBaselFaceModel");
        GetProcAddress(ref Native_DestroyBaselFaceModel, "DestroyBaselFaceModel");

        GetProcAddress(ref Native_ChangeBaselFaceModelShapeCoff, "ChangeBaselFaceModelShapeCoff");
        GetProcAddress(ref Native_ChangeBaselFaceModelExpressionCoff, "ChangeBaselFaceModelExpressionCoff");
        GetProcAddress(ref Native_ChangeBaselFaceModelColorCoff, "ChangeBaselFaceModelColorCoff");
        GetProcAddress(ref Native_ChangeBaselFaceModelShapeCoffFromLandmark, "ChangeBaselFaceModelShapeCoffFromLandmark");
        GetProcAddress(ref Native_ChangeBaselFaceModelShapeCoffFromImage, "ChangeBaselFaceModelShapeCoffFromImage");

        GetProcAddress(ref Native_SetMeshVerticesMemoryAddr, "SetMeshVerticesMemoryAddr");
        GetProcAddress(ref Native_SaveBFMH5, "SaveBFMH5");
        GetProcAddress(ref Native_SetTextureMemoryAddr, "SetTextureMemoryAddr");

        GetProcAddress(ref Native_GetContoursMesh, "GetContoursMesh");
        GetProcAddress(ref Native_GetContoursMeshByPoints, "GetContoursMeshByPoints");

        GetProcAddress(ref Native_SetUnityTexturePtr, "SetUnityTexturePtr");
        GetProcAddress(ref Native_FillUnityData, "FillUnityData");
    }
    public static void LoadLibrary()
    {
#if UNITY_EDITOR
        if(m_PluginDll != IntPtr.Zero)
        {
            UnityEngine.Debug.Log("Library alreadly Loaded");

            return;
        }

        string sCurrentPath = Directory.GetCurrentDirectory();
        UnityEngine.Debug.Log("sCurrentPath:" + sCurrentPath);

        UnityEngine.Debug.Log("Load Library");
        string sPluginPath = Path.Combine(Application.dataPath, @"Plugins");
        string dllPath = Path.Combine(sPluginPath, @"YGarmentLib.dll");
        if (File.Exists(dllPath))
        {
            Directory.SetCurrentDirectory(sPluginPath);
            m_PluginDll = NativeMethods.LoadLibrary(dllPath);

            if (m_PluginDll != IntPtr.Zero)
            {
                LoadFunctions();


            }
            else
            {
                UnityEngine.Debug.LogError("Load Library Failed : " + Marshal.GetLastWin32Error().ToString());
            }

            Directory.SetCurrentDirectory(sCurrentPath);
        }
        else
        {
            UnityEngine.Debug.LogError("Dll File Not Exist");

        }
#endif
    }
    public static void FreeLibrary()
    {
#if UNITY_EDITOR
        if (m_PluginDll == IntPtr.Zero)
        {
            return;
        }
        bool result = NativeMethods.FreeLibrary(m_PluginDll);
        if(result)
        {

            m_PluginDll = IntPtr.Zero;
            UnityEngine.Debug.Log("Free Library");
        }
        else
        {

            UnityEngine.Debug.Log("Free Library Failed");
        }
#endif
    }
    public static void FreeAllLibrary()
    {
        FreeLibrary();
        List<string> freelist = new List<string>();
        freelist.Add("YGarmentLib");

        foreach (ProcessModule mod in Process.GetCurrentProcess().Modules)
        {
            foreach(string file in freelist)
            {
                if (mod.FileName.Contains(file))
                {
                    bool b = NativeMethods.FreeLibrary(mod.BaseAddress);
                    if(b)
                    {
                        UnityEngine.Debug.Log("Free Sucess:" + mod.FileName);
                        break;
                    }
                    else
                    {
                        UnityEngine.Debug.Log("Free Failed:" + mod.FileName);

                    }
                }
            }
          
        }
    }

    [DllImport("YGarmentLib")]
    private static extern IntPtr MeshDeformation(ref int size,IntPtr plist,int pl,IntPtr qlist,int ql,IntPtr vlist,int vl,IntPtr tlist,int tl,int type);
    [DllImport("YGarmentLib")]
    private static extern IntPtr ARAPDeformation(ref int size, IntPtr plist, int pl, IntPtr qlist, int ql, IntPtr vlist, int vl, IntPtr tlist, int tl, int iterCount);

    [DllImport("YGarmentLib")]
    private static extern void Release(IntPtr p);

    [DllImport("YGarmentLib")]
    private static extern IntPtr DetectContours(IntPtr texData, int width, int height,float thresh,ref int size);
    [DllImport("YGarmentLib")]
    private static extern IntPtr DetectContoursImage(IntPtr texData, int width, int height);






    static Point2D[] Vector2IntPtr(Vector2[] v)
    {
        Point2D[] p = new Point2D[v.Length];
        for(int i = 0; i< v.Length;i++)
        {
            p[i].x = v[i].x;
            p[i].y = v[i].y;
        }




        //IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Point2D)) * p.Length);
        //Marshal.StructureToPtr(p, ptr, false);

        return p;
    }

    static Vector2[] IntPtr2Vector(IntPtr ptr,int size, float scalex = 1,float scaley = 1)
    {

        Point2D[] p = new Point2D[size];

        // memory layout
        // |float|float|float|float|float|float|float|float|float|float..
        // |   ExamplePoint0 |   ExamplePoint1 |   ExamplePoint2 |
        int offset = 0;
        int pointSize = Marshal.SizeOf(typeof(Point2D));
        for (int i = 0; i < size; i++)
        {
            p[i] = (Point2D)Marshal.PtrToStructure(new IntPtr(ptr.ToInt64() + offset), typeof(Point2D));
            offset += pointSize;
        }


        Vector2[] v = new Vector2[p.Length];
        for (int i = 0; i < v.Length; i++)
        {
            v[i].x = p[i].x * scalex;
            v[i].y = p[i].y * scaley;

            //Debug.Log("Ret V:" + v[i].x + "   " + v[i].y);
        }
        return v;
    }

    static Vector3[] IntPtr2Vector3(IntPtr ptr, int size, float scalex = 1, float scaley = 1)
    {

        Point2D[] p = new Point2D[size];

        // memory layout
        // |float|float|float|float|float|float|float|float|float|float..
        // |   ExamplePoint0 |   ExamplePoint1 |   ExamplePoint2 |
        int offset = 0;
        int pointSize = Marshal.SizeOf(typeof(Point2D));
        for (int i = 0; i < size; i++)
        {
            p[i] = (Point2D)Marshal.PtrToStructure(new IntPtr(ptr.ToInt64() + offset), typeof(Point2D));
            offset += pointSize;
        }


        Vector3[] v = new Vector3[p.Length];
        for (int i = 0; i < v.Length; i++)
        {
            v[i].x = p[i].x * scalex;
            v[i].y = p[i].y * scaley;
            v[i].z = 0;

            //Debug.Log("Ret V:" + v[i].x + "   " + v[i].y);
        }
        return v;
    }
    static int[] IntPtr2IntArray(IntPtr ptr, int size)
    {

        int[] arr = new int[size];

        // memory layout
        // |float|float|float|float|float|float|float|float|float|float..
        // |   ExamplePoint0 |   ExamplePoint1 |   ExamplePoint2 |
        int offset = 0;
        int pointSize = Marshal.SizeOf(typeof(int));
        for (int i = 0; i < size; i++)
        {
            arr[i] = (int)Marshal.PtrToStructure(new IntPtr(ptr.ToInt64() + offset), typeof(int));
            offset += pointSize;
        }
        
        return arr;
    }




    public static Vector2[] MeshDeformation(Vector2[] p, Vector2[] q, Vector2[] v,int[] t)
    {
        int size = 0;

        
        var ps = Vector2IntPtr(p);
        GCHandle ph = GCHandle.Alloc(ps, GCHandleType.Pinned);
        IntPtr PPtr = ph.AddrOfPinnedObject();

        var qs = Vector2IntPtr(q);
        GCHandle qh = GCHandle.Alloc(qs, GCHandleType.Pinned);
        IntPtr QPtr = qh.AddrOfPinnedObject();

        var vs = Vector2IntPtr(v);
        GCHandle vh = GCHandle.Alloc(vs, GCHandleType.Pinned);
        IntPtr VPtr = vh.AddrOfPinnedObject();

        GCHandle th = GCHandle.Alloc(t, GCHandleType.Pinned);
        IntPtr TPtr = th.AddrOfPinnedObject();

        IntPtr ptr = MeshDeformation(ref size, PPtr, p.Length, QPtr, q.Length, VPtr, v.Length,TPtr,t.Length,0);

        ph.Free();
        qh.Free();
        vh.Free();
        th.Free();

        Vector2[] varray = IntPtr2Vector(ptr,size);


        Release(ptr);

        return varray;

    }


    public static Vector2[] ARAPDeformation(Vector2[] p, Vector2[] q, Vector2[] v, int[] t)
    {
        int size = 0;


        var ps = Vector2IntPtr(p);
        GCHandle ph = GCHandle.Alloc(ps, GCHandleType.Pinned);
        IntPtr PPtr = ph.AddrOfPinnedObject();

        var qs = Vector2IntPtr(q);
        GCHandle qh = GCHandle.Alloc(qs, GCHandleType.Pinned);
        IntPtr QPtr = qh.AddrOfPinnedObject();

        var vs = Vector2IntPtr(v);
        GCHandle vh = GCHandle.Alloc(vs, GCHandleType.Pinned);
        IntPtr VPtr = vh.AddrOfPinnedObject();

        GCHandle th = GCHandle.Alloc(t, GCHandleType.Pinned);
        IntPtr TPtr = th.AddrOfPinnedObject();

        IntPtr ptr = ARAPDeformation(ref size, PPtr, p.Length, QPtr, q.Length, VPtr, v.Length, TPtr, t.Length, 30);

        ph.Free();
        qh.Free();
        vh.Free();
        th.Free();

        Vector2[] varray = IntPtr2Vector(ptr, size);


        Release(ptr);

        return varray;

    }


    public static unsafe Texture2D DetectContoursImage(Texture2D texData)
    {
        byte[] data = texData.GetRawTextureData();
        UnityEngine.Debug.Log(data.Length);
        Color32[] texDataColor = texData.GetPixels32();
        //Pin Memory
        fixed (Color32* p = (texDataColor))
        {
            IntPtr pImageData = DetectContoursImage((IntPtr)p, texData.width, texData.height);

            if (pImageData != IntPtr.Zero)
            {
                //Point2D[] p = (Point2D[])Marshal.PtrToStructure(pImageData, typeof(Point2D));
                int arraySize = texData.width * texData.height * 4;
                byte[] rdata = new byte[arraySize];
                Marshal.Copy(pImageData, rdata, 0, arraySize);
                Marshal.FreeCoTaskMem(pImageData);

                Texture2D tex = new Texture2D(texData.width, texData.height, TextureFormat.RGBA32, false);
                tex.LoadRawTextureData(rdata);
                tex.Apply();

                return tex;
            }
        }
        return null;
    }

    public static unsafe Vector2[] DetectContours(Texture2D texData)
    {
        byte[] data = texData.GetRawTextureData();
        UnityEngine.Debug.Log(data.Length);
        Color32[] texDataColor = texData.GetPixels32();
        //Pin Memory
        fixed (Color32* p = (texDataColor))
        {
            int size = 0;
            IntPtr pData = DetectContours((IntPtr)p, texData.width, texData.height,100,ref size);
            UnityEngine.Debug.Log("Contours Size:" + size);
            if (pData != IntPtr.Zero)
            {
                return IntPtr2Vector(pData, size);
                
            }
        }
        return null;
    }


    public static unsafe Mesh GetContoursMesh(Texture2D texData)
    {


        byte[] data = texData.GetRawTextureData();
        UnityEngine.Debug.Log(data.Length);
        Color32[] texDataColor = texData.GetPixels32();
        fixed (Color32* p = (texDataColor))
        {
            int size = 0;
            int vsize = 0;
            int tsize = 0;

            IntPtr pVer = IntPtr.Zero;
            IntPtr pTri = IntPtr.Zero;
            Native_GetContoursMesh((IntPtr)p, texData.width, texData.height,50,50, 100, ref size, ref pVer,ref vsize, ref pTri,ref tsize);

            Mesh m = new Mesh();

            var vertices = IntPtr2Vector3(pVer, vsize);

            for (int i = 0; i < vertices.Length; i++)
            {
                var temp = vertices[i];
                vertices[i] = new Vector3(temp.x / texData.width - 0.5f, temp.y / texData.height - 0.5f, 0);
            }



            var triangles = IntPtr2IntArray(pTri, tsize);

            var uvs = IntPtr2Vector(pVer, vsize, 1f / texData.width, 1f / texData.height);


            m.vertices = vertices;


            for(int i = 0; i< triangles.Length/3;i++)
            {
                var temp = triangles[i * 3 + 2];
                triangles[i * 3 + 2] = triangles[i * 3 + 1];
                triangles[i * 3 + 1] = temp;
            }
            m.triangles = triangles;
            m.uv = uvs;

            Release(pVer);
            Release(pTri);

            return m;
        }

    }

    public static unsafe Mesh GetContoursMeshByPoints(List<Vector2> points, int width, int height,int intervalX,int intervalY)
    {


        var ps = Vector2IntPtr(points.ToArray());
        GCHandle ph = GCHandle.Alloc(ps, GCHandleType.Pinned);
        IntPtr PPtr = ph.AddrOfPinnedObject();

        IntPtr pVer = IntPtr.Zero;
        IntPtr pTri = IntPtr.Zero;

        int size = 0;
        int vsize = 0;
        int tsize = 0;

        Native_GetContoursMeshByPoints(PPtr, points.Count, width, height, intervalX, intervalY, ref size, ref pVer, ref vsize, ref pTri, ref tsize);

        UnityEngine.Debug.Log(string.Format("GetContoursMeshByPoints pVer: {0}", pVer.ToInt64()));

        Mesh m = new Mesh();

        var vertices = IntPtr2Vector3(pVer, vsize);

        for (int i = 0; i < vertices.Length; i++)
        {
            var temp = vertices[i];
            vertices[i] = new Vector3(temp.x - width / 2, temp.y, 0);
        }



        var triangles = IntPtr2IntArray(pTri, tsize);

        var uvs = IntPtr2Vector(pVer, vsize, 1f / width, 1f / height);


        m.vertices = vertices;


        for (int i = 0; i < triangles.Length / 3; i++)
        {
            var temp = triangles[i * 3 + 2];
            triangles[i * 3 + 2] = triangles[i * 3 + 1];
            triangles[i * 3 + 1] = temp;
        }
        m.triangles = triangles;
        m.uv = uvs;

        Release(pVer);
        Release(pTri);

        return m;


    }


    public static unsafe IntPtr CreateBaselFaceModel(string modelFileName,string shapeFileName,ref Vector3[] vertices,ref int[] triangles,ref int pcaDimCount,ref Color[] colors)
    {
        IntPtr verBuffer = IntPtr.Zero;
        int verCount = 0;
        IntPtr triBuffer = IntPtr.Zero;
        int triCount = 0;

        IntPtr colBuffer = IntPtr.Zero;
        int colCount = 0;

        IntPtr handle = Native_CreateBaselFaceModel(modelFileName,shapeFileName, ref verBuffer,ref verCount, ref triBuffer, ref triCount,ref pcaDimCount, ref colBuffer,ref colCount);


        vertices = new Vector3[verCount];
        int offset = 0;
        int vecSize = Marshal.SizeOf(typeof(Vector3));
        for (int i = 0; i < verCount; i++)
        {
            vertices[i] = (Vector3)Marshal.PtrToStructure(new IntPtr(verBuffer.ToInt64() + offset), typeof(Vector3));
            offset += vecSize;
        }

        int triLen = triCount * 3;
        triangles = new int[triLen];
        offset = 0;
        vecSize = Marshal.SizeOf(typeof(int));
        for (int i = 0; i < triLen; i++)
        {
            triangles[i] = (int)Marshal.PtrToStructure(new IntPtr(triBuffer.ToInt64() + offset), typeof(int));
            offset += vecSize;
        }


        colors = new Color[colCount];
        offset = 0;
        vecSize = Marshal.SizeOf(typeof(Vector3));
        for (int i = 0; i < colCount; i++)
        {
            Vector3 temp = (Vector3)Marshal.PtrToStructure(new IntPtr(colBuffer.ToInt64() + offset), typeof(Vector3));
            colors[i] = new Color(temp.x, temp.y, temp.z, 1.0f);
            offset += vecSize;
        }

        return handle;
    }
    public static unsafe void DestroyBaselFaceModel(IntPtr handle)
    {
        Native_DestroyBaselFaceModel(handle);
    }
    public static unsafe void ChangeBaselFaceModelShapeCoff(IntPtr handle,float[] coff)
    {
        GCHandle ph = GCHandle.Alloc(coff, GCHandleType.Pinned);
        IntPtr coffBuffer = ph.AddrOfPinnedObject();

        Native_ChangeBaselFaceModelShapeCoff(handle,coffBuffer, coff.Length);
        ph.Free();
    }
    public static unsafe void ChangeBaselFaceModelExpressionCoff(IntPtr handle, float[] shapeCoff,float[] expCoff)
    {
        GCHandle phs = GCHandle.Alloc(shapeCoff, GCHandleType.Pinned);
        IntPtr shapeCoffBuffer = phs.AddrOfPinnedObject();

        GCHandle phe = GCHandle.Alloc(expCoff, GCHandleType.Pinned);
        IntPtr expCoffBuffer = phe.AddrOfPinnedObject();

        Native_ChangeBaselFaceModelExpressionCoff(handle, shapeCoffBuffer, shapeCoff.Length, expCoffBuffer, expCoff.Length);
        phs.Free();
        phe.Free();
    }
    public static unsafe void ChangeBaselFaceModelColorCoff(IntPtr handle, float[] coff)
    {
        GCHandle ph = GCHandle.Alloc(coff, GCHandleType.Pinned);
        IntPtr coffBuffer = ph.AddrOfPinnedObject();

        Native_ChangeBaselFaceModelColorCoff(handle, coffBuffer, coff.Length);
        ph.Free();
    }

    public static unsafe void ChangeBaselFaceModelShapeCoffFromLandmark(IntPtr handle, List<Vector2> landmarks,int width,int height, int bfm_point_count, bool bDebug)
    {
        float[] landmarkBuff = new float[landmarks.Count * 2];
        for(int i = 0; i< landmarks.Count;i++)
        {

            landmarkBuff[i * 2 + 0] = landmarks[i].x;
            landmarkBuff[i * 2 + 1] = landmarks[i].y;
        }


        GCHandle ph = GCHandle.Alloc(landmarkBuff, GCHandleType.Pinned);
        IntPtr buffer = ph.AddrOfPinnedObject();

        Native_ChangeBaselFaceModelShapeCoffFromLandmark(handle, buffer, landmarkBuff.Length,width,height, bfm_point_count, bDebug);
        ph.Free();
    }

    public static unsafe void ChangeBaselFaceModelShapeCoffFromImage(IntPtr handle, Texture2D tex, int width, int height,int bfm_point_count, bool bDebug,ref Matrix4x4 cameraMatrix)
    {
        Color32[] texDataColor = tex.GetPixels32();
        float[] cameraCoff = new float[12];
        fixed (Color32* p = (texDataColor))
        {
            fixed(float* pCoff = cameraCoff)
            {
                Native_ChangeBaselFaceModelShapeCoffFromImage(handle, (IntPtr)p, width, height, bfm_point_count, bDebug, (IntPtr)pCoff);

            }

        }
        cameraMatrix = Matrix4x4.identity;

        cameraMatrix.m00 = cameraCoff[0];
        cameraMatrix.m01 = cameraCoff[1];
        cameraMatrix.m02 = cameraCoff[2];
        cameraMatrix.m10 = cameraCoff[3];
        cameraMatrix.m11 = cameraCoff[4];
        cameraMatrix.m12 = cameraCoff[5];
        cameraMatrix.m20 = cameraCoff[6];
        cameraMatrix.m21 = cameraCoff[7];
        cameraMatrix.m22 = cameraCoff[8];
        cameraMatrix.m03 = cameraCoff[9];
        cameraMatrix.m13 = cameraCoff[10];

        //need divide
        cameraMatrix.m33 = 1 / cameraCoff[11];
    }

    public static unsafe void SetMeshVerticesMemoryAddr(IntPtr handle, Vector3[] vertices, Vector2[] uvs,Color[] colors,bool saveh5)
    {
        GCHandle phv = GCHandle.Alloc(vertices, GCHandleType.Pinned);
        IntPtr verBuffer = phv.AddrOfPinnedObject();

        GCHandle phc = GCHandle.Alloc(colors, GCHandleType.Pinned);
        IntPtr colerBuffer = phc.AddrOfPinnedObject();


        GCHandle phuv = GCHandle.Alloc(uvs, GCHandleType.Pinned);
        IntPtr uvBuffer = phuv.AddrOfPinnedObject();
        Native_SetMeshVerticesMemoryAddr(handle,verBuffer, vertices.Length,uvBuffer,uvs.Length, colerBuffer, colors.Length, saveh5);


    }

    public static unsafe void SetTextureMemoryAddr(IntPtr handle, Color32[] textureData, int width, int height)
    {
        fixed (Color32* p = (textureData))
        {
            Native_SetTextureMemoryAddr(handle, (IntPtr)p, width, height);
        }
    }
    public static unsafe void SaveBFMH5(IntPtr handle, string h5FileName)
    {
        Native_SaveBFMH5(handle, h5FileName);
    }


    public static unsafe void SetUnityTexturePtr(string texName,Texture tex)
    {
        long ptr = (long)(tex.GetNativeTexturePtr());
        long i = tex.GetNativeTexturePtr().ToInt64();
        UnityEngine.Debug.Log("Ptr:" + i.ToString("X"));
        Native_SetUnityTexturePtr(texName, tex.GetNativeTexturePtr());
    }

    public static unsafe void DoFillUnityData()
    {
        Native_FillUnityData("D:/DevelopProj/Yuji/YProject/YGarmentClient/YGarmentClient/Assets/2D/Cloth/T.jpg");
    }
    



}
