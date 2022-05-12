using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public Vector3 size;
    public Texture texture;
}
