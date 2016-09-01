using UnityEngine;
using System.Collections;
using System;

public class TentacleBehavior : MonoBehaviour {

    public BreakOff[] BreakawayParts;
    public int howManyHits = 0;

    public BezierSpline ThisBezier;
    public float HandSpeed;
    public Handedness hand;

    private Vector3 startPos;
    internal bool TentacleAlive;

    float TriggerDisablePause = .5f;
    public float KilledTime;


    public bool TentacleHittable = true;

    public bool Growing = false;


    // Use this for initialization
    void Awake () {

      // startPos = transform.position;
	foreach(BreakOff b in BreakawayParts)
        {
            b.GetComponent<BreakOff>().Register(this);
        }
        TentacleAlive = true; 
	}
	
	// Update is called once per frame
	public void UpdateTentacle () {
       
  
    }
    private IEnumerator TempDisableTrigger(bool trigger, float howLong)
    {
        trigger = false;
        yield return new WaitForSeconds(howLong);
        trigger = true;
    }

    internal void TentacleHit(BreakOff breakOffPart, Collider missile)
    {
        
        if ( !TentacleAlive ) return;

        if (TentacleHittable)
        {
            if (breakOffPart != BreakawayParts[howManyHits]) return; // keep the parts breaking off in sequence
            StartCoroutine(TempDisableTrigger(TentacleHittable, 1f));
           
            
            breakOffPart.BreakOffPart(missile.GetComponent<Rigidbody>().velocity);
            howManyHits++;
            if (howManyHits == BreakawayParts.Length)
            {
                TentacleKilled();
            }
            missile.GetComponent<Collider>().enabled = false;
        }
         
        } 


    public void TentacleKilled()
    {
        print("Tentacle killed" + hand.ToString());
        KilledTime = Time.time;
        TentacleAlive = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;

      //  StartCoroutine(TentacleStartOver());
    }

    public void TentacleStartOver()
    {
         
        print("tentacle startover()");
     //   transform.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = TentacleStateMaterial[0];
        howManyHits = 0;

       foreach(BreakOff breakOffPart in BreakawayParts) breakOffPart.UnBreakOffPart();

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;

        ThisBezier.Restart(this);
        TentacleAlive = true;  
    }

    internal void Ungrow(float playerMissingTimeLimit)
    {
        print("UNGROWING");
    }

    public enum Handedness
    {
        Left, Right
    }

    internal void DoneGrowing(bool growing)
    { 
        Growing = growing;
        print("tentacle grow" + Growing);
    }
}
