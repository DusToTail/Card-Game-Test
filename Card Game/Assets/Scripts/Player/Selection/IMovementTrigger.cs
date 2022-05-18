using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementTrigger : ITrigger
{
    public GameObject nextMovementTrigger { get; set; }
    public void InitializeMoveObject(GameObject moveObject);
    public void InitializeMoveObjectTowards(GameObject moveObject, Transform destination);
    public void InitializeMoveObjectTowards(GameObject moveObject, Vector3 destination);
    public void InitializeNextMovementTrigger(IMovementTrigger nextMovementTrigger);
    public void ReInitializeSelf();

}
