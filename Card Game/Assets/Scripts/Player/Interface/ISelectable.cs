using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public void OnSelect();

    public void OnDeselect();

    public void OnClick();
}
