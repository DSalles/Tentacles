using UnityEngine;
using System.Collections;
using System;

public class ShipFiring : ShipState
{

    public float HowLong;
    // float StartTime;
    public float FirePause = 2;
    public float FireTime;
    public int howManyHits = 0;
    public float Speed = .01f;
    public Transform Target;
    public int HitsToKill = 3;
    public bool forward = true;
    public Transform[] Missiles;

    public float FireForce = 1f;
    // Use this for initialization
    void Start()
    {
        //  StartTime = Time.time;
    }

    public override void ShipUpdate()
    {
        // print("Ship update");
        if (StartTime + HowLong < Time.time) { print("startsailing"); stateMachine.StartSailing(); }


        if (FireTime + FirePause < Time.time)
            Fire();
    }

    private void Fire()
    {
        Transform candidateTarget1 = null;
        Transform candidateTarget2 = null;

        if (stateMachine.TentacleRight != null) candidateTarget1 = stateMachine.TentacleRight.TentacleAlive ? stateMachine.TentacleRight.BreakawayParts[stateMachine.TentacleRight.howManyHits].transform : null;
        if (stateMachine.TentacleLeft != null) candidateTarget2 = stateMachine.TentacleLeft.TentacleAlive ? stateMachine.TentacleLeft.BreakawayParts[stateMachine.TentacleLeft.howManyHits].transform : null;

        if (candidateTarget1 == null && candidateTarget2 == null) return;  // no more targets 

        if (candidateTarget1 != null && candidateTarget2 != null) // both tentacles have valid targets find the nearest

            Target = Vector3.Distance(candidateTarget1.position, Ship.transform.position) < Vector3.Distance(candidateTarget2.position, Ship.transform.position) ? candidateTarget1 : candidateTarget2;

        else Target = (candidateTarget1 != null) ? candidateTarget1 : candidateTarget2;
        FireTime = Time.time;
        Missiles[0].GetComponent<Collider>().enabled = true;
        Missiles[0].GetComponent<Rigidbody>().velocity = Vector3.zero;
        Missiles[0].position = Ship.transform.position;
        Missiles[0].LookAt(Target);
        Missiles[0].GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * FireForce * .5f);
    }
}