using UnityEngine;
using System.Collections;

public class KinectGestureExample : MonoBehaviour {

    public KinectMotionListener[] listeners;
    public Vector3 gestureVector;
//	public KinectMotionListener.Region region;
	public bool bothHands;



	//use:  KinectGestureListener.StopListening() to stop gesture detection.
    void Start()
    {
		Debug.Log ("Listening");
        foreach (var l in listeners)
        {
            //l.ListenForDirectional(gestureVector, OnGestureSuccess, bothHands);
		//	l.ListenForDirectionalRegional(gestureVector, OnGestureSuccess, region, bothHands);
        }
    }

    bool OnGestureSuccess(KinectMotionListener listener)
    {
        Debug.Log("Kinect " + listener.kinectId + " Completed Gesture!");
        return true;
    }
}
