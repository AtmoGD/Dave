using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GridElement : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] public WorldGrid worldGrid = null;
    [SerializeField] public Vector2Int gridPosition = Vector2Int.zero;
    [SerializeField] public Placeable objectOnGrid = null;

    public void SetElementActive(bool _active)
    {
        if (_active)
        {
            gameObject.SetActive(true);
            animator.SetFloat("Active", 1f);
        }
        else
        {
            animator.SetFloat("Active", 0f);
        }
        // animator.SetFloat("Active", _active ? 1f : 0f);
    }

    public void DeactivateElement()
    {
        gameObject.SetActive(false);
    }
}
