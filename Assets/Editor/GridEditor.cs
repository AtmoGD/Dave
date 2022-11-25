using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGrid))]
public class GridEditor : Editor
{
    public Object source;
    [field: SerializeField] public bool IsPlacing { get; private set; } = false;
    [field: SerializeField] public bool IsDeleting { get; private set; } = false;
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

        if (GUILayout.Button("Load Level"))
        {
            worldGrid.LoadLevel();
        }

        GUILayout.Space(30);
        GUILayout.Label("Place Objects", EditorStyles.boldLabel);

        source = EditorGUILayout.ObjectField(source, typeof(Object), true);

        Placeable placeable = null;
        try
        {
            placeable = (Placeable)source;
        }
        catch (System.Exception)
        {
            placeable = null;
        }

        if (!placeable)
        {
            GUILayout.Label("No Placeable Selected");
        }
        else
        {
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

        if (GUILayout.Button("Delete Object"))
        {
            IsDeleting = !IsDeleting;
        }

        if (IsPlacing)
        {
            GUILayout.Label("PLACING ACTIVE");
        }

        if (IsDeleting)
        {
            GUILayout.Label("DELETING ACTIVE");
        }

    }

    public void OnSceneGUI()
    {

        Placeable PlaceObject = null;
        try
        {
            PlaceObject = (Placeable)source;
        }
        catch (System.Exception)
        {
            PlaceObject = null;
        }

        WorldGrid worldGrid = (WorldGrid)target;

        if (PlaceObject != null && objectToPlace != null && IsPlacing)
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
                    objectToPlace.transform.parent = worldGrid.transform;
                    worldGrid.PlaceObject(objectToPlace, gridElements);
                    PlacedObject placedObject = new PlacedObject(PlaceObject, gridElement.gridPosition);
                    worldGrid.LevelData.placedObjects.Add(placedObject);
                }
                Event.current.Use();
            }
        }

        if (IsDeleting)
        {

            Vector2 mousePosition = Event.current.mousePosition;
            Vector2 worldPosition = HandleUtility.GUIPointToWorldRay(mousePosition).origin;

            worldGrid.InitGrid();

            GridElement gridElement = worldGrid.GetGridElement(worldPosition, true);


            if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp)
            {
                Event.current.Use();
                if (gridElement != null && gridElement.ObjectOnGrid != null)
                {
                    GameObject objectToDelete = gridElement.ObjectOnGrid;

                    PlaceableObject placeableObject = gridElement.ObjectOnGrid.GetComponent<PlaceableObject>();
                    if (placeableObject != null)
                    {
                        foreach (GridElement gridElem in placeableObject.placedOnGridElements)
                        {
                            gridElem.ObjectOnGrid = null;
                        }
                    }

                    foreach (PlacedObject placedObject in worldGrid.LevelData.placedObjects)
                    {
                        List<GridElement> placedGridElements = worldGrid.GetGridElements(placedObject.gridPosition, placedObject.placeable.size);
                        if (placedGridElements.Contains(gridElement))
                        {
                            worldGrid.LevelData.placedObjects.Remove(placedObject);
                            break;
                        }
                    }

                    DestroyImmediate(objectToDelete);
                }
            }
        }
    }
}