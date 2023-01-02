using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortLayer : MonoBehaviour
{
    [SerializeField] private int offset = 0;
    [SerializeField] private bool runOnlyOnce = false;
    [SerializeField] private bool runInEditor = false;

    private SpriteRenderer spriteRenderer = null;
    private int startSortingOrder = 0;
    private float startSortingOrderFloat = 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startSortingOrder = spriteRenderer.sortingOrder;
        startSortingOrderFloat = spriteRenderer.sortingOrder;
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
        spriteRenderer.sortingOrder = (int)(startSortingOrderFloat - transform.position.y * 100) + offset;
    }
}
