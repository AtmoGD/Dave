using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampManager : MonoBehaviour
{
    public static CampManager Instance { get; protected set; }

    [Header("Camp Manager")]
    [SerializeField] private Vector2Int campSize = new Vector2Int(10, 10);
    public Vector2Int CampSize { get { return campSize; } }
    [field: SerializeField] public GameObject campPrefab { get; private set; } = null;
    [SerializeField] public Vector2 NekromancerSpawnPosition = Vector2.zero;
    // [SerializeField] private GameObject chooseLevelUI = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(NekromancerSpawnPosition, new Vector3(1, 1, 0));
    }

    // public void Start()
    // {
    //     if (chooseLevelUI) chooseLevelUI.SetActive(false);
    // }

    // public void OpenChooseLevelUI()
    // {
    //     chooseLevelUI.SetActive(true);
    // }

    // public void CloseChooseLevelUI()
    // {
    //     chooseLevelUI.SetActive(false);
    // }
}
