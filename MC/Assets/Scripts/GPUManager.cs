using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Kernel
{
    public int kernel;
    public int numThread_x;
    public int numThread_y;
    public int numThread_z;

    public Kernel(ComputeShader cs, string name)
    {
        kernel = cs.FindKernel(name);
        uint _numThread_x;
        uint _numThread_y;
        uint _numThread_z;
        cs.GetKernelThreadGroupSizes(kernel, out _numThread_x, out _numThread_y, out _numThread_z);

        numThread_x = (int)_numThread_x;
        numThread_y = (int)_numThread_y;
        numThread_z = (int)_numThread_z;
    }
}

public class GPUManager : MonoBehaviour
{
    public static ComputeShader brain;
    
    static Kernel kInitialize;
    static Kernel kMarchCube;
    static Kernel kCarve;

    void Awake()
    {
        brain = Resources.Load("Shaders/Brain") as ComputeShader;
        
        kInitialize = new Kernel(brain, "InitVoxel");
        kMarchCube = new Kernel(brain, "MarchCube");
        kCarve = new Kernel(brain, "Carve");
    }
    
    public static void InitializeBuffer(string name, ComputeBuffer buffer)
    {
        brain.SetBuffer(kInitialize.kernel, name, buffer);
    }
    public static void MarchCubeBuffer(string name, ComputeBuffer buffer)
    {
        brain.SetBuffer(kMarchCube.kernel, name, buffer);
    }
    public static void CarveBuffer(string name, ComputeBuffer buffer)
    {
        brain.SetBuffer(kCarve.kernel, name, buffer);
    }
    
    public static void Dispatch_InitVoxel(int x)
    {
        brain.Dispatch(kInitialize.kernel, Mathf.CeilToInt((float)x / kInitialize.numThread_x), 1, 1);
    }
    public static void Dispatch_MarchCube(int x)
    {
        brain.Dispatch(kMarchCube.kernel, Mathf.CeilToInt((float)x / kMarchCube.numThread_x), 1, 1);
    }
    public static void Dispatch_Carve(int x)
    {
        brain.Dispatch(kCarve.kernel, Mathf.CeilToInt((float)x / kCarve.numThread_x), 1, 1);
    }
}
