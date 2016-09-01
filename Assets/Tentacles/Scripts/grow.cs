using UnityEngine;
using System.Collections;
using System;

public class grow : MonoBehaviour {

    public string growName;
 //   public Vector3 StartSize;
  //  public PartSize size;
   public float restartTime;
    public float restartPause = 1;
	// Use this for initialization
	void Start () {
      //  StartSize = transform.localScale;
        startPos = transform.localPosition;
        transform.position -= new Vector3(0, .05F, 0);
     //   print("StartSize" + StartSize.x);
	}

    public Vector3 offset;
    private Vector3 startPos;

    public bool Restarted = false;

    // Update is called once per frame
    void Update () {
if(transform.localPosition.y < startPos.y)
            transform.localPosition += offset;// * transform.localScale.x;

	}

    internal void Restart()
    {
        if (Restarted) return;
        Restarted = true;
        restartTime = Time.time;
        transform.localPosition = startPos;
  //      transform.localScale = StartSize;
   //     print(growName+"local scale is " + StartSize.x);
        print("RESTART TEBNTACLE");     
    }


}
public enum PartSize : int
{
    Small=1, Medium, Large
}