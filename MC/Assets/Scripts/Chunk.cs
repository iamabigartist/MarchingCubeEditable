
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public struct Voxel
{
    public Vector3 pos;
    public float value;
}
public struct PolyInfo
{
    public Vector3 v1, v2, v3;
    public Vector3 facetNormal;
}

struct tCustom
{
    Vector3 p1, p2, p3;
}

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class Chunk : MonoBehaviour
{
    #region Fields
    
    [Range(2, 23)] [SerializeField] int cubeNumSide;
    
    float cubeLength;
    int totalCubeNum;
    int voxelNum;
    int totalVoxelNum;
    
    ComputeBuffer meshVerticesBuffer;
    ComputeBuffer meshTrianglesBuffer;

    [HideInInspector] public ComputeBuffer voxelBuffer;
    [HideInInspector] public ComputeBuffer polyBuffer;

    [SerializeField] Material sampleMaterial;
    
    Tool tool;
    
    #endregion
    
    void Awake()
    {
        INIT_FIELD();
        INIT_VOXELS();
        INIT_MATERIAL();

        MARCHING_CUBE();
    }

    void Update()
    {
        Carve();
    }
    
    public void INIT_FIELD()
    {
        Collider coll = GetComponent<Collider>();
        Vector3 totalSize = coll.bounds.max - coll.bounds.min;
        coll.enabled = false;

        tool = GameObject.FindObjectOfType<Tool>();
        
        cubeLength = Mathf.Max(Mathf.Max(totalSize.x, totalSize.y), totalSize.z) / (cubeNumSide - 1);
        
        totalCubeNum = cubeNumSide * cubeNumSide * cubeNumSide;

        voxelNum = cubeNumSide + 1;

        totalVoxelNum = voxelNum * voxelNum * voxelNum;
    }
    
    public void INIT_VOXELS()
    {
        voxelBuffer = new ComputeBuffer(totalVoxelNum, Marshal.SizeOf(typeof(Voxel)));

        GPUManager.brain.SetFloat("cubeLength", cubeLength);
        GPUManager.brain.SetInt("cubeNum", cubeNumSide);
        GPUManager.brain.SetInt("voxelNum", voxelNum);
        GPUManager.brain.SetVector("chunkPos", transform.position);

        SetMeshInfo(GetComponent<MeshFilter>().sharedMesh, out meshVerticesBuffer, out meshTrianglesBuffer);
        GPUManager.brain.SetInt("meshtrianglesNum", meshTrianglesBuffer.count);
        GPUManager.InitializeBuffer("meshVertices", meshVerticesBuffer);
        GPUManager.InitializeBuffer("meshTriangles", meshTrianglesBuffer);

        GPUManager.InitializeBuffer("voxelBuffer", voxelBuffer);

        
        GPUManager.Dispatch_InitVoxel(totalVoxelNum);
    }
    void SetMeshInfo(Mesh _mesh, out ComputeBuffer vertBuffer, out ComputeBuffer triBuffer)
    {
        Vector3[] vertices = _mesh.vertices;
        for(int i = 0; i < vertices.Length; ++i)
        {
            vertices[i] = transform.TransformPoint(vertices[i]);
        }
        int[] triangles = _mesh.triangles;

        vertBuffer = new ComputeBuffer(vertices.Length, 12);
        vertBuffer.SetData(vertices);
        triBuffer = new ComputeBuffer(triangles.Length, 4);
        triBuffer.SetData(triangles);
    }
    
    public void INIT_MATERIAL()
    {
        polyBuffer = new ComputeBuffer(totalCubeNum * 5, Marshal.SizeOf(typeof(PolyInfo)));

        Material renderMat = new Material(sampleMaterial);
        renderMat.SetInt("cubeNum", cubeNumSide);
        renderMat.SetBuffer("polyBuffer", polyBuffer);
        GetComponent<MeshRenderer>().material = renderMat;

        GetComponent<MeshFilter>().sharedMesh = GetMesh(totalCubeNum * 5);
    }
    Mesh GetMesh(int size)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[size];
        int[] indices = new int[size];
        for(int i = 0; i < size; ++i)
        {
            indices[i] = i;
        }
        mesh.SetIndices(indices, MeshTopology.Points, 0);

        return mesh;
    }

    public void MARCHING_CUBE()
    {
        GPUManager.MarchCubeBuffer("voxelBuffer", voxelBuffer);
        GPUManager.MarchCubeBuffer("polyBuffer", polyBuffer);
        
        GPUManager.Dispatch_MarchCube(totalCubeNum);
    }
    

    public void Carve()
    {
        GPUManager.brain.SetInt("voxelNum", voxelNum);
        GPUManager.brain.SetFloat("intensity", tool.Intensity);
        GPUManager.brain.SetVector("carvingCenter", tool.CenterPt);
        GPUManager.brain.SetVector("carvingDir", tool.UpDir);
        GPUManager.brain.SetVector("chunkPos", transform.position);
        GPUManager.brain.SetInt("toolTriangleNum", tool.TrianglesNum);
        GPUManager.CarveBuffer("toolVertices", tool.VertBuffer);
        GPUManager.CarveBuffer("toolTriangles", tool.TriBuffer);
        GPUManager.CarveBuffer("voxelBuffer", voxelBuffer);
        GPUManager.Dispatch_Carve(totalVoxelNum);
        
        MARCHING_CUBE();
    }
    



    void OnDestroy()
    {
        meshVerticesBuffer.Release();
        meshTrianglesBuffer.Release();
        polyBuffer.Release();
        voxelBuffer.Release();
    }
}
