using UnityEngine;
using System.Collections;
using System;
// for projecting the elbow placement. first position is shoulder and second is hand, for example 
public class PlaceObjectInRealBend : MonoBehaviour {
    public Transform  firstTransform;
    public  Transform secondTransform;
    public Vector3 distance;
    public float Angle;
    public Vector3 Axis = Vector3.back;
    public float multiplier = 1;
    public float startAngle = -50;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    transform.position = PlaceFartherBend();
     Angle =  AngleInDeg(firstTransform.position, secondTransform.position);
      
	}

    private Vector3 PlaceFartherBend()
    {
        Vector3 NewPosition = Vector3.zero;
      //  Angle = Vector2.Angle(firstTransform.position, secondTransform.position);
        NewPosition =  distance ;// + secondTransform.position;
        NewPosition = Quaternion.AngleAxis(startAngle+Angle*multiplier,Axis)*NewPosition + secondTransform.position;
        return NewPosition;
    }

    public static float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        float angle = AngleInRad(vec1, vec2) * 180 / Mathf.PI;
        if (angle < 0) angle += 360;
        return angle;

    }
    public static float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }
}