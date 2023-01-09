using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonItem : UIMenuItem
{
    [SerializeField] private Button button = null;

    public override void Interact()
    {
        base.Interact();

        button?.Select();
    }
}
