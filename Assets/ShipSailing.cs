using UnityEngine;
using System.Collections;
using System;

public class ShipSailing : ShipState  {
   
    public float SmashSpeed = 0.1f;
    public float slow = 0.02f;
    public float fast = 0.03f;
    public Transform[] Waypoints;
    public int CurrentWaypoint = 0;
    public int Left;
    public float FirePause = 2;
    public float FireTime;
    public int howManyHits = 0;
    public float Speed = .01f;
    public Transform Target;
    public int HitsToKill = 3;
    public bool forward = true;
    public Transform[] Missiles;
  //  internal float StartTime;
    public float FireForce = 1f;
    public float minWaypointDistance = 0.01f;

    public float HowLong = 1;
    // Use this for initialization
    void Start () {
        forward = true;
        StartPos = Ship.transform.position;
    }
    int plusOrMinus = 1;
    public override void ShipUpdate () {

        // print("sailing");
        // WAYPOINT NAVIGATION
        if (StartTime + HowLong < Time.time) { print("startFiring"); stateMachine.StartFiring(); }
        Vector3 tempLocalPosition;
        Vector3 tempWaypointPosition;
        tempLocalPosition = transform.position;
        tempWaypointPosition = Waypoints[CurrentWaypoint].position;
       
        // Is the distance between the agent and the current waypoint within the minWaypointDistance?
        if (Vector3.Distance(tempLocalPosition, tempWaypointPosition) <= minWaypointDistance)
            { 
              
                // Have we reached the last waypoint?
            if (CurrentWaypoint == Waypoints.Length - 1)
            { // If so, go back to the first waypoint and start over again
               // plusOrMinus = -1;
                CurrentWaypoint = 0;
                Ship.transform.position = StartPos; 
           //   print(" reached the end");               
            }
          //  else if (CurrentWaypoint == 0)
          //  {
          //      plusOrMinus = 1;
          ////    print("reached the beginning");
          //      CurrentWaypoint = 1;
          //  }
            else
                // If we haven't reached the Last waypoint, just move on to the next one
                CurrentWaypoint+= plusOrMinus;
            }

        // point to current waypoint
         Ship.transform.LookAt(Waypoints[CurrentWaypoint]);
         
        // move ship
        Ship.transform.Translate(0, 0, Speed);       

        //if (FireTime + FirePause < Time.time)
        //  Fire();
    }

    //private void Fire()
    //{
    //    Transform candidateTarget1 = null;
    //    Transform candidateTarget2 = null;

    //    if(stateMachine.TentacleRight !=null) candidateTarget1 = stateMachine.TentacleRight.TentacleAlive? stateMachine.TentacleRight.BreakawayParts[stateMachine.TentacleRight.howManyHits].transform : null;
    //    if(stateMachine.TentacleLeft != null) candidateTarget2 = stateMachine.TentacleLeft.TentacleAlive ? stateMachine.TentacleLeft.BreakawayParts[stateMachine.TentacleLeft.howManyHits].transform : null;

    //    if (candidateTarget1 == null && candidateTarget2 == null) return;  // no more targets 

    //    if (candidateTarget1 != null && candidateTarget2 != null) // both tentacles have valid targets find the nearest

    //        Target = Vector3.Distance(candidateTarget1.position, Ship.transform.position) < Vector3.Distance(candidateTarget2.position, Ship.transform.position) ? candidateTarget1 : candidateTarget2;
            
    //    else Target = (candidateTarget1 != null) ? candidateTarget1 : candidateTarget2;
    //    FireTime = Time.time;
    //    Missiles[0].GetComponent<Collider>().enabled = true;
    //    Missiles[0].GetComponent<Rigidbody>().velocity = Vector3.zero;
    //    Missiles[0].position = Ship.transform.position;
    //    Missiles[0].LookAt(Target);
    //    Missiles[0].GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * FireForce * .5f);


    //}
  
    //public override void WayPointIntersected()
    //{
    //    // print("WAYPOINT" + forward + CurrentWaypoint);

    //    if (CurrentWaypoint >= Waypoints.Length -1) { Left = -1; flip(); }
    //    else if (CurrentWaypoint <= 0) { Left = 1; flip(); }
    //    CurrentWaypoint +=Left;
             
                
    //}

    private void flip()
    {
       // Ship.transform.localScale = new Vector3(Ship.transform.localScale.x * -1, Ship.transform.localScale.y, Ship.transform.localScale.z);
    }

    public override void ShipHit(Collider other)
    {
        print("SHIP HIT");

        try { GrabbingHand = other.transform.parent.parent.GetComponent<TentacleBehavior>(); }
        catch { other.gameObject.SetActive(false); }// loose pieces of tentacle setting off triggers

        if (GrabbingHand.HandSpeed > SmashSpeed) // player moving hand fast so smash it
        {// smash
            print("Smash");
            StartCoroutine(Speedup());
            howManyHits++;
            try { Ship.GetComponent<Renderer>().material = ShipStateMaterial[howManyHits]; } catch (IndexOutOfRangeException) { print("Not enough materials in ship"); }
            if (howManyHits > HitsToKill-1) stateMachine.ShipWreck();
        }
        else
        {//grab     
            print("grab");       
            Ship.transform.parent = other.gameObject.transform;
            stateMachine.ShipGetGrabbed(other.transform.parent.parent.GetComponent<TentacleBehavior>());
        }
    }

    internal override void Restart()
    {
        howManyHits = 0;
        
        base.Restart();
    }


    private IEnumerator Speedup()
    {
        //print("speedup");
        Speed = fast;
        yield return new WaitForSeconds(2);
        Speed = slow;
    }


}
