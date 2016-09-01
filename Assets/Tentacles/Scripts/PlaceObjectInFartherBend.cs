using UnityEngine;
using System.Collections;
using System;
// for projecting the elbow placement. first position is shoulder and second is hand, for example 
public class PlaceObjectInFartherBend : MonoBehaviour {
    public Transform  firstPosition;
   public  Transform secondPosition;
    private float XOvershoot = 1;
  public float xProportion;
    public float yProportion;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = PlaceFartherBend();

     //   fartherPosition.transform.position = Vector3.Scale(transform.position, targetSphere.transform.position);
	}

    private Vector3 PlaceFartherBend()
    {
        Vector3 NewPosition = Vector3.zero;
        float xDiff = secondPosition.position.x - firstPosition.position.x;
        float yDiff = secondPosition.position.y - firstPosition.position.y;


        //float whole = Math.Abs(xDiff) + Math.Abs(yDiff);
        // xProportion = Math.Abs(xDiff) / whole;
        // yProportion = Math.Abs(yDiff) / whole;
        //float newX = Mathf.Lerp(secondPosition.position.x, (secondPosition.position.x - firstPosition.position.x)*1.5f + secondPosition.position.x, xProportion);             //Mathf.Lerp(firstPosition.position.x, secondPosition.position.x, 0.1f);
        //float newY = Mathf.Lerp(secondPosition.position.y, (secondPosition.position.y - firstPosition.position.y) * 0.1f + secondPosition.position.y, yProportion);
        float newX =  Mathf.Lerp(secondPosition.position.x + xDiff, secondPosition.position.x - 2*xDiff, 0.5f - Math.Min(Math.Max(0, yDiff), 0.5f)) ;
        float newY = Mathf.Lerp(secondPosition.position.y + yDiff, firstPosition.position.y-0.5f, 1 - Math.Min(Math.Max(0,yDiff),1));
      if(!float.IsNaN(newX) && !float.IsNaN(newY))  NewPosition = new Vector3(newX, newY, 0);
        return NewPosition;
    }
}

