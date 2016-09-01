using UnityEngine;
using System.Collections;

public class Smoother : MonoBehaviour
{
    public Transform target;
    public Transform secondRef;
    public float closenessToSecond = 0.5f;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    void Update()
    {
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, 0));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        if (secondRef)
        { print("secondRef" + secondRef.name.ToString());
            float Ypos = Mathf.Lerp(target.position.y, secondRef.position.y, closenessToSecond);
            transform.position = new Vector3(transform.position.x, Ypos, transform.position.z);
        }
    }
}