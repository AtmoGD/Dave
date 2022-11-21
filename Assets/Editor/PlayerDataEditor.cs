using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerDataEditor : Editor
{
    public string path = "player.dave";
    private PlayerController playerController = null;

    private void OnEnable()
    {
        playerController = (PlayerController)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);

        path = EditorGUILayout.TextField("Path", path);

        if (GUILayout.Button("Load Data"))
        {
            playerController.LoadData(path);
        }

        if (GUILayout.Button("Save Data"))
        {
            playerController.SaveData(path);
        }

        base.OnInspectorGUI();

    }
}
