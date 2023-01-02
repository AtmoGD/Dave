using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortParticleLayer : MonoBehaviour
{
    [SerializeField] private int offset = 0;
    [SerializeField] private bool runOnlyOnce = false;
    [SerializeField] private bool runInEditor = false;

    private ParticleSystem particles = null;
    private int startSortingOrder = 0;
    private float startSortingOrderFloat = 0f;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        startSortingOrder = particles.GetComponent<Renderer>().sortingOrder;
        startSortingOrderFloat = particles.GetComponent<Renderer>().sortingOrder;
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
        particles.GetComponent<Renderer>().sortingOrder = (int)(startSortingOrderFloat - transform.position.y * 100) + offset;
    }
}
