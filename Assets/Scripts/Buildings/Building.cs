using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : PlaceableObject, IInteractable
{
    public virtual void Interact(Nekromancer _nekromancer)
    {
        Debug.Log("Interacting with building " + this.gameObject.name);
    }

    public virtual void InteractEnd()
    {
        Debug.Log(" End Interacting with building " + this.gameObject.name);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
