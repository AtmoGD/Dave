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

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
