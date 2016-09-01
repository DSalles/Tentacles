using UnityEngine;
using System.Collections;

public class HeadMover : MonoBehaviour
{
    public string HeadName;
    public float ActualHeight = 1.75f;
    private float Offset = 1.21f;
    private float down;
    private float HeightSubtraction = 4f;
    public float NormalizationFactor = 2;
    public float StationIndex;

    /// <summary>
    /// moves an object based on the Kinect parts /// </summary>
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (GameObject.Find(HeadName))
        {   
            Vector3 refPos = GameObject.Find(HeadName).transform.position;
            //   refPos = new Vector3(refPos.x, refPos.y - 6.15f, refPos.z);  // bring height down to start from 0
            transform.position = new Vector3(refPos.x, (refPos.y - HeightSubtraction) * NormalizationFactor * 2, refPos.z - StationIndex * Offset);

            ActualHeight = GameObject.Find(HeadName).transform.position.y;
        }
    }
}
