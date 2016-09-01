using UnityEngine;
using System.Collections;
using System;

public class ShipWreck : ShipSailing  {
    public float secondsBeforeRecover = 2;
    public float WreckTime = 0;
    bool wrecked = false;
    // Use this for initialization
    void Start () {
	
	}

    
        public override void ShipUpdate () {
        if (!wrecked)
        {
            wrecked = true;
            WreckTime = Time.time;
        }
        if (WreckTime + secondsBeforeRecover < Time.time)
            stateMachine.RestartShip();
    }


    public override void WayPointIntersected()
    {
        throw new NotImplementedException();
    }

    public override void ShipHit(Collider other)
    {
      //nothing
    }
 


}
