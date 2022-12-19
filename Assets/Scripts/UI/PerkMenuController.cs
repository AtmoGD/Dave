using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMenuController : UIMenuController
{
    [SerializeField] protected int maxPerks = 3;
    public void UpdatePerkCards()
    {
        List<Perk> perks = new List<Perk>(GameManager.Instance.DataList.Perks);
        PlayerController player = GameManager.Instance.PlayerController;

        foreach (Perk perk in player.Perks)
        {
            if (perks.Contains(perk))
            {
                perks.Remove(perk);
            }
        }

        List<Perk> perkChoices = new List<Perk>();
        for (int i = 0; i < maxPerks; i++)
        {
            int randomIndex = Random.Range(0, perks.Count);
            perkChoices.Add(perks[randomIndex]);
            perks.RemoveAt(randomIndex);
        }

        for (int i = 0; i < contentItems.Count; i++)
        {
            if (i >= perkChoices.Count) break;

            PerkPanelController perkPanel = contentItems[i] as PerkPanelController;
            if (perkPanel)
            {
                perkPanel.SetData(perkChoices[i]);
            }
        }
    }
}
