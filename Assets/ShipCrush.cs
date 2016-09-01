using UnityEngine;
using System.Collections;
using System;

public class ShipCrush : MonoBehaviour {

    public ShipState CurrentShipState;

    public ShipFiring Firing;
    public ShipWaiting Waiting;
    public ShipSailing Sailing;
    public ShipWreck ShipWrecked;
    public ShipGrabbed ShipGrabbed;
    private bool ShipHittable = true;
    private bool WayPointable = true;
    public TentacleBehavior TentacleLeft;
    public TentacleBehavior TentacleRight;

    // Use this for initialization
    void Start ()
    {
        ChangeState(Waiting);
        //startPos = transform.position;
        //ShipStartOver();

    }

    internal void RestartShip()
    {
      
        ChangeState(Sailing);
        CurrentShipState.Restart(); 
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;      
    }

    internal void ReleaseShip()
    {
        ChangeState(Sailing);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    internal void StartFiring()
    {
        ChangeState(Firing);
        
    }

    internal void StartSailing()
    {
        print("start sailing");
        ChangeState(Sailing);
    }

    private void ChangeState(ShipState newState)
    {
        CurrentShipState = newState;
        CurrentShipState.StartTime = Time.time;
    }

    void OnTriggerEnter(Collider other)
    {
        //if (ShipWrecked || grabbed) return;
       string tag = other.gameObject.tag;

        switch (tag)
        {
            case "TentacleHand":
                if (ShipHittable) // to prevent too many triggers
                {
                    CurrentShipState.ShipHit(other);
                    StartCoroutine(TempDisableTrigger(ShipHittable,0.5f));
                }
                break;          
        }
    } 

    private IEnumerator TempDisableTrigger(bool trigger, float howLong)
    {
        trigger = false;
        yield return new WaitForSeconds(howLong);
        trigger = true;
    }

   
    internal void ShipGetGrabbed(TentacleBehavior tentacle)
    {
        ChangeState(ShipGrabbed);
       ShipGrabbed.GrabbingHand = tentacle;
//print("Assigned grabbing hand"+ tentacle.gameObject.ToString());
    }
    public void ShipWreck()
    {
        //WreckTime = Time.time;
        //ShipWrecked = true;
        ChangeState(ShipWrecked);
        
       GetComponent<Rigidbody>().useGravity = true;
       GetComponent<Rigidbody>().isKinematic = false;
    }

    // Update is called once per frame
    void Update () {
        CurrentShipState.ShipUpdate();
	}


    void ShipStartOver()
    {
        //ShipWrecked = false;
        //GetComponent<Renderer>().material = ShipStateMaterial[0];


    }
}
