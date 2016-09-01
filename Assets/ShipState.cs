using UnityEngine;
using System.Collections;
using System;

public class ShipState : MonoBehaviour {

    public TentacleBehavior GrabbingHand;
    public Vector3 StartPos;
    public ShipCrush stateMachine;
    public UnityEngine.Material[] ShipStateMaterial;
    public Transform Ship;
    internal float StartTime;

    public float SecondsBeforeNewShip = 1;



    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	public virtual void ShipUpdate () {
	// sail ship. etc
	}


    public virtual void WayPointIntersected()
    {
       
    }

    public virtual void ShipHit(Collider other)
    {
    }

    internal virtual void ResetStartPos()
    {
       Ship.transform.position = StartPos;
    }

    internal virtual void Restart()
    {
        ResetStartPos();
       
    }
}
