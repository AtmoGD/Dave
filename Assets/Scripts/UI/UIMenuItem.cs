using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuItem : MonoBehaviour
{
    [SerializeField] private Animator selectedVizualizer = null;
    [SerializeField] private float selectSpeed = 2f;

    [SerializeField] public UIMenuItem up = null;
    [SerializeField] public UIMenuItem down = null;
    [SerializeField] public UIMenuItem left = null;
    [SerializeField] public UIMenuItem right = null;

    bool selected = false;

    public void Select()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
    }

    public virtual void Update()
    {
        float selectedFloat = selectedVizualizer.GetFloat("Active");

        selectedFloat = selectedFloat + (selected ? 1f : -1f) * selectSpeed * Time.deltaTime;
        selectedFloat = Mathf.Clamp(selectedFloat, 0f, 1f);

        selectedVizualizer.SetFloat("Active", selectedFloat);
    }

    public virtual void Interact()
    {

    }
}
