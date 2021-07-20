using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Debugger : MonoBehaviour
{

    public float coordLength = 0.25f;

    static Dictionary<int, Billboard> billboards = new Dictionary<int, Billboard>();

    static Stopwatch stopwatch = new Stopwatch();
    class AveInfo
    {
        int num = 0;
        double total = 0f;

        public void PlusValue(double value)
        {
            ++num;
            total += value;
        }
        public double GetAve
        {
            get
            {
                return total / (double)num;
            }
        }
    }
    static AveInfo ave1 = new AveInfo();
    static AveInfo ave2 = new AveInfo();
    static AveInfo ave3 = new AveInfo();
    static AveInfo ave4 = new AveInfo();

    struct ScreenTextInfo
    {
        public string context;
        public Rect rc;
        public Color color;

        public void SetText(string _context)
        {
            context = _context;
        }
        public void SetRc(float posX, float posY, float width, float height)
        {
            rc = new Rect(posX, posY, width, height);
        }
        public void SetColor(Color _color)
        {
            color = _color;
        }
    }
    static Dictionary<int, ScreenTextInfo> screens = new Dictionary<int, ScreenTextInfo>();
    static Dictionary<int, GameObject> markers = new Dictionary<int, GameObject>();

    static int num = 0;
    public static bool DoTimes(int time)
    {
        if (num++ < time)
        {
            return true;
        }

        return false;
    }
    public static void ClearTimes()
    {
        num = 0;
    }

    public static void StartCount()
    {
        stopwatch.Reset();
        stopwatch.Start();
    }
    public static void StopCount(string title)
    {
        stopwatch.Stop();
        UnityEngine.Debug.Log(stopwatch.Elapsed + title);
    }

    public static Color RandomColor
    {
        get
        {
            return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
    }

    public static void Ave1Count(string title)
    {
        stopwatch.Stop();

        ave1.PlusValue(stopwatch.Elapsed.TotalMilliseconds);

        UnityEngine.Debug.Log(ave1.GetAve + title);

        StartCount();
    }
    public static void Ave2Count(string title)
    {
        stopwatch.Stop();

        ave2.PlusValue(stopwatch.Elapsed.TotalMilliseconds);

        UnityEngine.Debug.Log(ave2.GetAve + title);
        StartCount();
    }
    public static void Ave3Count(string title)
    {
        stopwatch.Stop();

        ave3.PlusValue(stopwatch.Elapsed.TotalMilliseconds);

        UnityEngine.Debug.Log(ave3.GetAve + title);
        StartCount();
    }
    public static void Ave4Count(string title)
    {
        stopwatch.Stop();

        ave4.PlusValue(stopwatch.Elapsed.TotalMilliseconds);

        UnityEngine.Debug.Log(ave4.GetAve + title);
        StartCount();
    }

    static int tempID = 0;
    public static int TempID
    {
        get
        {
            return tempID++;
        }
    }

    public static GameObject MarkStatic(Vector3 position, float scale, Color color, PrimitiveType shape = PrimitiveType.Sphere)
    {
        var obj = GameObject.CreatePrimitive(shape);
        obj.transform.localScale = Vector3.one * scale;
        obj.transform.position = position;
        obj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        obj.GetComponent<MeshRenderer>().material.color = color;

        return obj;
    }
    public static GameObject MarkStatic(float x, float y, float z, float scale, Color color, PrimitiveType shape = PrimitiveType.Sphere)
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.localScale = Vector3.one / 6;
        obj.transform.position = new Vector3(x, y, z);

        return obj;
    }
    struct Line
    {
        public Vector3 from;
        public Vector3 to;
        public Color color;

        public Line(Vector3 _from, Vector3 _to, Color _color)
        {
            from = _from;
            to = _to;
            color = _color;
        }
    }
    static Dictionary<int, Line> lines = new Dictionary<int, Line>();
    static List<Line> lineList = new List<Line>();
    struct Tri
    {
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 v3;
        public Color color;

        public Tri(Vector3 _v1, Vector3 _v2, Vector3 _v3, Color _color)
        {
            v1 = _v1;
            v2 = _v2;
            v3 = _v3;

            color = _color;
        }
    }
    static Dictionary<int, Tri> tris = new Dictionary<int, Tri>();
    public static void MarkLine(Vector3 from, Vector3 to, Color color)
    {
        lineList.Add(new Line(from, to, color));
    }
    public static void MarkLine(Vector3 from, Vector3 dir, float length, Color color)
    {
        Vector3 to = from + dir * length;

        lineList.Add(new Line(from, to, color));
    }
    public static void MarkLine(int id, Vector3 from, Vector3 to, Color color)
    {
        if (lines.ContainsKey(id))
        {
            lines[id] = new Line(from, to, color);
        }
        else
            lines.Add(id, new Line(from, to, color));
    }
    public static void MarkLine(int id, Vector3 from, Vector3 dir, float length, Color color)
    {
        Vector3 to = from + dir * length;

        if (lines.ContainsKey(id))
        {
            lines[id] = new Line(from, to, color);
        }
        else
            lines.Add(id, new Line(from, to, color));
    }
    public static void RemoveLine(int id)
    {
        if (lines.ContainsKey(id))
        {
            lines.Remove(id);
        }
    }
    public static void MarkTri(int ID, Vector3 v1, Vector3 v2, Vector3 v3, Color color)
    {
        if (tris.ContainsKey(ID))
        {
            tris[ID] = new Tri(v1, v2, v3, color);
        }
        else
        {
            tris.Add(ID, new Tri(v1, v2, v3, color));
        }
    }

    public struct Sphere
    {
        public Vector3 pos;
        public float rad;
        public Color color;

        public Sphere(Vector3 _pos, float _rad, Color _color)
        {
            pos = _pos;
            rad = _rad;
            color = _color;
        }
    }
    static List<Sphere> wiredSphereList = new List<Sphere>();
    public static void MarkWiredSphere(Vector3 pos, float rad, Color color)
    {
        wiredSphereList.Add(new Sphere(pos, rad, color));
    }
    struct Cube
    {
        public Vector3 center;
        public Vector3 size;
        public Color color;

        public Cube(Vector3 _center, float lengthX, float lengthY, float lengthZ, Color _color)
        {
            center = _center;
            size = new Vector3(lengthX, lengthY, lengthZ);
            color = _color;
        }
    }
    static List<Cube> cubeList = new List<Cube>();
    static Dictionary<int, Cube> cubes = new Dictionary<int, Cube>();
    public static void MarkCube(Vector3 center, float length, Color color)
    {
        cubeList.Add(new Cube(center, length, length, length, color));
    }
    public static void MarkCube(int ID, Vector3 center, float length, Color color)
    {
        if (cubes.ContainsKey(ID))
        {
            cubes[ID] = new Cube(center, length, length, length, color);
        }
        else
        {
            cubes.Add(ID, new Cube(center, length, length, length, color));
        }
    }
    public static void MarkCube(int ID, Vector3 center, float lengthX, float lengthY, float lengthZ, Color color)
    {
        if (cubes.ContainsKey(ID))
        {
            cubes[ID] = new Cube(center, lengthX, lengthY, lengthZ, color);
        }
        else
        {
            cubes.Add(ID, new Cube(center, lengthX, lengthY, lengthZ, color));
        }
    }
    public static void RemoveCube(int ID)
    {
        if (cubes.ContainsKey(ID))
        {
            cubes.Remove(ID);
        }
    }
    public static void ClearCube()
    {
        cubes.Clear();
    }

    public static void Billboard(Vector3 position, float scale, object context, Color color)
    {
        GameObject textPref = Resources.Load("Debug/Billboard") as GameObject;
        Billboard textObj = Instantiate(textPref, position, Quaternion.identity).GetComponent<Billboard>();
        textObj.SetColor(color);
        textObj.SetText(context.ToString());
        textObj.SetText(1f / scale);
    }
    public static void Billboard(int ID, Vector3 position, float scale, object context, Color color)
    {
        if (billboards.ContainsKey(ID))
        {
            billboards[ID].transform.position = position;
            billboards[ID].SetColor(color);
            billboards[ID].SetText(context.ToString());
            billboards[ID].SetText(1f / scale);
        }
        else
        {
            GameObject textPref = Resources.Load("Debug/Billboard") as GameObject;
            Billboard textObj = Instantiate(textPref, position, Quaternion.identity).GetComponent<Billboard>();
            textObj.SetColor(color);
            textObj.SetText(context.ToString());
            textObj.SetText(1f / scale);
            billboards.Add(ID, textObj);
        }
    }
    public static void ScreenText(int ID, float posX, float posY, float width, float height, object context, Color color)
    {
        if (screens.ContainsKey(ID))
        {
            screens[ID].SetText(context.ToString());
            screens[ID].SetRc(posX, posY, width, height);
            screens[ID].SetColor(color);
        }
        else
        {
            ScreenTextInfo newScreenText = new ScreenTextInfo();
            newScreenText.SetText(context.ToString());
            newScreenText.SetRc(posX, posY, width, height);
            newScreenText.SetColor(color);
            screens.Add(ID, newScreenText);
        }
    }
    public static void MarkDynamic(int ID, Vector3 pos, float rad, Color color, Transform _parent = null)
    {
        if (markers.ContainsKey(ID))
        {
            markers[ID].transform.position = pos;
            markers[ID].GetComponent<MeshRenderer>().material.color = color;
        }
        else
        {
            var newMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newMarker.transform.position = pos;
            newMarker.transform.localScale = Vector3.one * 2 * rad;
            newMarker.GetComponent<MeshRenderer>().material.color = color;
            newMarker.transform.SetParent(_parent);

            markers.Add(ID, newMarker);
        }

    }

    private void OnGUI()
    {
        foreach (var text in screens.Values)
        {
            GUI.color = text.color;
            GUI.TextField(text.rc, text.context);
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
            return;
        
        #region Coord
        //Vector3 middlePos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 7, Screen.height / 7, 1));

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(middlePos, middlePos + Vector3.right * coordLength);
        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(middlePos, middlePos + Vector3.up * coordLength);
        //Gizmos.color = Color.blue;
        //if (Vector3.Dot(Vector3.forward, Camera.main.transform.forward) >= 0)
        //{
        //    Gizmos.DrawLine(middlePos, middlePos + Vector3.forward * coordLength);
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawSphere(middlePos, 0.035f);
        //}
        //else
        //{
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawSphere(middlePos, 0.035f);
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawLine(middlePos, middlePos + Vector3.forward * coordLength);
        //}
        #endregion

        #region Line

        foreach (var line in lines.Values)
        {
            Gizmos.color = line.color;
            Gizmos.DrawLine(line.from, line.to);
        }
        foreach (var line in lineList)
        {
            Gizmos.color = line.color;
            Gizmos.DrawLine(line.from, line.to);
        }

        #endregion

        #region Tri

        foreach (var tri in tris.Values)
        {
            Gizmos.color = tri.color;
            Gizmos.DrawLine(tri.v1, tri.v2);
            Gizmos.DrawLine(tri.v2, tri.v3);
            Gizmos.DrawLine(tri.v3, tri.v1);
        }

        #endregion

        #region Cube

        foreach(Cube cube in cubeList)
        {
            Gizmos.color = cube.color;
            Gizmos.DrawWireCube(cube.center, cube.size);
        }
        foreach(Cube cube in cubes.Values)
        {
            Gizmos.color = cube.color;
            Gizmos.DrawWireCube(cube.center, cube.size);
        }

        #endregion

        #region Wired Sphere

        for(int i=0; i< wiredSphereList.Count; ++i)
        {
            Gizmos.color = wiredSphereList[i].color;
            Gizmos.DrawWireSphere(wiredSphereList[i].pos, wiredSphereList[i].rad);
        }

        #endregion
    }

    private void OnDisable()
    {
        lines.Clear();
    }
}
