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
    private bool wasLocked = false;
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

                    wasLocked = ActiveEditorTracker.sharedTracker.isLocked;
                    ActiveEditorTracker.sharedTracker.isLocked = true;
                }
                else
                {
                    if (objectToPlace != null)
                    {
                        DestroyImmediate(objectToPlace);
                        objectToPlace = null;
                    }
                    ActiveEditorTracker.sharedTracker.isLocked = wasLocked;
                }
            }
        }

        if (GUILayout.Button("Delete Object"))
        {
            IsDeleting = !IsDeleting;

            if (IsDeleting)
            {
                IsPlacing = false;

                wasLocked = ActiveEditorTracker.sharedTracker.isLocked;
                ActiveEditorTracker.sharedTracker.isLocked = true;
            }
            else
            {
                ActiveEditorTracker.sharedTracker.isLocked = wasLocked;
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

        // ActiveEditorTracker.sharedTracker.isLocked = IsPlacing || IsDeleting;
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
#if UNITY_EDITOR
                        EditorUtility.SetDirty(worldGrid.currentGameManager.CurrentLevelData);
                        worldGrid.currentGameManager.CurrentLevelData.placedObjects.Add(new PlacedObject(PlaceObject, gridPosition));
#else
                        GameManager.Instance.CurrentLevelData.placedObjects.Add(new PlacedObject(PlaceObject, gridPosition));
#endif
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
#if UNITY_EDITOR
                        EditorUtility.SetDirty(worldGrid.currentGameManager.CurrentLevelData);
                        PlacedObject placed = worldGrid.currentGameManager.CurrentLevelData.placedObjects.Find(x => x.gridPosition == placeableObject.GridElement.gridPosition);
#else
                        PlacedObject placed = GameManager.Instance.CurrentLevelData.placedObjects.Find(x => x.gridPosition == placeableObject.GridElement.gridPosition);
#endif

                        if (placed != null)
                        {
#if UNITY_EDITOR
                            EditorUtility.SetDirty(worldGrid.currentGameManager.CurrentLevelData);
                            worldGrid.currentGameManager.CurrentLevelData.placedObjects.Remove(placed);
#else
                            GameManager.Instance.CurrentLevelData.placedObjects.Remove(placed);
#endif
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