using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// *** MAY MAKE INTO ONE STATIC HELPER CLASS ***
/// English: Class that holds a bezier curve and handles lerp operation. Support Cubic, Quadratic, Linear
/// 日本語：Bezierカーブを持ち、Lerpを処理するクラス
/// </summary>
public class BezierCurve : MonoBehaviour
{
    public Transform[] controlPoints;
    [SerializeField]
    private int segments;
    [SerializeField]
    private bool displayGizmos;

    /// <summary>
    /// English: Cubic Lerp position calculated from start point to end point.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
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

    /// <summary>
    /// English: Quadratic Lerp position calculated from start point to end point.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
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

    /// <summary>
    /// English: Linear Lerp position calculated from start point to end point.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public Vector3 LinearBezierCurveLerp(float t)
    {
        Vector3 result = Vector3.zero;
        if (controlPoints == null) { return result; }
        if (controlPoints.Length < 2) { return result; }
        t = Mathf.Clamp01(t);
        Vector3 p0 = controlPoints[0].position;
        Vector3 p1 = controlPoints[1].position;

        result = (1 - t) * p0 + t * p1;
        return result;
    }

    /// <summary>
    /// English: Cubic Lerp rotation calculated from start point to end point.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public Quaternion GetCubicBezierCurveRotation(float t)
    {
        Quaternion result = new Quaternion();
        if (controlPoints == null) { return result; }
        if (controlPoints.Length < 4) { return result; }
        t = Mathf.Clamp01(t);
        Quaternion q0 = Quaternion.Lerp(controlPoints[0].rotation, controlPoints[1].rotation, t);
        Quaternion q1 = Quaternion.Lerp(controlPoints[1].rotation, controlPoints[2].rotation, t);
        Quaternion q2 = Quaternion.Lerp(controlPoints[2].rotation, controlPoints[3].rotation, t);
        Quaternion q01 = Quaternion.Lerp(q0, q1, t);
        Quaternion q12 = Quaternion.Lerp(q1, q2, t);
        Quaternion q012 = Quaternion.Lerp(q01, q12, t);

        result = q012 ;
        return result;
    }

    /// <summary>
    /// English: Quadratic Lerp rotation calculated from start point to end point.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public Quaternion GetQuadraticBezierCurveRotation(float t)
    {
        Quaternion result = new Quaternion();
        if (controlPoints == null) { return result; }
        if (controlPoints.Length < 3) { return result; }
        t = Mathf.Clamp01(t);
        Quaternion q0 = Quaternion.Lerp(controlPoints[0].rotation, controlPoints[1].rotation, t);
        Quaternion q1 = Quaternion.Lerp(controlPoints[1].rotation, controlPoints[2].rotation, t);
        Quaternion q01 = Quaternion.Lerp(q0, q1, t);
        result = q01;
        return result;
    }

    /// <summary>
    /// English: Linear Lerp rotation calculated from start point to end point.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public Quaternion GetLinearBezierCurveRotation(float t)
    {
        Quaternion result = new Quaternion();
        if (controlPoints == null) { return result; }
        if (controlPoints.Length < 2) { return result; }
        t = Mathf.Clamp01(t);
        Quaternion q0 = Quaternion.Lerp(controlPoints[0].rotation, controlPoints[1].rotation, t);
        result = q0;
        return result;
    }


    public void OnDrawGizmos()
    {
        if (!displayGizmos) { return; }
        if(segments == 0) { return; }
        Gizmos.color = Color.red;
        Vector3[] vertices = new Vector3[segments + 1]; 
        for(int i = 0; i < segments + 1; i++)
        {
            if (controlPoints.Length == 4)
                vertices[i] = CubicBezierCurveLerp((float)i * 1 / segments);
            else if (controlPoints.Length == 3)
                vertices[i] = QuadraticBezierCurveLerp((float)i * 1 / segments);
            else if (controlPoints.Length == 2)
                vertices[i] = LinearBezierCurveLerp((float)i * 1 / segments);
        }

        for(int i = 0; i < segments; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[i + 1]);
        }
    }


}
