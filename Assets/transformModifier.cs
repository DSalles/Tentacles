using UnityEngine;
using System.Collections;

public class transformModifier : MonoBehaviour {
    public float Yoffset;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, Yoffset, transform.position.z);
	}
}
