using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Board myBoard = (Board)target;

        if (GUILayout.Button("Generate Board"))
        {
            myBoard.CreateTiles();
        }
        else if (GUILayout.Button("Reveal Board"))
        {
            myBoard.RevealBoard();
        }
        else if (GUILayout.Button("Reveal Board DEBUG"))
        {
            myBoard.RevealBoard_Debug();
        }
    }
}
