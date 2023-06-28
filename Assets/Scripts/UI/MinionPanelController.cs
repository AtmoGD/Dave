using UnityEngine;

public class MinionPanelController : PanelController
{

    public override void PlaceObject()
    {
        MinionData minionData = (MinionData)data;
        if (minionData)
        {
            bool hasEnoughRessources = true;
            foreach (CollectedRessource ressource in minionData.cost)
            {
                if (!LevelManager.Instance.HasEnoughRessources(ressource.ressource, ressource.amount))
                {
                    hasEnoughRessources = false;
                    break;
                }
            }
            if (!hasEnoughRessources)
                return;

            playerController.PlaceMinion(minionData);
            playerUIController.CloseMenu();

            foreach (CollectedRessource ressource in minionData.cost)
            {
                LevelManager.Instance.RemoveRessources(ressource.ressource, ressource.amount);
            }
        }
    }

}