using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour, IMovementTrigger
{
    [SerializeField]
    private CircularCurve circularCurve;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform rotationTransform;
    private GameObject moveObject;

    private float t = 0;
    private bool movementFinished;

    private void Update()
    {
        if (movementFinished) { return; }
        MoveInCircularCurve(moveObject, t);
        t += Time.deltaTime * speed;
        if (t >= 1)
            movementFinished = true;

    }

    public void InitializeMoveObjectTowards(GameObject moveObject, Transform destination)
    {
        t = 0;
        this.moveObject = moveObject;
        circularCurve.startPoint.position = moveObject.transform.position;
        circularCurve.endPoint.position = destination.position;
    }

    public void InitializeMoveObjectTowards(GameObject moveObject, Vector3 destination)
    {
        t = 0;
        this.moveObject = moveObject;
        circularCurve.startPoint.position = moveObject.transform.position;
        circularCurve.endPoint.position = destination;
    }

    public void Trigger()
    {
        movementFinished = false;
    }

    private void MoveInCircularCurve(GameObject moveObject, float t)
    {
        if (moveObject == null) { return; }
        moveObject.transform.position = circularCurve.CircularCurveLerp(t);
        moveObject.transform.rotation = circularCurve.GetCircularCurveRotation(t);

    }
}
