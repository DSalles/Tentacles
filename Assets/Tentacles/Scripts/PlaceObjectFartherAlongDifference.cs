using UnityEngine;
using System.Collections;

public class PlaceObjectFartherAlongDifference : MonoBehaviour {
    public Transform  firstPosition;
   public  Transform secondPosition;
    public float HowFar = 1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

       float newX = (secondPosition.position.x - firstPosition.position.x)*HowFar + secondPosition.position.x;
       float newY = (secondPosition.position.y - firstPosition.position.y) + secondPosition.position.y;
        transform.position = new Vector3(newX, newY, 0);
        //   fartherPosition.transform.position = Vector3.Scale(transform.position, targetSphere.transform.position);
    }
}
