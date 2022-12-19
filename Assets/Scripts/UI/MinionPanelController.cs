using UnityEngine;

public class MinionPanelController : PanelController
{

    public override void PlaceObject()
    {
        MinionData minionData = (MinionData)data;
        if (minionData)
        {
            playerController.PlaceMinion(minionData);
            playerUIController.CloseMenu();
        }
    }
}