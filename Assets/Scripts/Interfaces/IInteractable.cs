using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(Nekromancer _nekromancer);
    void InteractEnd();
    Transform GetTransform();
}
