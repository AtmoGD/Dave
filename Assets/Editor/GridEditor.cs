using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGrid))]
public class GridEditor : Editor
{
    public Object source;
    [field: SerializeField] public bool IsPlacing { get; private set; } = false;
    [SerializeField] private GameObject objectToPlace = null;
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

        GUILayout.Space(30);
        GUILayout.Label("Place Objects", EditorStyles.boldLabel);

        source = EditorGUILayout.ObjectField(source, typeof(Object), true);

        Placeable placeable = (Placeable)source;
        if (!placeable)
        {
            GUILayout.Label("No Placeable Selected");
        }

        if (IsPlacing)
        {
            GUILayout.Label("IS PLACING");
        }

        if (GUILayout.Button("Place Object"))
        {
            IsPlacing = !IsPlacing;

            if (IsPlacing)
            {
                objectToPlace = Instantiate(((Placeable)source).prefab, worldGrid.transform);
            }
            else
            {
                if (objectToPlace != null)
                {
                    DestroyImmediate(objectToPlace);
                    objectToPlace = null;
                }
            }
        }
    }

    public void OnSceneGUI()
    {
        Placeable PlaceObject = (Placeable)source;

        if (!PlaceObject)
            return;

        WorldGrid worldGrid = (WorldGrid)target;

        if (objectToPlace != null && IsPlacing)
        {
            Vector2 mousePosition = Event.current.mousePosition;
            Vector2 worldPosition = HandleUtility.GUIPointToWorldRay(mousePosition).origin;

            worldGrid.InitGrid();

            GridElement gridElement = worldGrid.GetGridElement(worldPosition, true);

            objectToPlace.transform.position = gridElement.transform.position + (Vector3)worldGrid.GetObjectOffset(PlaceObject);

            if (Event.current.type == EventType.MouseDown && IsPlacing)
            {
                List<GridElement> gridElements = worldGrid.GetGridElements(gridElement.transform.position, PlaceObject.size);

                bool isPlaceable = worldGrid.IsObjectPlaceable(PlaceObject, gridElements);

                if (isPlaceable)
                {
                    objectToPlace.transform.parent = gridElement.transform;
                    worldGrid.PlaceObject(objectToPlace, gridElements);
                    Event.current.Use();
                }
            }
        }
    }
}