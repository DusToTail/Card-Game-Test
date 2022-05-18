using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// *** MAY MAKE INTO ONE STATIC HELPER CLASS ***
/// English: Class that holds a circular curve and handles lerp operation
/// 日本語：円のカーブを持ち、Lerpを処理するクラス
/// </summary>
public class CircularCurve : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform circleCenter;
    [SerializeField]
    private int segments;
    [SerializeField]
    private bool displayGizmos;

    /// <summary>
    /// English: Lerp position calculated from start point to end point away from a circle center
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
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
        
        return result;
    }

    /// <summary>
    /// English: Lerp rotation calculated from start point to end point away from a circle center
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public Quaternion LerpCircularCurveRotation(float t)
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
        Vector3[] vertices = new Vector3[segments + 1];
        for (int i = 0; i < segments + 1; i++)
        {
            vertices[i] = CircularCurveLerp((float)i * 1 / segments);
        }

        for (int i = 0; i < segments; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[i + 1]);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(circleCenter.position, endPoint.position);

    }
}
