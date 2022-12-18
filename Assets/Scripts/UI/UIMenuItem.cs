using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuItem : MonoBehaviour
{
    [SerializeField] private Animator selectedVizualizer = null;
    [SerializeField] private float selectSpeed = 2f;
    bool selected = false;

    public void Select()
    {
        selected = true;
        // selectedVizualizer.SetBool("Selected", true);
    }

    public void Deselect()
    {
        selected = false;
        // selectedVizualizer.SetBool("Selected", false);
    }

    public virtual void Update()
    {
        float selectedFloat = selectedVizualizer.GetFloat("Active");
        selectedFloat = selectedFloat + (selected ? 1f : -1f) * selectSpeed * Time.deltaTime;
        selectedFloat = Mathf.Clamp(selectedFloat, 0f, 1f);
        // selectedFloat = Mathf.Lerp(selectedFloat, selected ? 1f : 0f, selectSpeed * Time.deltaTime);
        selectedVizualizer.SetFloat("Active", selectedFloat);
    }

    public virtual void Interact()
    {

    }
}
