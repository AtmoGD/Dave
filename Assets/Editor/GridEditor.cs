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
                    IsDeleting = false;
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

            if (IsDeleting)
            {
                IsPlacing = false;
            }
        }

        if (IsPlacing)
        {
            GUILayout.Label("PLACING ACTIVE");
        }

        if (IsDeleting)
        {
            GUILayout.Label("DELETING ACTIVE");
        }

        ActiveEditorTracker.sharedTracker.isLocked = IsPlacing || IsDeleting;
    }

    public void OnSceneGUI()
    {
        WorldGrid worldGrid = (WorldGrid)target;

        worldGrid.InitGrid();

        LevelManager levelManager = FindObjectOfType<LevelManager>();

        if (IsPlacing)
        {
            Placeable PlaceObject = null;
            try
            {
                PlaceObject = (Placeable)source;
            }
            catch (System.Exception)
            {
                Debug.LogError("ERROR: No Placeable Selected");
                PlaceObject = null;
            }

            if (PlaceObject)
            {
                Vector2 mousePosition = Event.current.mousePosition;
                mousePosition = HandleUtility.GUIPointToWorldRay(mousePosition).origin;

                GridElement gridElement = worldGrid.GetGridElement(mousePosition);

                if (gridElement)
                {
                    if (!objectToPlace)
                        objectToPlace = Instantiate(((Placeable)source).prefab, worldGrid.transform);

                    Vector2Int gridPosition = gridElement.gridPosition;

                    Vector2 gridPositionWorld = gridElement.transform.position + (Vector3)worldGrid.GetObjectOffset(PlaceObject);

                    objectToPlace.transform.position = gridPositionWorld;

                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        worldGrid.PlaceObject(PlaceObject, gridPosition);
                        levelManager.LevelData.placedObjects.Add(new PlacedObject(PlaceObject, gridPosition));
                        Event.current.Use();
                    }
                }
                else
                {
                    DestroyImmediate(objectToPlace);
                }
            }
        }
        else if (IsDeleting)
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                Vector2 mousePosition = Event.current.mousePosition;
                mousePosition = HandleUtility.GUIPointToWorldRay(mousePosition).origin;

                GridElement gridElement = worldGrid.GetGridElement(mousePosition);

                if (gridElement && gridElement.ObjectOnGrid)
                {
                    PlaceableObject placeableObject = gridElement.ObjectOnGrid.GetComponent<PlaceableObject>();
                    if (placeableObject)
                    {
                        PlacedObject placed = levelManager.LevelData.placedObjects.Find(x => x.gridPosition == placeableObject.GridElement.gridPosition);
                        if (placed != null)
                        {
                            levelManager.LevelData.placedObjects.Remove(placed);
                            DestroyImmediate(placeableObject.gameObject);
                            worldGrid.LoadLevel();
                        }
                    }
                }
                Event.current.Use();
            }
        }
    }
}