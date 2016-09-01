using UnityEngine;
using System.Collections;
using System;

public class BreakOff : MonoBehaviour {


    TentacleBehavior tentacle;
    Transform Parent;
    public Transform[] ChildrenWhenBroken;
   public  Transform[] ChildrenParents;
    Vector3 StartPos;
    private Quaternion StartRot;

    // Use this for initialization
    void Awake () {

        Parent = transform.parent;
        Array.Resize(ref ChildrenParents, ChildrenWhenBroken.Length);
        for (int i = 0; i < ChildrenParents.Length; i++) ChildrenParents[i] = ChildrenWhenBroken[i].transform.parent;
        print("breakoff childrenParent length = " + ChildrenParents.Length);
        StartPos = transform.localPosition;
        StartRot = transform.localRotation;
        ResetPart();
    }
    public void Register(TentacleBehavior behavior)
    {
        print("register");
        tentacle = behavior;
    }
   public void BreakOffPart(Vector3 missileVelocity){
        GetComponent<Rigidbody>().isKinematic = false;
        //  foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) mr.enabled = true;
        transform.parent = null;
        foreach (var child in ChildrenWhenBroken) child.parent = transform;
       
        Vector3 missileVector = new Vector3(missileVelocity.x, missileVelocity.y, 0);
        GetComponent<Rigidbody>().AddForce(missileVector * 20);
        GetComponent<Rigidbody>().AddTorque(missileVector * 20);
        StartCoroutine(Fall());

    }

   void OnTriggerEnter(Collider other)
   {
       if (tentacle && other.gameObject.tag == "Missile") tentacle.TentacleHit(this, other);
    
   }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(1.5f);
        Stop();
    }

    private void Stop()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
    }


    /// <summary>
    ///  not used
    /// </summary>


    // Update is called once per frame
    void Update () {
	
	}

    internal void UnBreakOffPart()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.parent = Parent;
        for (int i = 0; i < ChildrenWhenBroken.Length; i++) {

            try { ChildrenWhenBroken[i].parent = ChildrenParents[i]; } catch(NullReferenceException e) { print(tentacle.name.ToString()+e.ToString()); }
            ChildrenWhenBroken[i].localPosition = Vector3.zero;
            ChildrenWhenBroken[i].localRotation = Quaternion.Euler(Vector3.zero);
        }
        transform.localPosition = StartPos;
        transform.localRotation = StartRot;
    }

    private void ResetPart()
    {
        //foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) mr.enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
    }


}
