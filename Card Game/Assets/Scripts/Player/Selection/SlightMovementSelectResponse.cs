using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// English: A select resposne class that slightly move the object either towards a specific direction or towards a Transform when select or deselect. Use Bezier Curve
/// 日本語：選択された時、選択解除された時に、特定の方向、またはTransformまでちょっと移動する選択反応のクラス。Bezier Curveを使用する
/// </summary>
public class SlightMovementSelectResponse : MonoBehaviour, ISelectResponse
{
    public GameObject moveObject { get; set; }
    public bool initializedOnce { get; set; }

    [SerializeField]
    private BezierCurve bezierCurve;
    
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool useTowardsTransform;

    [Header("If towards is specified")]
    [SerializeField]
    private Transform towards;

    [Header("If local direction is specified")]
    [SerializeField]
    private Vector3 dir;
    [SerializeField]
    private float distance;

    private float t = 0;
    private bool isSelected = false;

    private void Update()
    {
        // if selected, continues to move towards the destination.
        // if unselected, continues to move back to the starting point.
        if (!initializedOnce) { return; }
        if(moveObject == null) { return; }
        if(isSelected && t > 1) { return; }
        if(!isSelected && t < 0) { return; }
        if(isSelected)
            t += Time.deltaTime * speed;
        else
            t -= Time.deltaTime * speed;

        MoveInBezierCurve(moveObject, t);
    }

    public void OnDeselect()
    {
        isSelected = false;
        if (t < 0) { t = 0; }
        if (t > 1) { t = 1; }
    }

    public void OnSelect()
    {
        isSelected = true;
        if(t < 0) { t = 0; }
        if(t > 1) { t = 1; }
        Initialize();
    }
    
    /// <summary>
    /// English: Initialize the start and end point of this movement
    /// 日本語：この動きのスタートとエンドポイントを初期化する
    /// </summary>
    private void Initialize()
    {
        // To prevent the starting point from continuously changing when (re)initialized
        if(!initializedOnce)
        {
            bezierCurve.controlPoints[0].position = moveObject.transform.position;
            bezierCurve.controlPoints[0].rotation = moveObject.transform.rotation;
        }
        initializedOnce = true;

        if (useTowardsTransform)
        {
            if (towards != null)
            {
                bezierCurve.controlPoints[bezierCurve.controlPoints.Length - 1].position = towards.position;
                bezierCurve.controlPoints[bezierCurve.controlPoints.Length - 1].position = towards.position;
            }
        }
        else
        {
            bezierCurve.controlPoints[bezierCurve.controlPoints.Length - 1].position = moveObject.transform.position + dir.normalized * distance;
            bezierCurve.controlPoints[bezierCurve.controlPoints.Length - 1].rotation = moveObject.transform.rotation;
        }
    }

    /// <summary>
    /// English: Lerp the object position and rotation according to the bezier curve
    /// 日本語：Bezier Curveに応じてオブジェクトの位置とローテーションをLerpする
    /// </summary>
    /// <param name="moveObject"></param>
    /// <param name="t"></param>
    private void MoveInBezierCurve(GameObject moveObject, float t)
    {
        if (moveObject == null) { return; }
        if (bezierCurve.controlPoints.Length == 4)
        {
            moveObject.transform.position = bezierCurve.CubicBezierCurveLerp(t);
            moveObject.transform.rotation = bezierCurve.GetCubicBezierCurveRotation(t);
        }
        else if (bezierCurve.controlPoints.Length == 3)
        {
            moveObject.transform.position = bezierCurve.QuadraticBezierCurveLerp(t);
            moveObject.transform.rotation = bezierCurve.GetQuadraticBezierCurveRotation(t);
        }
        else if (bezierCurve.controlPoints.Length == 2)
        {
            moveObject.transform.position = bezierCurve.LinearBezierCurveLerp(t);
            moveObject.transform.rotation = bezierCurve.GetLinearBezierCurveRotation(t);
        }
    }

    
}
