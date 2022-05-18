using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhaseController
{
    public void ExecuteCurrentPhase();
    public void NextPhase();
}
