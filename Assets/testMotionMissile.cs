using UnityEngine;
using System.Collections;

public class testMotionMissile : MonoBehaviour {
    public int Force = 1;
	// Use this for initialization
	void Start () {

        transform.LookAt(Vector3.right);
        GetComponent<Rigidbody>().AddForce(Vector3.back * Force);
        //print("FIRE!!!");
    }

    // Update is called once per frame
    void Update () {
	
	}
}
