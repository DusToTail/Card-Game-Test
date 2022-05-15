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
        Vector3 sc = startPoint.position - circleCenter.position;
        Vector3 es = endPoint.position - startPoint.position;

        float radius = sc.magnitude;
        Vector3 tempResult = startPoint.position + es * Mathf.Clamp01(t);
        Vector3 tc = tempResult - circleCenter.position;
        result = circleCenter.position + tc.normalized * radius;

        Debug.Log(sc);
        Debug.Log(es);
        Debug.Log(radius);
        Debug.Log(tempResult);
        Debug.Log(tc);
        Debug.Log(result);
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
