using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayBezierMovement : MonoBehaviour, IMovementTrigger
{
    public GameObject nextMovementTrigger { get; set; }

    [SerializeField]
    private BezierCurve bezierCurve;
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool isLooped;
    private GameObject moveObject;

    private float t = 0;
    private bool isReversed = false;
    private bool movementFinished;

    private void Start()
    {
        movementFinished = true;
    }

    private void Update()
    {
        if (movementFinished) { return; }
        // Movement function
        MoveInBezierCurve(moveObject, t);
        if(!isReversed)
            t += Time.deltaTime * speed;
        else
            t -= Time.deltaTime * speed;
        if (t >= 1 && !isReversed)
            isReversed = true;
        else if(t < 0 && isReversed && isLooped)
            isReversed = false;

        if (t < 0 && isReversed && !isLooped)
        {
            // Movement function
            MoveInBezierCurve(moveObject, t);
            movementFinished = true;
            moveObject = null;
            if (nextMovementTrigger != null)
                InitializeNextMovementTrigger(nextMovementTrigger.GetComponent<IMovementTrigger>());
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


    public void InitializeMoveObject(GameObject moveObject)
    {
        t = 0;
        this.moveObject = moveObject;
        bezierCurve.controlPoints[0].position = moveObject.transform.position;
        bezierCurve.controlPoints[0].rotation = moveObject.transform.rotation;
    }

    public void InitializeMoveObjectTowards(GameObject moveObject, Transform destination)
    {
        t = 0;
        this.moveObject = moveObject;
        bezierCurve.controlPoints[0].position = moveObject.transform.position;
        bezierCurve.controlPoints[0].rotation = moveObject.transform.rotation;
        bezierCurve.controlPoints[bezierCurve.controlPoints.Length - 1].position = destination.position;
        bezierCurve.controlPoints[bezierCurve.controlPoints.Length - 1].rotation = destination.rotation;
    }

    public void InitializeMoveObjectTowards(GameObject moveObject, Vector3 destination)
    {
        t = 0;
        this.moveObject = moveObject;
        bezierCurve.controlPoints[0].position = moveObject.transform.position;
        bezierCurve.controlPoints[0].rotation = moveObject.transform.rotation;
        bezierCurve.controlPoints[bezierCurve.controlPoints.Length - 1].position = destination;
    }

    public void InitializeNextMovementTrigger(IMovementTrigger movementTrigger)
    {
        if (movementTrigger == null) { return; }
        movementTrigger.ReInitializeSelf();
        movementTrigger.Trigger();

    }

    public void ReInitializeSelf()
    {
        t = 0;
        bezierCurve.controlPoints[0].position = moveObject.transform.position;
    }

    public void Trigger()
    {
        movementFinished = false;
        isReversed = false;
    }


}
