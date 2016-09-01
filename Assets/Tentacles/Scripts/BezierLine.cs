using UnityEngine;
using System.Collections;

public class BezierLine : MonoBehaviour
{/*
    public Vector3[] points;
    public GameObject[] SourceObjects;
    public Transform[] SpineBlocks;
    public void Reset()
    {
        points = new Vector3[] {
            //new Vector3(1f, 0f, 0f),
            //new Vector3(2f, 0f, 0f),
            //new Vector3(3f, 0f, 0f),
            //new Vector3(4f,0f,0f)
		};
    }

    public Vector3 GetPoint(float t)
    {
        Vector3 point = Vector3.zero;
        { for (int i = 0; i < SourceObjects.Length; i++) points[i] = SourceObjects[i].transform.position; }

        //   else         if (SourceObjects.Length == 4) {for(int i = 0; i < SourceObjects.Length;i++) points[i] = SourceObjects[i].transform.position; }
        if (SourceObjects.Length == 4) point = transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
        else if (SourceObjects.Length == 5) point = transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], points[4], t));
        else if (SourceObjects.Length == 6) point = transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], points[4], points[5], t));


        return point;

    }
    public Vector3 GetVelocity(float t)
    {
        Vector3 point = Vector3.zero;
        if (points.Length == 3) { point = Bezier.GetFirstDerivative(points[0], points[1], points[2], t) - transform.position; }
        else if (points.Length == 4) { point = Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t) - transform.position; }
        else if (points.Length == 5) { point = Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], points[4], t) - transform.position; }
        return transform.TransformPoint(point);
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }


    void Update()
    {
        for (int i = 0; i < SpineBlocks.Length; i++) { SpineBlocks[i].position = GetPoint((float)i / ((float)SpineBlocks.Length)); SpineBlocks[i].transform.LookAt(SpineBlocks[i].position + GetDirection((float)i / ((float)SpineBlocks.Length)), Vector3.back); }
    }
}

/*

public static class Bezier {



    // 3 sources
    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return
            2f * (1f - t) * (p1 - p0) +
            2f * t * (p2 - p1);
    }

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * p0 +
            2f * oneMinusT * t * p1 +
            t * t * p2;
    }
    // 4 sources
    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            3f * oneMinusT * oneMinusT * (p1 - p0) +
            6f * oneMinusT * t * (p2 - p1) +
            3f * t * t * (p3 - p2);
    }
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }

    // 5 sources
    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            3f * oneMinusT * oneMinusT * (p1 - p0) +
            6f * oneMinusT * t * (p2 - p1) + 6f * oneMinusT * t * (p3 - p2) +
            3f * t * t * (p4 - p3);
    }

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * oneMinusT * p0 +
            4f * oneMinusT * oneMinusT * oneMinusT * t * p1 +
            4f * oneMinusT * oneMinusT * t * t * p2 + 4f * oneMinusT * t * t * t * p3 +
            t * t * t * t * p4;
    }






    //6 sources

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Vector3 p5, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            3f * oneMinusT * oneMinusT * (p1 - p0) +
            6f * oneMinusT * t * (p2 - p1) + 6f * oneMinusT * t * (p3 - p2) + 6f * oneMinusT * t * (p4 - p3) +
           3f * t * t * (p5 - p4);
    }

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Vector3 p5, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * oneMinusT * oneMinusT * p0 +
            5f * oneMinusT * oneMinusT * oneMinusT * oneMinusT * t * p1 +
            5f * oneMinusT * oneMinusT * oneMinusT * t * t * p2 + 5f * oneMinusT * oneMinusT * t * t * t * p3 +
             5f * oneMinusT * t * t * t * t * p4 + t * t * t * t * t * p5;
    }
} */
}
