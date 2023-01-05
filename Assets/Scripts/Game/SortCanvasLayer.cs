using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortCanvasLayer : MonoBehaviour
{
    [SerializeField] private int offset = 0;
    [SerializeField] private bool runOnlyOnce = false;
    [SerializeField] private bool runInEditor = false;

    // private SpriteRenderer spriteRenderer = null;
    private Canvas canvas = null;
    private int startSortingOrder = 0;
    private float startSortingOrderFloat = 0f;

    private void Awake()
    {
        // spriteRenderer = GetComponent<SpriteRenderer>();
        canvas = GetComponent<Canvas>();
        // startSortingOrder = spriteRenderer.sortingOrder;
        canvas.sortingOrder = startSortingOrder;
    }

    private void Start()
    {
        if (runInEditor)
            SetSortingOrder();
    }

    private void Update()
    {
        if (runOnlyOnce)
            return;

        SetSortingOrder();
    }

    private void SetSortingOrder()
    {
        canvas.sortingOrder = (int)(startSortingOrderFloat - transform.position.y * 100) + offset;
    }
}
