using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuItem : MonoBehaviour
{
    [SerializeField] private Animator selectedVizualizer = null;

    public void Select()
    {
        selectedVizualizer.SetBool("Selected", true);
    }

    public void Deselect()
    {
        selectedVizualizer.SetBool("Selected", false);
    }

    public virtual void Interact()
    {

    }
}
