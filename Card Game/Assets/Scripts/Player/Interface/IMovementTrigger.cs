using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementTrigger : ITrigger
{
    public void InitializeMoveObjectTowards(GameObject moveObject, Transform destination);
    public void InitializeMoveObjectTowards(GameObject moveObject, Vector3 destination);

}
