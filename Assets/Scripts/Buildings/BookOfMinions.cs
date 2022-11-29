using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookOfMinions : Building
{
    public override void Interact(Nekromancer _nekromancer)
    {
        base.Interact(_nekromancer);

        _nekromancer.PlayerController.UIController.OpenMinionsMenu();
    }

}
