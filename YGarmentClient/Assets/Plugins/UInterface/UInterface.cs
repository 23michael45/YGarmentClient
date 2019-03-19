using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class UInterface : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point2D
    {
        public float x;
        public float y;
    }

    [DllImport("YGarmentLib")]
    private static extern IntPtr RigidAffine(ref int size,IntPtr plist,int pl,IntPtr qlist,int ql,IntPtr vlist,int vl,int type);

    [DllImport("YGarmentLib")]
    private static extern void Release(IntPtr p);

    [DllImport("YGarmentLib")]
    private static extern IntPtr DetectContours(IntPtr texData, int width, int height,float thresh,ref int size);
    [DllImport("YGarmentLib")]
    private static extern IntPtr DetectContoursImage(IntPtr texData, int width, int height);


    [DllImport("YGarmentLib")]
    private static extern void GetContoursMesh(IntPtr texData, int width, int height,int intervalX,int intervalY, float thresh, ref int size, ref IntPtr pVertices, ref int vsize, ref IntPtr pTriangles,ref int tsize);

    Point2D[] Vector2IntPtr(Vector2[] v)
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

    Vector2[] IntPtr2Vector(IntPtr ptr,int size, float scalex = 1,float scaley = 1)
    {

        Point2D[] p = new Point2D[size];

        // memory layout
        // |float|float|float|float|float|float|float|float|float|float..
        // |   ExamplePoint0 |   ExamplePoint1 |   ExamplePoint2 |
        int offset = 0;
        int pointSize = Marshal.SizeOf(typeof(Point2D));
        for (int i = 0; i < size; i++)
        {
            p[i] = (Point2D)Marshal.PtrToStructure(new IntPtr(ptr.ToInt32() + offset), typeof(Point2D));
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

    Vector3[] IntPtr2Vector3(IntPtr ptr, int size, float scalex = 1, float scaley = 1)
    {

        Point2D[] p = new Point2D[size];

        // memory layout
        // |float|float|float|float|float|float|float|float|float|float..
        // |   ExamplePoint0 |   ExamplePoint1 |   ExamplePoint2 |
        int offset = 0;
        int pointSize = Marshal.SizeOf(typeof(Point2D));
        for (int i = 0; i < size; i++)
        {
            p[i] = (Point2D)Marshal.PtrToStructure(new IntPtr(ptr.ToInt32() + offset), typeof(Point2D));
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
    int[] IntPtr2IntArray(IntPtr ptr, int size)
    {

        int[] arr = new int[size];

        // memory layout
        // |float|float|float|float|float|float|float|float|float|float..
        // |   ExamplePoint0 |   ExamplePoint1 |   ExamplePoint2 |
        int offset = 0;
        int pointSize = Marshal.SizeOf(typeof(int));
        for (int i = 0; i < size; i++)
        {
            arr[i] = (int)Marshal.PtrToStructure(new IntPtr(ptr.ToInt32() + offset), typeof(int));
            offset += pointSize;
        }
        
        return arr;
    }




    public Vector2[] DoRigidAffine(Vector2[] p, Vector2[] q, Vector2[] v)
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
        

        IntPtr ptr = RigidAffine(ref size, PPtr, p.Length, QPtr, q.Length, VPtr, v.Length,2);

        ph.Free();
        qh.Free();
        vh.Free();

        Vector2[] varray = IntPtr2Vector(ptr,size);


        Release(ptr);

        return varray;

    }

    public unsafe Texture2D DetectContoursImage(Texture2D texData)
    {
        byte[] data = texData.GetRawTextureData();
        Debug.Log(data.Length);
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

    public unsafe Vector2[] DetectContours(Texture2D texData)
    {
        byte[] data = texData.GetRawTextureData();
        Debug.Log(data.Length);
        Color32[] texDataColor = texData.GetPixels32();
        //Pin Memory
        fixed (Color32* p = (texDataColor))
        {
            int size = 0;
            IntPtr pData = DetectContours((IntPtr)p, texData.width, texData.height,100,ref size);
            Debug.Log("Contours Size:" + size);
            if (pData != IntPtr.Zero)
            {
                return IntPtr2Vector(pData, size);
                
            }
        }
        return null;
    }


    public unsafe Mesh GetContoursMesh(Texture2D texData)
    {


        byte[] data = texData.GetRawTextureData();
        Debug.Log(data.Length);
        Color32[] texDataColor = texData.GetPixels32();
        fixed (Color32* p = (texDataColor))
        {
            int size = 0;
            int vsize = 0;
            int tsize = 0;

            IntPtr pVer = IntPtr.Zero;
            IntPtr pTri = IntPtr.Zero;
            GetContoursMesh((IntPtr)p, texData.width, texData.height,10,10, 100, ref size, ref pVer,ref vsize, ref pTri,ref tsize);

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
}
