using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookOfBuildings : Building
{
    public override void Interact(Nekromancer _nekromancer)
    {
        base.Interact(_nekromancer);

        _nekromancer.PlayerController.OpenBuildingsMenu();
    }

}
