using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INeedSacrifice
{
    public bool IsSatisfied();
    public void Sacrifice(IHaveHealth[] sacrificeCount);
}
