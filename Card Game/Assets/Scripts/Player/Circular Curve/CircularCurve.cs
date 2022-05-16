using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularCurve : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform circleCenter;
    [SerializeField]
    private int segments;
    [SerializeField]
    private bool displayGizmos;

    public Vector3 CircularCurveLerp(float t)
    {
        Vector3 result = Vector3.zero;
        if (circleCenter == null || startPoint == null || endPoint == null) { return result; }
        Vector3 centerToStart = startPoint.position - circleCenter.position;
        Vector3 startToEnd = endPoint.position - startPoint.position;
        float radius = centerToStart.magnitude;
        Vector3 tempResult = startPoint.position + startToEnd * Mathf.Clamp01(t);
        Vector3 centerToTemp = tempResult - circleCenter.position;
        result = circleCenter.position + centerToTemp.normalized * radius;
        Debug.Log($"Start Point {startPoint.position}");
        Debug.Log($"End Point {endPoint.position}");
        Debug.Log($"Circle Center Point {circleCenter.position}");

        Debug.Log($"t is {t}");

        Debug.Log($"Center To Start {centerToStart}");
        Debug.Log($"Start To End {startToEnd}");
        Debug.Log($"tempResult {tempResult}");
        Debug.Log($"Result {result}");

        return result;
    }

    public Quaternion GetCircularCurveRotation(float t)
    {
        Quaternion result = new Quaternion();
        if (circleCenter == null || startPoint == null || endPoint == null) { return result; }
        Quaternion q0 = Quaternion.Lerp(startPoint.rotation, endPoint.rotation, t);
        result = q0;

        return result;

    }

    public void OnDrawGizmos()
    {
        if (!displayGizmos) { return; }
        if (segments == 0) { return; }
        Gizmos.color = Color.red;
        Vector3[] vertices = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            vertices[i] = CircularCurveLerp((float)i * 1 / segments);
        }

        for (int i = 0; i < segments - 1; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[i + 1]);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(circleCenter.position, endPoint.position);

    }
}
