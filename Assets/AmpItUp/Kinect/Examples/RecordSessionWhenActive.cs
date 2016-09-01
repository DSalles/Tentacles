using UnityEngine;
using System.Collections;

public class RecordSessionWhenActive : MonoBehaviour {

	//public string sessionPrefix = "Example_";
	//int previousCount = 0;
	//bool recording = false;
	//bool captured = false;

	//void Update () {

	//	int currentCount = TentaclesManager.Instance.userTable.Count;

	//	if (previousCount == 0 && currentCount > 0 && !KinectBodiesReceiver.Instance.playingSession) {
	//		KinectBodiesReceiver.Instance.StartSession(sessionPrefix + KinectBodiesReceiver.GetFormattedTimestamp(), "Example");
	//		KinectBodiesReceiver.Instance.CaptureFullFrame("", "Start");
	//		recording = true;
	//	} else if (recording && currentCount == 0) {
	//		KinectBodiesReceiver.Instance.CaptureFullFrame("", "End");
	//		KinectBodiesReceiver.Instance.StopSession();
	//		recording = false;
	//		captured = false;
	//	}

	//	if (recording && !captured) {
	//		if (  TentaclesManager.Instance.ClosestUserBody.data.handRightPosition.y > TentaclesManager.Instance.ClosestUserBody.data.headPosition.y &&
	//			TentaclesManager.Instance.ClosestUserBody.data.handLeftPosition.y < TentaclesManager.Instance.ClosestUserBody.data.spineMidPosition.y
	//			)
	//		{
	//			KinectBodiesReceiver.Instance.CaptureFullFrame("", "RightWave");
	//			Debug.Log("RightWave Detected");
	//			captured = true;
	//		}
	//	}


	//	previousCount = TentaclesManager.Instance.userTable.Count;
	//}
}
