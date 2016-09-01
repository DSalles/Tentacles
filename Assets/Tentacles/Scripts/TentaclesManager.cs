using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KinectLink;

public class TentaclesManager : MonoBehaviour
{

	public static TentaclesManager Instance;

	public class Body
	{
		public Transform transform;
		public Transform handLeftTransform;
		public Transform handRightTransform;
		public Transform headTransform;
		public KinectBodiesReceiver.BodyData data;

		public Body (Transform transform, Transform handLeftTransform, Transform handRightTransform, Transform headTransform, KinectBodiesReceiver.BodyData data)
		{
			this.transform = transform;
			this.handLeftTransform = handLeftTransform;
			this.handRightTransform = handRightTransform;
			this.headTransform = headTransform;
			this.data = data;
		}


	}

	public GameObject bodyPrefab;
	public GameObject handPrefab;
	public GameObject headPrefab;
	public Dictionary<ulong, Body> userTable = new Dictionary<ulong, Body> ();

	public Vector3 AverageUserPosition {
		get {
			if (userTable.Count == 0)
				return Vector3.zero;

			Vector3 pos = Vector3.zero;
			foreach (Body b in userTable.Values) {
				pos += b.transform.position;
			}

			pos /= userTable.Count;

			return pos;
		}
	}

	public Vector3 ClosestUserPosition {
		get {
			if (userTable.Count == 0)
				return Vector3.zero;

			float z = float.MaxValue;
			Transform t = null;
			foreach (Body b in userTable.Values) {
				if (b.transform.position.z < z) {
					t = b.transform;
					z = t.position.z;
				}
			}

			return t.position;
		}
	}

	public Body ClosestUserBody {
		get {
			if (userTable.Count == 0)
				return null;

			float z = float.MaxValue;
			Body body = null;
			foreach (Body b in userTable.Values) {
				if (b.transform.position.z < z) {
					body = b;
					z = b.transform.position.z;
				}
			}

			return body;
		}
	}

	Bounds bounds;
	KinectBodiesReceiver bodyReceiver;

	void OnGUI ()
	{
		/*
		if (Application.isEditor) {
			foreach (var kp in userTable) {
				GUILayout.Label(kp.Key.ToString() + "  :  " + kp.Value.data.position);
			}
		}
		 */
	}

	void Awake ()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		bodyReceiver = KinectBodiesReceiver.Instance;
		bodyReceiver.OnBodyFrameDataReceived += HandleOnBodyFrameDataReceived;

		bounds = new Bounds (Vector3.zero, Vector3.zero);
	}

	void HandleOnBodyFrameDataReceived (KinectBodiesReceiver.BodyFrameData bodyFrame)
	{
		List<ulong> userCullList = new List<ulong> ();
		foreach (var id in userTable.Keys) {
			//if frame is from same Kinect as body data
			if (userTable [id].data.kinectId == bodyFrame.kinectId) {
				userCullList.Add (id);
			} else {
				//todo:  deal with bodies from other Kinects
			}
		}

		//TODO redo order of operations to avoid unnecessary math on untracked things
		Matrix4x4 matrix = Matrix4x4.identity;

		if (bodyReceiver.kinectInfoTable.ContainsKey (bodyFrame.kinectId))
			matrix = bodyReceiver.kinectInfoTable [bodyFrame.kinectId].matrix;

		foreach (var bodyData in bodyFrame.bodyData) {
			//skip filtered data
			if (!bodyData.valid) {
				//if (bodyFrame.kinectId == 0x04)
					//Debug.Log (bodyData.trackingId + " Not Valid!");

				continue;
			}
                

			Vector3 spinePos = matrix.MultiplyPoint3x4 (bodyData.position);
			Vector3 spineMidPos = matrix.MultiplyPoint3x4 (bodyData.spineMidPosition);
			Vector3 spineShoulderPos = matrix.MultiplyPoint3x4 (bodyData.spineShoulderPosition);
			Vector3 headPos = matrix.MultiplyPoint3x4 (bodyData.headPosition);
			Vector3 handLeftPos = matrix.MultiplyPoint3x4 (bodyData.handLeftPosition);
            Vector3 handRightPos = matrix.MultiplyPoint3x4(bodyData.handRightPosition);
 //           Vector3 ShoulderLeftPos = matrix.MultiplyPoint3x4(bodyData.shoulderLeftPosition);
   //         Vector3 ShoulderRightPos = matrix.MultiplyPoint3x4(bodyData.shoulderRightPosition);
 //           Vector3 elbowLeftPosition = matrix.MultiplyPoint3x4(bodyData.elbowLeftPosition);
    //        Vector3 elbowRightPosition = matrix.MultiplyPoint3x4(bodyData.elbowRightPosition);
    //        Vector3 handTipLeftPosition = matrix.MultiplyPoint3x4(bodyData.handTipLeftPosition);
    //        Vector3 handTipRightPosition = matrix.MultiplyPoint3x4(bodyData.handTipRightPosition);

         

            if (bounds.center == Vector3.zero)
				bounds.center = spinePos;

			bounds.Encapsulate (spinePos);
			if (userTable.ContainsKey (bodyData.trackingId)) {

				if (bodyData.trackingState != TrackingState.NotTracked)
					userCullList.Remove (bodyData.trackingId);
				else
					continue;

				if (bodyData.kinectId != userTable [bodyData.trackingId].data.kinectId)
					Debug.Log ("WTH THIS SHOULDNT HAPPEN");
				userTable [bodyData.trackingId].data = bodyData;

			} else {
				if (bodyData.trackingState == TrackingState.NotTracked)
					continue;

                ////************************ TODO: assign avatar to each body in user table corresponding to its station

                /*using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class HandsManager : MonoBehaviour
{
    public BodySourceManager BodyManager;
    public GameObject HandObject;
    private Dictionary<ulong, GameObject[]> _Bodies = new Dictionary<ulong, GameObject[]>();
    private Kinect.JointType[] UpperLimbsJointTypes;
    //public Kinect.ColorSpacePoint SpacePoint;
    // Use this for initialization

    void Start()
    {
        UpperLimbsJointTypes = new Kinect.JointType[8] { Kinect.JointType.HandRight, Kinect.JointType.ElbowRight, Kinect.JointType.SpineMid,Kinect.JointType.HandLeft, Kinect.JointType.ElbowLeft, Kinect.JointType.SpineMid,Kinect.JointType.HandTipRight, Kinect.JointType.HandTipLeft};
    }

    // Update is called once per frame
    void Update()
    {
        if (BodyManager == null)
        {
            return;
        }

        Kinect.Body[] data = BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                for (int i = 0; i < _Bodies[trackingId].Length; i++) { Destroy(_Bodies[trackingId][i]); }
                _Bodies.Remove(trackingId);
            }
        }
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }

                MoveBothHands(body, _Bodies[body.TrackingId]);                
            }
        }
    }

    private void MoveBothHands(Kinect.Body body, GameObject[] upperLimbs)
    {
             Kinect.CameraSpacePoint cameraspace;
        Vector2 vectorPoint;
        for (int i = 0; i < UpperLimbsJointTypes.Length; i++)
        {                
        cameraspace = body.Joints[UpperLimbsJointTypes[i]].Position;
       // if (i == 1 || i == 4) { upperLimbs[i].transform.rotation = new Quaternion(body.JointOrientations[UpperLimbsJointTypes[i - 1]].Orientation.X, body.JointOrientations[UpperLimbsJointTypes[i - 1]].Orientation.Y, body.JointOrientations[UpperLimbsJointTypes[i - 1]].Orientation.Z, body.JointOrientations[UpperLimbsJointTypes[i - 1]].Orientation.W); }
        try
        {
            vectorPoint = Vectorize(cameraspace);
            try
            { 
                upperLimbs[i].transform.position = new Vector3(vectorPoint.x, vectorPoint.y, 0);           
            }
            catch { print("positioning trouble"); 
            }
        }
        catch { print("Missing coordinate mapper");
        }       
      
        }

    }

    public CoordinateMapperManager CoordinateMapper;

    private Kinect.ColorSpacePoint ColorPointize(Kinect.CameraSpacePoint cameraspace)
    {  
      return   CoordinateMapper.MapSpaceToCoorPoints(cameraspace);
    }

    private static Vector2 Vectorize(Kinect.CameraSpacePoint cameraspace)
    {
      Vector2  cameraVector = new Vector2(cameraspace.X, cameraspace.Y);
      return cameraVector;
    }

    private GameObject[] CreateBodyObject(ulong p)
    {
        GameObject[] UpperLimbs = new GameObject[8];
        var bothHands = Instantiate(HandObject);
        bothHands.SetActive(true);
        foreach (Transform child in bothHands.transform) { child.gameObject.layer = 9; foreach (Transform grandchild in child) { grandchild.gameObject.layer = 9; foreach (Transform greatgrandchild in grandchild) { greatgrandchild.gameObject.layer = 9; foreach (Transform greatgreatgrandchild in greatgrandchild) { greatgreatgrandchild.gameObject.layer = 9; foreach (Transform lastgrandchild in greatgreatgrandchild) lastgrandchild.gameObject.layer = 9; } } } }
        bothHands.name = "BothHands" + p;
       
        try { UpperLimbs[(int)UpperLimbParts.RightHandTip] = bothHands.transform.FindChild("RightHandTip").gameObject; }
        catch { print("Missing Right HAnd"); }
        try { UpperLimbs[(int)UpperLimbParts.RightHand] = bothHands.transform.FindChild("RightHand").gameObject; }
        catch { print("Missing Right HAnd"); }
        try { UpperLimbs[(int)UpperLimbParts.RightElbow] = bothHands.transform.FindChild("RightElbow").gameObject; }
        catch { print("Missing rightelbow"); }
        try { UpperLimbs[(int)UpperLimbParts.RightShoulder] = bothHands.transform.FindChild("RightTorso").gameObject; }
        catch { print("missing right shoulder"); }
        try { UpperLimbs[(int)UpperLimbParts.LeftHandTip] = bothHands.transform.FindChild("LeftHandTip").gameObject; }
        catch { print("Missing Right HAnd"); }
         try {UpperLimbs[(int)UpperLimbParts.LeftHand] = bothHands.transform.FindChild("LeftHand").gameObject;}
         catch { print("missing lieft hand"); }
         try {UpperLimbs[(int)UpperLimbParts.LeftElbow] = bothHands.transform.FindChild("LeftElbow").gameObject;}
         catch { print("missiding left elbow"); }
         try { UpperLimbs[(int)UpperLimbParts.LeftShoulder] = bothHands.transform.FindChild("LeftTorso").gameObject; }
         catch { print("LeftShoulder");}
        return UpperLimbs;
    }

   enum  UpperLimbParts : int
    {       
        RightHand = 0,
       RightElbow = 1,
        RightShoulder = 2,
        LeftHand = 3,
       LeftElbow = 4,
        LeftShoulder = 5,
       RightHandTip = 6,
       LeftHandTip = 7
    }

}
*/

                //add new
                GameObject go = (GameObject)Instantiate (bodyPrefab, spinePos, Quaternion.identity);
				go.GetComponent<Renderer> ().material.color = new Color (Random.value, Random.value, Random.value, 1);
				if (handPrefab != null) {
					GameObject hl = (GameObject)Instantiate (handPrefab, handLeftPos, Quaternion.identity);
					GameObject hr = (GameObject)Instantiate (handPrefab, handRightPos, Quaternion.identity);
					GameObject hd = (GameObject)Instantiate (headPrefab, headPos, Quaternion.identity);
					hl.name += "[Left]" + "[" + bodyFrame.kinectId + "]";
					hr.name += "[Right]" + "[" + bodyFrame.kinectId + "]";
					hd.name += "[Head]" + "[" + bodyFrame.kinectId + "]";

					hr.transform.parent = go.transform;
					hl.transform.parent = go.transform;
					hd.transform.parent = go.transform;

					//Debug.Log ("Adding Tracker: " + bodyData.trackingId + " Valid: " + bodyData.valid + " " + bodyData.kinectId);
					userTable.Add (bodyData.trackingId, new Body (go.transform, hl.transform, hr.transform, hd.transform, bodyData));
				} else {
					userTable.Add (bodyData.trackingId, new Body (go.transform, null, null, null, bodyData));
				}

				go.BroadcastMessage ("SetBody", userTable [bodyData.trackingId], SendMessageOptions.DontRequireReceiver);

				userCullList.Remove (bodyData.trackingId);
			}

			//update existing

			Body body = userTable [bodyData.trackingId];
			body.data.position = spinePos;
			body.data.spineMidPosition = spineMidPos;
			body.data.spineShoulderPosition = spineShoulderPos;
        //    body.data.elbowLeftPosition = elbowLeftPosition;
			body.data.headPosition = headPos;
			body.data.handLeftPosition = handLeftPos;
			body.data.handRightPosition = handRightPos;
			body.data.modifiedByMatrix = true;
			body.transform.position = spinePos;
			if (body.handLeftTransform != null)
				body.handLeftTransform.position = handLeftPos;
			if (body.handRightTransform != null)
				body.handRightTransform.position = handRightPos;
			if (body.headTransform != null)
				body.headTransform.position = headPos;

		}

		foreach (ulong id in userCullList) {
			//if (bodyFrame.kinectId == 0x04)
				//Debug.Log (id);

			if (userTable.ContainsKey (id)) {
				//Debug.Log ("Destroying: " + id + " From " + userTable [id].data.kinectId);
				Destroy (userTable [id].transform.gameObject);
				if (userTable [id].handLeftTransform != null) {
					Destroy (userTable [id].handLeftTransform.gameObject);
					Destroy (userTable [id].handRightTransform.gameObject);
					Destroy (userTable [id].headTransform.gameObject);
				}
				userTable.Remove (id);
			}
		}
	}

	void OnDrawGizmos ()
	{
		Gizmos.DrawWireCube (bounds.center, bounds.size);

		foreach (var kp in userTable) {
			KinectBodiesReceiver.BodyData data = kp.Value.data;
			//draw head position
			if (data.modifiedByMatrix) {
				Gizmos.color = Color.magenta;
				Gizmos.DrawWireCube (data.position, Vector3.one * 0.2f);
				Gizmos.DrawWireCube (data.spineMidPosition, Vector3.one * 0.2f);
				Gizmos.DrawWireCube (data.spineShoulderPosition, Vector3.one * 0.15f);
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireCube (data.headPosition, Vector3.one * 0.15f);
				DrawHandGizmo (data.handLeftPosition, data.handLeftState, data.handLeftConfidence, data.handLeftTrackingState);
				DrawHandGizmo (data.handRightPosition, data.handRightState, data.handRightConfidence, data.handRightTrackingState);
			}
		}

	}

	void DrawHandGizmo (Vector3 position, HandState handState, TrackingConfidence confidence, TrackingState trackingState)
	{
		Color trackingColor = confidence == TrackingConfidence.High ? Color.green : Color.yellow;

		switch (handState) {
		case HandState.Unknown:
			Gizmos.color = trackingColor;
			Gizmos.DrawWireSphere (position, 0.15f);
			break;
		case HandState.Closed:
			Gizmos.color = trackingColor;
			Gizmos.DrawWireCube (position, Vector3.one * 0.3f);
			break;
		case HandState.Open:
			Gizmos.color = trackingColor;
			Gizmos.DrawWireSphere (position, 0.3f);
			break;

		case HandState.Lasso:
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube (position + new Vector3 (0, 0.3f, 0), new Vector3 (0.3f, 0.6f, 0.3f));
			break;
		case HandState.NotTracked:
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere (position, 0.3f);
			break;
		}
	}
}
