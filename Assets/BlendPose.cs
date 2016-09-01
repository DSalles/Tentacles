using UnityEngine;
using System.Collections;

public class BlendPose : MonoBehaviour
{
    public Transform Hand;
    public Transform zeroPoseTarget;
    public Transform highPoseTarget;
    public Transform farPoseTarget;
    public Transform zeroHandTarget;
    public Transform highHandTarget;
    public Transform farHandTarget;
    public float howFarAlongX;
    public float howFarAlongY;

    void Update()
    {
        float handX = Mathf.Abs(Hand.position.x - zeroHandTarget.position.x);
        float reachX = Mathf.Abs(farHandTarget.position.x - zeroHandTarget.position.x);
        howFarAlongX = Mathf.Max(.001f, handX) / Mathf.Max(.001f, reachX);

        Vector3 spacePosX = Vector3.Lerp(zeroPoseTarget.position, farPoseTarget.position, howFarAlongX);

        float handY = Mathf.Abs(Hand.position.y - zeroHandTarget.position.y);
        float reachY = Mathf.Abs(highHandTarget.position.y - zeroHandTarget.position.y);
      
        
        
        howFarAlongY =  Mathf.Max(.001f, handY) /  Mathf.Max(.001f, reachY);


        Vector3 spacePosY = Vector3.Lerp(zeroPoseTarget.position, highPoseTarget.position, howFarAlongY);

        float howFarBetweenSpacePoints =  Mathf.Max(.001f, howFarAlongY)/ Mathf.Max(.001f, (howFarAlongX+howFarAlongY));


        transform.position = Vector3.Lerp(spacePosX, spacePosY, howFarBetweenSpacePoints);
    }
}