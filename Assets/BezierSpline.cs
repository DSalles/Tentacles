using UnityEngine;
using System;

public class BezierSpline : MonoBehaviour {

    public Vector3 StartPos;
    public Vector3 EndPos;
    public float TentacleGrowthSpeed = 1f;
    public float TentacleGrowthStart = 0.23f;
    public float TentacleCompletion;
	public Vector3[] points;
    public GameObject[] SourceObjects;
    public Transform[] SpineBlocks;
    public GameObject[] SourceObjectsInUse;
    private BezierControlPointMode[] modes;
    public bool growing = false;
    public int CurveCount {
		get {
			return (points.Length - 1) / 3;
		}
	}

    public TentacleBehavior Tentacle { get; private set; }

    public float MaxBezierCompletion = 0.7f;

    void Awake()
    {
        modes = new BezierControlPointMode[] {
            BezierControlPointMode.Aligned,
            BezierControlPointMode.Aligned
        };
        TentacleCompletion = TentacleGrowthStart;
        SourceObjectsInUse = new GameObject[4];
        for(int i = 0; i < 4; i++)
        SourceObjectsInUse[i] = SourceObjects[i];
        AddCurve();
        AddCurve();
    }

	public Vector3 GetPoint (float t) {
        try
        {
            for (int i = 0; i < SourceObjectsInUse.Length; i++) points[i] = SourceObjectsInUse[i].transform.position;
            int j;
            if (t >= 1f)
            {
                t = 1f;
                j = points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                j = (int)t;
                t -= j;
                j *= 3;
            }
            Vector3 pointsVector = Bezier.GetPoint(points[j], points[j + 1], points[j + 2], points[j + 3], t);

          
            return transform.TransformPoint(pointsVector);
        }
        catch {
            print("OUT OF RANGE MAYBE" );
            return Vector3.zero;           
        }
 }

    public BezierControlPointMode GetControlPointMode(int index)
    {
        return modes[(index + 1) / 3];
    }

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        int modeIndex = (index + 1) / 3;
        modes[modeIndex] = mode;
      
        EnforceMode(index);
    }

    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if (mode == BezierControlPointMode.Free ||  (modeIndex == 0 || modeIndex == modes.Length - 1))
        {
            return;
        }

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0)
            {
                fixedIndex = points.Length - 2;
            }
            enforcedIndex = middleIndex + 1;
            if (enforcedIndex >= points.Length)
            {
                enforcedIndex = 1;
            }
        }
        else
        {
            fixedIndex = middleIndex + 1;
            if (fixedIndex >= points.Length)
            {
                fixedIndex = 1;
            }
            enforcedIndex = middleIndex - 1;
            if (enforcedIndex < 0)
            {
                enforcedIndex = points.Length - 2;
            }
        }

        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }
        points[enforcedIndex] = middle + enforcedTangent;
    }


    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    float t = 0f;


    void Update()
    {      
      for (int i = 0; i < SpineBlocks.Length; i++) { SpineBlocks[i].position = GetPoint((float)i / ((float)SpineBlocks.Length)*TentacleCompletion); SpineBlocks[i].transform.LookAt(SpineBlocks[i].position + GetDirection((float)i / ((float)SpineBlocks.Length) * TentacleCompletion), Vector3.back); }
      if(TentacleCompletion < MaxBezierCompletion)  TentacleCompletion += ( Time.deltaTime/100)*TentacleGrowthSpeed;
        t = TentacleCompletion / MaxBezierCompletion;
            transform.position = Vector3.Lerp(StartPos, EndPos, t);
        if (t >= 1) // at max 
        {      if (growing == true)
            {
                growing = false;
                if (Tentacle) Tentacle.DoneGrowing(growing);
            }
        }
    }


    public void AddCurve()
    {
        Array.Resize(ref SourceObjectsInUse, SourceObjectsInUse.Length + 4);
        for (int i = 4; i < SourceObjectsInUse.Length; i++)
        {
            SourceObjectsInUse[i] = SourceObjects[i];
        }
       
        //Vector3 point = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 4);
        for (int i = 0; i < modes.Length; i++) modes[i] = BezierControlPointMode.Aligned;

    }

    public void Restart(TentacleBehavior thisTentacle)
    {
        print("restart bezier");
        Tentacle = thisTentacle;
        TentacleCompletion = TentacleGrowthStart;
        transform.position = StartPos;
        growing = true;
        Tentacle.DoneGrowing(growing);
    }



    public void Reset()
    {
        points = new Vector3[] {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
        modes = new BezierControlPointMode[] {
            BezierControlPointMode.Aligned,
            BezierControlPointMode.Aligned
        };
    }
}



public static class Bezier
  
{

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * p0 +
            2f * oneMinusT * t * p1 +
            t * t * p2;
    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return
            2f * (1f - t) * (p1 - p0) +
            2f * t * (p2 - p1);
    }

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float OneMinusT = 1f - t;
       
        return
            OneMinusT * OneMinusT * OneMinusT * p0 +
            3f * OneMinusT * OneMinusT * t * p1 +
            3f * OneMinusT * t * t * p2 +
            t * t * t * p3;
    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            3f * oneMinusT * oneMinusT * (p1 - p0) +
            6f * oneMinusT * t * (p2 - p1) +
            3f * t * t * (p3 - p2);
    }
}


public enum BezierControlPointMode
{
    Free,
    Aligned,
    Mirrored
}