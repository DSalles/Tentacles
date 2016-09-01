using UnityEngine;
using System.Collections;
using System;
// for projecting the elbow placement. first position is shoulder and second is hand, for example 
public class PlaceObjectInMiddleBend : MonoBehaviour {
    public Transform  firstTransform;
   public  Transform secondTransform;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = PlaceInBend();

     //   fartherPosition.transform.position = Vector3.Scale(transform.position, targetSphere.transform.position);
	}

    private Vector3 PlaceInBend()
    {
        Vector3 NewPosition = Vector3.zero;
        float newX = Mathf.Lerp(firstTransform.position.x -(secondTransform.position.x-firstTransform.position.x)*1.2f, secondTransform.position.x, 0);
        float newY = Mathf.Lerp(firstTransform.position.y, secondTransform.position.y, 0.5f) + 0.5f -(secondTransform.position.y - firstTransform.position.y);
        NewPosition = new Vector3(newX, newY, 0);
        return NewPosition;
    }
}

