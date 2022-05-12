using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform[] controlPoints;
    [SerializeField]
    private int segments;
    [SerializeField]
    private bool displayGizmos;

    public Vector3 CubicBezierCurveLerp(float t)
    {
        Vector3 result = Vector3.zero;
        if (controlPoints == null) { return result; }
        if (controlPoints.Length < 4) { return result; }
        t = Mathf.Clamp01(t);
        Vector3 p0 = controlPoints[0].position;
        Vector3 p1 = controlPoints[1].position;
        Vector3 p2 = controlPoints[2].position;
        Vector3 p3 = controlPoints[3].position;

        result = ((1 - t) * (1 - t) * (1 - t) * p0) + (3 * (1 - t) * (1 - t) * t * p1) + (3 * (1 - t) * t * t * p2) + (t * t * t * p3);
        return result;
    }

    public Vector3 QuadraticBezierCurveLerp(float t)
    {
        Vector3 result = Vector3.zero;
        if (controlPoints == null) { return result; }
        if (controlPoints.Length < 3) { return result; }
        t = Mathf.Clamp01(t);
        Vector3 p0 = controlPoints[0].position;
        Vector3 p1 = controlPoints[1].position;
        Vector3 p2 = controlPoints[2].position;

        result = ((1 - t) * (1 - t) * p0) + (2 * (1 - t) * t * p1) + (t * t * p2);
        return result;
    }

    public Vector3 GetCubicBezierCurveDirection(float t)
    {
        Vector3 result = Vector3.zero;
        if (controlPoints == null) { return result; }
        if (controlPoints.Length < 4) { return result; }
        t = Mathf.Clamp01(t);
        float v = 0;
        if(segments == 0)
        {
            v = Mathf.Clamp01(t - 0.1f);
        }
        else
        {
            v = Mathf.Clamp01(t - 1/segments);
        }
        Vector3 p0 = controlPoints[0].position;
        Vector3 p1 = controlPoints[1].position;
        Vector3 p2 = controlPoints[2].position;
        Vector3 p3 = controlPoints[3].position;

        result = ((1 - t) * (1 - t) * (1 - t) * p0) + (3 * (1 - t) * (1 - t) * t * p1) + (3 * (1 - t) * t * t * p2) + (t * t * t * p3);
        Vector3 prev = ((1 - v) * (1 - v) * (1 - v) * p0) + (3 * (1 - v) * (1 - v) * v * p1) + (3 * (1 - v) * v * v * p2) + (v * v * v * p3);

        return (result - prev).normalized;
    }

    public Vector3 GetQuadraticBezierCurveDirection(float t)
    {
        Vector3 result = Vector3.zero;
        if (controlPoints == null) { return result; }
        if (controlPoints.Length < 3) { return result; }
        t = Mathf.Clamp01(t);
        float v = 0;
        if (segments == 0)
        {
            v = Mathf.Clamp01(t - 0.1f);
        }
        else
        {
            v = Mathf.Clamp01(t - 1 / segments);
        }
        Vector3 p0 = controlPoints[0].position;
        Vector3 p1 = controlPoints[1].position;
        Vector3 p2 = controlPoints[2].position;

        result = ((1 - t) * (1 - t) * p0) + (2 * (1 - t) * t * p1) + (t * t * p2);
        Vector3 prev = ((1 - v) * (1 - v) * p0) + (2 * (1 - v) * v * p1) + (v * v * p2);
        return (result - prev).normalized;
    }


    public void OnDrawGizmos()
    {
        if (!displayGizmos) { return; }
        if(segments == 0) { return; }
        Gizmos.color = Color.red;
        Vector3[] vertices = new Vector3[segments]; 
        for(int i = 0; i < segments; i++)
        {
            if(controlPoints.Length == 4)
                vertices[i] = CubicBezierCurveLerp((float)i * 1 / segments);
            else if(controlPoints.Length == 3)
                vertices[i] = QuadraticBezierCurveLerp((float)i * 1 / segments);
        }

        for(int i = 0; i < segments - 1; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[i + 1]);
        }
    }


}
