using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class KinectMotionListener : MonoBehaviour
{

    public StationListener ThisStationListener; // DS added 

   // public enum Region { Lower, Upper, Left, Right, UpperLeft, UpperRight, LowerLeft, LowerRight }

    public byte kinectId;

    [Header("Directional")]
    public int samples = 10;
    public float angleLimit = 30;


    public Transform[] RightHand;
    public Transform[] RightShoulder;
    public Transform[] LeftHand;
    public Transform[] LeftShoulder;

    public TentacleBehavior[] tentacles;
    public float ShoulderYOffset = -1f;

    List<Vector3> leftPositions = new List<Vector3>();
    List<Vector3> rightPositions = new List<Vector3>();

    Vector3 spinePos;
    Vector3 spineMidPos;
    Vector3 spineShoulderPos;
    Vector3 headPos;
    Vector3 handLeftPos;
    Vector3 handRightPos;

    bool initialized = false;

   // public bool IsListening = false;
    Vector3 handLeftPrevious;
    Vector3 handRightPrevious;
    public Vector3 handLeftDelta;
    public Vector3 handRightDelta;
    public float LastTimeBodiesInPlayfield;

    void OnEnable()
    {
        if (KinectBodiesReceiver.Instance != null)
        {
            KinectBodiesReceiver.Instance.OnBodyFrameDataReceived += OnBodyFrameDataReceived;
            initialized = true;
        }

    }

    void Start()
    {
        Application.targetFrameRate = 60;
        if (!initialized)
            KinectBodiesReceiver.Instance.OnBodyFrameDataReceived += OnBodyFrameDataReceived;
        foreach (Transform rs in GetComponentsInChildren<Transform>())
        {
            switch (rs.name)

            {
                case "RightShoulder":

                    Array.Resize<Transform>(ref RightShoulder, RightShoulder.Length + 1);
                    RightShoulder[RightShoulder.Length - 1] = rs;
                    break;

                case "RightHand":
                    Array.Resize<Transform>(ref RightHand, RightHand.Length + 1);
                    RightHand[RightHand.Length - 1] = rs;
                    break;

                case "LeftShoulder":
                    Array.Resize<Transform>(ref LeftShoulder, LeftShoulder.Length + 1);
                    LeftShoulder[LeftShoulder.Length - 1] = rs;
                    break;

                case "LeftHand":
                    Array.Resize<Transform>(ref LeftHand, LeftHand.Length + 1);
                    LeftHand[LeftHand.Length - 1] = rs;
                    break;

                default:
                   
                    break;
            }

        }
    }

    void OnDisable()
    {
        KinectBodiesReceiver.Instance.OnBodyFrameDataReceived -= OnBodyFrameDataReceived;
    }

    /// <summary>
    /// Handle incoming data from any kinect, only care if it matches configured Kinect ID
    /// </summary>
    /// <param name="bodyFrame"></param>
    void OnBodyFrameDataReceived(KinectBodiesReceiver.BodyFrameData bodyFrame)
    {
       
        // make sure this is only from the kinect assigned to this station
        if (bodyFrame.kinectId != kinectId)
            return;

        Matrix4x4 matrix = Matrix4x4.identity;

        if (KinectBodiesReceiver.Instance.kinectInfoTable.ContainsKey(bodyFrame.kinectId))
            matrix = KinectBodiesReceiver.Instance.kinectInfoTable[bodyFrame.kinectId].matrix;



       //check to see if bodies are in playfield
     //   if (LastTimeBodiesInPlayfield + .3f < Time.time)
      //  {
            if (bodyFrame.bodyData.Length > 0)
            {
                foreach (var bodyData in bodyFrame.bodyData)
                {
                    //skip filtered data
                    if (!bodyData.valid)
                    {                      
                        ThisStationListener.PlayerIn(false);
                        continue;
                    }
                    ThisStationListener.PlayerIn(true);
                }
            }
            else
            {
                ThisStationListener.PlayerIn(false);
            }
     //   }

        foreach (var bodyData in bodyFrame.bodyData)
        {
            //skip filtered data
            if (!bodyData.valid)
                continue;
         //   LastTimeBodiesInPlayfield = Time.time; // DS added
            spinePos = matrix.MultiplyPoint3x4(bodyData.position);
            spineMidPos = matrix.MultiplyPoint3x4(bodyData.spineMidPosition);
            spineShoulderPos = matrix.MultiplyPoint3x4(bodyData.spineShoulderPosition);
            //   headPos = matrix.MultiplyPoint3x4(bodyData.headPosition);
            handLeftPos = matrix.MultiplyPoint3x4(bodyData.handLeftPosition);
            handRightPos = matrix.MultiplyPoint3x4(bodyData.handRightPosition);

            handLeftDelta = handLeftPos - handLeftPrevious;
            handRightDelta = handRightPos - handRightPrevious;

            //if(kinectId == 0x04)
            //Debug.Log(handRightDelta);
            handLeftPrevious = handLeftPos;
            handRightPrevious = handRightPos;

        }
    }


    void Update()
    {
        float leftHandSpeed = 0f;
        float rightHandSpeed = 0f;
        leftHandSpeed += Mathf.Abs(handLeftDelta.y);
        leftHandSpeed += Mathf.Abs(handLeftDelta.x);
        leftHandSpeed += Mathf.Abs(handLeftDelta.z);

        rightHandSpeed += Mathf.Abs(handRightDelta.y);
        rightHandSpeed += Mathf.Abs(handRightDelta.x);
        rightHandSpeed += Mathf.Abs(handRightDelta.z);

        foreach (TentacleBehavior tentacle in tentacles) tentacle.HandSpeed = tentacle.hand == TentacleBehavior.Handedness.Left ? leftHandSpeed : rightHandSpeed;

        foreach (Transform rightHand in RightHand) rightHand.localPosition = HandNormalized(spinePos,handRightPos);
        foreach (Transform rightShoulder in RightShoulder) rightShoulder.localPosition = ShoulderNormalize(); //new Vector3(FlattenedAndNarrowed(spinePos).x, FlattenedAndNarrowed(spinePos).y - ShoulderYOffset, FlattenedAndNarrowed(spinePos).z);

        foreach (Transform leftHand in LeftHand) leftHand.localPosition = HandNormalized(spinePos, handLeftPos); // FlattenedAndNarrowed(handLeftPos);

        foreach (Transform leftShoulder in LeftShoulder) leftShoulder.localPosition = ShoulderNormalize();  // new Vector3(FlattenedAndNarrowed(spinePos).x, FlattenedAndNarrowed(spinePos).y - ShoulderYOffset, FlattenedAndNarrowed(spinePos).z);
    }

    private Vector3 FlattenedAndNarrowed(Vector3 pos)
    {
        return new Vector3(pos.x*XMultiplier + XOffset, pos.y , 0);
    }

    private Vector3 HandNormalized(Vector3 shoulderPos, Vector3 handPos)
    {
        Vector3 normalizingOffset = Vector3.zero - shoulderPos;
        Vector3 normalizedHand = handPos + normalizingOffset;
        return normalizedHand;
    }

    private Vector3 ShoulderNormalize()
    {
        return Vector3.zero;
    }

    private Vector3 NarrowedAndStretched(Vector3 pos)
    {
        return new Vector3(pos.x * XMultiplier + XOffset, pos.y * Ymultiplier + Yoffset, 0);
    }

    /// <summary>
    /// Begin listening for a Straight Directional Gesture
    /// </summary>
    /// <param name="vector"> Direction and Intensity aggregate value</param>
    /// <param name="callback">Callback when gesture is completed successfully</param>
    /// <param name="bothHands">Need to use both hands or not</param>
    //public void ListenForDirectional(Vector3 vector, System.Func<KinectMotionListener, bool> callback, bool bothHands = false)
    //{
    //    StartCoroutine(ListenForDirectionalRoutine(vector, callback, bothHands));
    //}

    /// <summary>
    /// L
    /// </summary>
    /// <param name="vector"> Direction and Intensity aggregate value</param>
    /// <param name="callback">Callback when gesture is completed successfully</param>
    /// <param name="region">Region of the end of the motion must be in to succeed</param>
    /// <param name="bothHands">Need to use both hands or not</param>
    //public void ListenForDirectionalRegional(Vector3 vector, System.Func<KinectMotionListener, bool> callback, Region region, bool bothHands = false)
    //{
    //    StartCoroutine(ListenForDirectionalRegionalRoutine(vector, callback, region, bothHands));
    //}

    /// <summary>
    /// Stop listening for any gesture on this object
    /// </summary>
    public void StopListening()
    {
     //   IsListening = false;
        StopAllCoroutines();
    }

 /*   IEnumerator ListenForDirectionalRegionalRoutine(Vector3 vector, System.Func<KinectMotionListener, bool> callback, Region region, bool bothHands)
    {
        IsListening = true;
        Vector3 leftDelta = Vector3.zero;
        Vector3 rightDelta = Vector3.zero;
        Vector3 leftPrevious = handLeftPos;
        Vector3 rightPrevious = handRightPos;

        leftPositions.Clear();
        rightPositions.Clear();

        while (true)
        {
            //			Debug.Log ("LISTEN");
            leftDelta += (handLeftPos - leftPrevious);
            rightDelta += (handRightPos - rightPrevious);

            leftPositions.Add(handLeftPos);
            rightPositions.Add(handRightPos);

            if (leftPositions.Count > samples)
            {
                leftPositions.RemoveAt(0);
                rightPositions.RemoveAt(0);
            }

            Vector3 lOrigin = leftPositions[0];
            Vector3 rOrigin = rightPositions[0];
            Vector3 lDelta = Vector3.zero;
            Vector3 rDelta = Vector3.zero;

            for (int i = 0; i < leftPositions.Count - 1; i++)
            {
                lDelta += (leftPositions[i + 1] - leftPositions[i]);
                rDelta += (rightPositions[i + 1] - rightPositions[i]);
            }

            bool leftComplete = false;
            bool rightComplete = false;

            if (lDelta.magnitude >= vector.magnitude)
            {
                if (Vector3.Angle(lDelta, vector) < 30)
                {
                    leftComplete = true;
                }
            }

            if (rDelta.magnitude >= vector.magnitude)
            {
                if (Vector3.Angle(rDelta, vector) < 30)
                {
                    rightComplete = true;
                }
            }


            if (bothHands)
            {
                if (leftComplete && IsRegionMatch(handLeftPos, region) && rightComplete && IsRegionMatch(handRightPos, region))
                {
                    IsListening = false;
                    callback(this);
                    break;
                }
            }
            else
            {
                if (leftComplete || rightComplete)
                {

                    if (leftComplete && IsRegionMatch(handLeftPos, region))
                    {
                        IsListening = false;
                        callback(this);
                        break;
                    }
                    else if (rightComplete && IsRegionMatch(handRightPos, region))
                    {
                        IsListening = false;
                        callback(this);
                        break;
                    }
                }
            }

            leftPrevious = handLeftPos;
            rightPrevious = handRightPos;
            yield return null;
        }
    }*/

    //bool IsRegionMatch(Vector3 pos, Region region)
    //{

    //    switch (region)
    //    {
    //        case Region.Left:
    //            return pos.z > spineShoulderPos.z;
    //        case Region.Right:
    //            return pos.z < spineShoulderPos.z;
    //        case Region.Upper:
    //            return pos.y > spineShoulderPos.y;
    //        case Region.Lower:
    //            return pos.y < spineShoulderPos.y;
    //        case Region.UpperLeft:
    //            return pos.z > spineShoulderPos.z && pos.y > spineShoulderPos.y;
    //        case Region.UpperRight:
    //            return pos.z < spineShoulderPos.z && pos.y > spineShoulderPos.y;
    //        case Region.LowerLeft:
    //            return pos.z > spineShoulderPos.z && pos.y < spineShoulderPos.y;
    //        case Region.LowerRight:
    //            return pos.z < spineShoulderPos.z && pos.y < spineShoulderPos.y;
    //    }

    //    return false;
    //}

 /*   IEnumerator ListenForDirectionalRoutine(Vector3 vector, System.Func<KinectMotionListener, bool> callback, bool bothHands)
    {
        IsListening = true;
        Vector3 leftDelta = Vector3.zero;
        Vector3 rightDelta = Vector3.zero;
        Vector3 leftPrevious = handLeftPos;
        Vector3 rightPrevious = handRightPos;

        leftPositions.Clear();
        rightPositions.Clear();

        while (true)
        {
            leftDelta += (handLeftPos - leftPrevious);
            rightDelta += (handRightPos - rightPrevious);

            leftPositions.Add(handLeftPos);
            rightPositions.Add(handRightPos);

            if (leftPositions.Count > samples)
            {
                leftPositions.RemoveAt(0);
                rightPositions.RemoveAt(0);
            }

            Vector3 lOrigin = leftPositions[0];
            Vector3 rOrigin = rightPositions[0];
            Vector3 lDelta = Vector3.zero;
            Vector3 rDelta = Vector3.zero;

            for (int i = 0; i < leftPositions.Count - 1; i++)
            {
                lDelta += (leftPositions[i + 1] - leftPositions[i]);
                rDelta += (rightPositions[i + 1] - rightPositions[i]);
            }

            bool leftComplete = false;
            bool rightComplete = false;

            if (lDelta.magnitude >= vector.magnitude)
            {
                if (Vector3.Angle(lDelta, vector) < 30)
                {
                    leftComplete = true;
                }
            }

            if (rDelta.magnitude >= vector.magnitude)
            {
                if (Vector3.Angle(rDelta, vector) < 30)
                {
                    rightComplete = true;
                }
            }

            if (bothHands)
            {
                if (leftComplete && rightComplete)
                {
                    IsListening = false;
                    callback(this);
                    break;
                }
            }
            else
            {
                if (leftComplete || rightComplete)
                {
                    IsListening = false;
                    callback(this);
                    break;
                }
            }

            leftPrevious = handLeftPos;
            rightPrevious = handRightPos;
            yield return null;
        }
    }*/



    public float NormalizationFactor { get; set; }
    public float Ymultiplier = 1;
    public float Yoffset = 0;
    public float XMultiplier = 1;
    public float XOffset = 0;
}
