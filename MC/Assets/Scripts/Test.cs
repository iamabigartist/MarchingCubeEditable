using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    float Dist2Poly(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 vertex, Vector3 dir)
    {
        const float EPSILON = 0.000001f;

        Vector3 normal = Vector3.Cross(v2-v1, v3-v2).normalized;
        if(Vector3.Dot(dir, normal) > 0)
            return 9999999;

        Vector3 v21, v31, h, s, q;
        float a, u, v;
        v21 = v2 - v1;
        v31 = v3 - v1;
        h = Vector3.Cross(dir, v31);
        a = Vector3.Dot(v21, h);
        s = vertex - v1;
        u = Vector3.Dot(s, h) / a;
        if(u < -EPSILON || u > 1.0 + EPSILON)
            return 9999999;

        q = Vector3.Cross(s, v21);
        v = Vector3.Dot(dir, q) / a;
        if(v < -EPSILON || u + v > 1.0+ EPSILON)
            return 9999999;

        float t = Vector3.Dot(v31, q) / a;
        if(t > EPSILON)
        {
            return t;
        }
        else
            return 9999999;
    }

    public Transform p1, p2, p3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debugger.MarkTri(0, p1.position, p2.position, p3.position, Color.cyan);

        Debug.Log(Dist2Poly(p1.position, p2.position, p3.position, Vector3.zero, Vector3.up));
    }
}
