using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class DataManager : EditorWindow
{
    [SerializeField] public PlayerData playerData = null;
    [SerializeField] private DataList dataList = null;
    [SerializeField] private string path = "player.dave";


    [MenuItem("Window/Data Manager")]
    public static void ShowWindow()
    {
        GetWindow<DataManager>("Data Manager");
    }

    private void OnEnable()
    {
        LoadData();
    }

    private void LoadData()
    {
        playerData = DataLoader.LoadData<PlayerData>(path);
        if (playerData == null)
        {
            playerData = new PlayerData();
            DataLoader.SaveData(playerData, path);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);

        path = EditorGUILayout.TextField("Path", path);

        GUILayout.Space(20);

        GUILayout.Label("Data Manager", EditorStyles.boldLabel);

        SerializedObject serializedObject = new SerializedObject(this);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("playerData"), true);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Load Data"))
        {
            LoadData();
        }

        if (GUILayout.Button("Save Data"))
        {
            DataLoader.SaveData<PlayerData>(playerData, path);
            LoadData();
        }

        if (GUILayout.Button("Delete Data"))
        {
            DataLoader.DeleteData(path);
            LoadData();
        }
    }
}
#endif