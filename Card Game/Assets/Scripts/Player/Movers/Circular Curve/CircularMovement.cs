using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// *** MAY MAKE INTO ONE STATIC HELPER CLASS ***
/// English: Mover class that handles circular curve movement
/// 日本語：円の動きを処理するMoverクラス
/// </summary>
public class CircularMovement : MonoBehaviour, IMovementTrigger
{
    public GameObject nextMovementTrigger { get; set; }
    public bool isFinished { get; set; }
    [SerializeField]
    private CircularCurve circularCurve;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform rotationTransform;
    private GameObject moveObject;

    private float t = 0;

    private void Awake()
    {
        isFinished = true;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (isFinished) { return; }
        MoveInCircularCurve(moveObject, t);
        t += Time.deltaTime * speed;
        if (t >= 1)
        {
            MoveInCircularCurve(moveObject, t);
            isFinished = true;
            moveObject = null;
            if(nextMovementTrigger != null)
                InitializeNextMovementTrigger(nextMovementTrigger.GetComponent<IMovementTrigger>());
        }
    }

    public void InitializeMoveObject(GameObject moveObject)
    {
        t = 0;
        this.moveObject = moveObject;
        circularCurve.startPoint.position = moveObject.transform.position;
        circularCurve.startPoint.rotation = Quaternion.LookRotation(moveObject.transform.position - circularCurve.circleCenter.position, rotationTransform.up);
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
        isFinished = false;
    }

    private void MoveInCircularCurve(GameObject moveObject, float t)
    {
        if (moveObject == null) { return; }
        moveObject.transform.position = circularCurve.CircularCurveLerp(t);
        moveObject.transform.rotation = circularCurve.LerpCircularCurveRotation(t);

    }
}
