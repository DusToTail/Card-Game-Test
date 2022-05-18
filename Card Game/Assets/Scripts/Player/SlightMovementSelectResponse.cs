using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlightMovementSelectResponse : MonoBehaviour, ISelectResponse
{
    public GameObject moveObject { get; set; }
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
    private Direction dir;
    [SerializeField]
    private float distance;

    private float t = 0;
    private bool isSelected = false;
    private bool initializedOnce = false;

    public enum Direction
    {
        Forward,
        Backward,
        Left,
        Right,
        Up,
        Down,
        None
    };

    private void Start()
    {
    }

    private void Update()
    {
        if(moveObject == null) { return; }
        if(isSelected && t > 1) { return; }
        if(!isSelected && t < 0) { return; }
        if(isSelected)
            t += Time.deltaTime * speed;
        else
            t -= Time.deltaTime * speed;
        MoveInBezierCurve(moveObject, t);
        Debug.Log("Slightly moving." + " t is " + t);
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

    private void Initialize()
    {
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
            Vector3 chosenDir = Vector3.zero;
            switch (dir)
            {
                case Direction.Forward:
                    chosenDir = moveObject.transform.forward;
                    break;
                case Direction.Backward:
                    chosenDir = -moveObject.transform.forward;
                    break;
                case Direction.Left:
                    chosenDir = -moveObject.transform.right;
                    break;
                case Direction.Right:
                    chosenDir = moveObject.transform.right;
                    break;
                case Direction.Up:
                    chosenDir = moveObject.transform.up;
                    break;
                case Direction.Down:
                    chosenDir = -moveObject.transform.up;
                    break;
                case Direction.None:
                    break;
                default:
                    break;

            }
            bezierCurve.controlPoints[bezierCurve.controlPoints.Length - 1].position = moveObject.transform.position + chosenDir * distance;
            bezierCurve.controlPoints[bezierCurve.controlPoints.Length - 1].rotation = moveObject.transform.rotation;
        }
    }

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
