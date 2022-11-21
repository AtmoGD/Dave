using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGrid))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WorldGrid worldGrid = (WorldGrid)target;

        if (worldGrid.ElementCount <= 0)
            worldGrid.InitGrid();

        EditorGUILayout.LabelField("Number of Elements", worldGrid.ElementCount.ToString());

        if (GUILayout.Button("Create Grid"))
        {
            worldGrid.CreateGrid();
        }

        if (GUILayout.Button("Delete Grid"))
        {
            worldGrid.DeleteAllChildren();
        }
    }
}