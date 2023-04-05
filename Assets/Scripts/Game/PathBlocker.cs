using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBlocker : MonoBehaviour
{
    [field: SerializeField] private float UpdateTime { get; set; } = 0.5f;

    private GridElement lastElement = null;
    private float lastUpdate = 0f;


    private void Start()
    {
        lastUpdate = Time.time;
    }

    private void Update()
    {
        if (Time.time - lastUpdate > UpdateTime)
        {
            lastUpdate = Time.time;

            GridElement currentElement = WorldGrid.Instance.GetGridElement(transform.position);

            if (currentElement && currentElement != lastElement && currentElement.ObjectOnGrid == null)
            {
                if (lastElement != null)
                    lastElement.ObjectOnGrid = null;

                currentElement.ObjectOnGrid = this.gameObject;
                lastElement = currentElement;
            }
        }
    }
}
