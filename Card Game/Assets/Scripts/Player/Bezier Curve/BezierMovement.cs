using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierMovement : MonoBehaviour, IMovementTrigger
{
    [SerializeField]
    private BezierCurve bezierCurve;
    [SerializeField]
    private float speed;
    private GameObject moveObject;

    private float t = 0;
    private bool movementFinished;

    private void Update()
    {
        if (movementFinished) { return; }
        MoveInBezierCurve(moveObject, t);
        t += Time.deltaTime * speed;
        if(t >= 1)
            movementFinished = true;

    }

    public void InitializeMoveObjectTowards(GameObject moveObject, Transform destination)
    {
        t = 0;
        this.moveObject = moveObject;
        bezierCurve.controlPoints[0].position = moveObject.transform.position;
        bezierCurve.controlPoints[bezierCurve.controlPoints.Length - 1].position = destination.position;
    }

    public void InitializeMoveObjectTowards(GameObject moveObject, Vector3 destination)
    {
        t = 0;
        this.moveObject = moveObject;
        bezierCurve.controlPoints[0].position = moveObject.transform.position;
        bezierCurve.controlPoints[bezierCurve.controlPoints.Length - 1].position = destination;
    }

    public void Trigger()
    {
        movementFinished = false;
    }

    private void MoveInBezierCurve(GameObject moveObject, float t)
    {
        if(moveObject == null) { return; }
        if(bezierCurve.controlPoints.Length == 4)
        {
            moveObject.transform.position = bezierCurve.CubicBezierCurveLerp(t);
            moveObject.transform.rotation = bezierCurve.GetCubicBezierCurveRotation(t);
        }
        else if(bezierCurve.controlPoints.Length == 3)
        {
            moveObject.transform.position = bezierCurve.QuadraticBezierCurveLerp(t);
            moveObject.transform.rotation = bezierCurve.GetQuadraticBezierCurveRotation(t);
        }
        
    }

}
