using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour {
    
    [Range(0.001f, 1f)] [SerializeField] float intensity;
    public Vector3 CenterPt{get { return transform.position; }}
    public float Intensity { get { return intensity; } }

    ComputeBuffer vertBuffer;
    ComputeBuffer triBuffer;
    int trianglesNum;
    public ComputeBuffer VertBuffer { get { return vertBuffer; } }
    public ComputeBuffer TriBuffer { get { return triBuffer; } }
    public int TrianglesNum { get { return trianglesNum; } }

    void Awake()
    {
        Mesh curMesh = GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = curMesh.vertices;
        int[] triangles = curMesh.triangles;

        vertBuffer = new ComputeBuffer(vertices.Length, 12);
        triBuffer = new ComputeBuffer(triangles.Length, 4);
        vertBuffer.SetData(vertices);
        triBuffer.SetData(triangles);
        trianglesNum = triangles.Length;
    }

    public Vector3 UpDir
    {
        get { return transform.up; }
    }

    void OnDisable()
    {
        vertBuffer.Release();
        triBuffer.Release();
    }
}
