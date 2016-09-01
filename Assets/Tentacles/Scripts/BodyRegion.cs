using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BodyRegion : BodyFilter {

    public float groundHeight;
    public float zOffset;
    public float radius;

    public bool closestToCenterOnly;

	Vector3 lastValidatedPosition = new Vector3(0,0,1000);


    public override void Validate(KinectBodiesReceiver.KinectInfo info,KinectBodiesReceiver.BodyData[] bodyData)
    {
        Vector3 fwd = transform.forward;
        fwd.y = 0;
        fwd.Normalize();

        Vector3 center = transform.position;
        center.y = groundHeight;
        center += fwd * zOffset;

        int closestIndex = -1;
        float closestDist = float.MaxValue;

        for (int i = 0; i < bodyData.Length; i++)
        {
            var data = bodyData[i];
            Vector3 pos = data.modifiedByMatrix ? data.position : info.matrix.MultiplyPoint3x4(data.position);

            pos.y = center.y;

            float dist = Vector3.Distance(pos, center);

            if (dist > radius)
            {
                data.valid = false;
            }
            else
            {
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestIndex = i;
                }
            }
        }

        if (closestToCenterOnly)
        {
            for (int i = 0; i < bodyData.Length; i++)
            {
                if (i != closestIndex)
                    bodyData[i].valid = false;
                else
                {
                    lastValidatedPosition = info.matrix.MultiplyPoint3x4(bodyData[i].position);
                }
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere (lastValidatedPosition, 0.25f);

        Vector3 fwd = transform.forward;
        fwd.y = 0;
        fwd.Normalize();

        Vector3 pos = transform.position;
        pos.y = groundHeight;
        pos += fwd * zOffset;

        //pos.y;


        DebugExtension.DrawCylinder(pos, pos + new Vector3(0, 2, 0), radius);
    }
#endif
}
