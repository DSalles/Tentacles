using UnityEngine;
using System.Collections;

public class HandMover : MonoBehaviour {
	public string HandName;  
	private float Offset = 1.21f;
	private float down;
	public float StationIndex;
    public float NormalizationFactor = 2;
    private float HeightSubtraction = 4f;

    /// <summary>
    /// moves an object based on the Kinect parts /// </summary>
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (GameObject.Find (HandName)) 
		{
			Transform refPos = GameObject.Find (HandName).transform;
			transform.position = new Vector3(refPos.position.x,( refPos.position.y - HeightSubtraction )* NormalizationFactor*2, refPos.position.z-StationIndex*Offset);
			print (HandName + refPos.position.y);
		}
	}
}
