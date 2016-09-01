using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpline))]
public class BezierCurveInspector : Editor
{

    private BezierSpline curve;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private const int steps =4;

    private void OnSceneGUI()
    {
        curve = target as BezierSpline;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        //Vector3 p0 = ShowPoint(0);
        //Vector3 p1 = ShowPoint(1);
        //Vector3 p2 = ShowPoint(2);

        Handles.color = Color.blue;
        Vector3 lineStart = curve.GetPoint(0f);
        for (int i = 1; i <= steps; i++)
        {
            Vector3 lineEnd = curve.GetPoint(i /(float) steps);
            Handles.DrawLine(lineStart, lineEnd);
            lineStart = lineEnd;
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
}