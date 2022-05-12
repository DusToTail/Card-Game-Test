using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SquirrelDeck))]
public class SquirrelDeckEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SquirrelDeck mySquirrelDeck = (SquirrelDeck)target;
        DrawDefaultInspector();
        EditorGUILayout.LabelField("Current Count", mySquirrelDeck.transform.childCount.ToString());
        if(GUILayout.Button("Build Deck"))
        {
            mySquirrelDeck.BuildDeck(mySquirrelDeck.buildNum);
        }

        if(GUILayout.Button("Clear Deck"))
        {
            mySquirrelDeck.ClearDeck();
        }
    }

    
}
