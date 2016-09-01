using UnityEngine;
using System.Collections;
using System;

public class ShipWaiting : ShipState  {
  
    public float HowLong;
  //  float StartTime;

    // Use this for initialization
    void Start () {
        StartTime = Time.time;
    }
 
    public override void ShipUpdate () {
       // print("Ship update");
        if (StartTime + HowLong < Time.time) { print("startsailing"); stateMachine.StartSailing(); }
   
    }

   




}
