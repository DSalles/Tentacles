using UnityEngine;
using System.Collections;

public class lookAtSplineBox : MonoBehaviour {
  public  Transform target;
  public bool matchPosition = true;
  public float trackedValue = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
       if(matchPosition) transform.position = target.transform.position;

        transform.rotation = target.rotation;

      //  if(target.localEulerAngles.y >200)
     //  transform.localEulerAngles = new Vector3(target.localEulerAngles.x , 270, 90);
      //  else
    //   transform.localEulerAngles = new Vector3(540-target.localEulerAngles.x, 270, 90);

       //trackedValue = target.localEulerAngles.x;
        // transform.LookAt(target, Vector3.forward);
	}
}
