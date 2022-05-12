using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerDeck))]
public class PlayerDeckEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerDeck myDeck = (PlayerDeck)target;
        DrawDefaultInspector();
        EditorGUILayout.LabelField("Current Count", myDeck.transform.childCount.ToString());
        for(int index = myDeck.transform.childCount - 1; index >= 0; index --)
        {
            EditorGUILayout.LabelField($"Card index {index}", myDeck.GetDeckContents()[index]);
        }
        if (GUILayout.Button("Build Deck"))
        {
            myDeck.BuildDeck(myDeck.deck);
        }
        if(GUILayout.Button("Shuffle Deck"))
        {
            myDeck.ShuffleDeck();
        }
        if (GUILayout.Button("Clear Deck"))
        {
            myDeck.ClearDeck();
        }
    }
}
