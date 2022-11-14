using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(WorldGrid))]
public class GridEditor : Editor
{
    [SerializeField] public int elementCount = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WorldGrid worldGrid = (WorldGrid)target;

        EditorGUILayout.LabelField("Number of Elements", worldGrid.ElementCount.ToString());

        if (GUILayout.Button("Create Grid"))
        {
            worldGrid.CreateGrid();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            worldGrid.ClearGrid();
        }
    }
}
#endif