using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IInteractable
{
    [SerializeField] protected PlayerController player = null;

    public virtual void Interact(Nekromancer _nekromancer)
    {
        Debug.Log("Interacting with building");
    }

    public virtual void InteractEnd()
    {
        Debug.Log("Interacting with building");
    }
}
