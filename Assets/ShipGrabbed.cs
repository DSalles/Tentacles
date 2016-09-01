using UnityEngine;
using System.Collections;
using System;

public class ShipGrabbed : ShipState
{
    public float throwSpeed = 0.2f;
    public float Force = 1f;
    public float FreefallTime = 2f;

    public override void ShipUpdate()
    {
        if (!GrabbingHand.TentacleAlive) stateMachine.ReleaseShip();
        if (
            GrabbingHand.HandSpeed > throwSpeed) {
    //        print("handspeed" + GrabbingHand.HandSpeed);
            ShipTossed();
           }

    }

    private void ShipTossed()
    {
        Ship.transform.parent = null;
        
        Ship.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * Force*7);
        Ship.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * Force * 11);
        StartCoroutine(Freefall());
      
    }

    private IEnumerator Freefall()
    {
        yield return new WaitForSeconds(FreefallTime);
        stateMachine.RestartShip();
    }
}
