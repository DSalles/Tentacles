using UnityEngine;
using System.Collections;

public abstract class BodyFilter : MonoBehaviour {

  //  public abstract void Validate(BodyReceiver.KinectInfo info, BodyReceiver.BodyData[] bodyData);
    public abstract void Validate(KinectBodiesReceiver.KinectInfo info, KinectBodiesReceiver.BodyData[] bodyData);
}
