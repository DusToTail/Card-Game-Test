using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour, IMovementTrigger
{
    public GameObject nextMovementTrigger { get; set; }
    [SerializeField]
    private CircularCurve circularCurve;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform rotationTransform;
    private GameObject moveObject;

    private float t = 0;
    private bool movementFinished;

    private void Start()
    {
        movementFinished = true;
    }

    private void Update()
    {
        if (movementFinished) { return; }
        MoveInCircularCurve(moveObject, t);
        t += Time.deltaTime * speed;
        if (t >= 1)
        {
            movementFinished = true;
            if(nextMovementTrigger != null)
                InitializeNextMovementTrigger(nextMovementTrigger.GetComponent<IMovementTrigger>());
        }
    }

    public void InitializeMoveObjectTowards(GameObject moveObject, Transform destination)
    {
        t = 0;
        this.moveObject = moveObject;
        circularCurve.startPoint.position = moveObject.transform.position;
        circularCurve.startPoint.rotation = Quaternion.LookRotation(moveObject.transform.position - circularCurve.circleCenter.position, rotationTransform.up);
        circularCurve.endPoint.position = destination.position;
        circularCurve.endPoint.rotation = Quaternion.LookRotation(destination.transform.position - circularCurve.circleCenter.position, rotationTransform.up);
    }

    public void InitializeMoveObjectTowards(GameObject moveObject, Vector3 destination)
    {
        t = 0;
        this.moveObject = moveObject;
        circularCurve.startPoint.position = moveObject.transform.position;
        circularCurve.startPoint.rotation = Quaternion.LookRotation(moveObject.transform.position - circularCurve.circleCenter.position, rotationTransform.up);
        circularCurve.endPoint.position = destination;
        circularCurve.endPoint.rotation = Quaternion.LookRotation(destination - circularCurve.circleCenter.position, rotationTransform.up);
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
        circularCurve.startPoint.position = moveObject.transform.position;
        circularCurve.startPoint.rotation = Quaternion.LookRotation(moveObject.transform.position - circularCurve.circleCenter.position, rotationTransform.up);
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
