using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMenuController : UIMenuController
{
    [SerializeField] protected PlayerUIController playerUIController;
    [SerializeField] protected PerkPanelController unperkPanel;
    [SerializeField] protected int maxPerks = 3;
    [SerializeField] protected float perkCardSpawnDelay = 0.2f;
    [SerializeField] protected float perkCardVanishDelay = 0.2f;
    [SerializeField] protected float unPerkCardSpawnDelay = 0.8f;
    [SerializeField] protected bool closeMenuOnPerkChose = true;
    [SerializeField] protected float closeMenuDelay = 4f;

    public PlayerController PlayerController { get { return playerUIController.Player; } }

    bool choosedPerk = false;

    public void UpdatePerkCards()
    {
        // currentIndex = 0;

        choosedPerk = false;

        unperkPanel.InstantVanish();

        List<Perk> perks = new List<Perk>(GameManager.Instance.DataList.Perks);
        PlayerController player = GameManager.Instance.PlayerController;

        foreach (Perk perk in player.Perks)
        {
            if (perks.Find(x => x == perk))
            {
                perks.Remove(perk);
            }
        }

        List<Perk> perkChoices = new List<Perk>();
        for (int i = 0; i < maxPerks; i++)
        {
            if (perks.Count == 0) break;

            int randomIndex = Random.Range(0, perks.Count);
            perkChoices.Add(perks[randomIndex]);
            perks.RemoveAt(randomIndex);
        }

        float selectCardDelay = 0f;

        for (int i = 0; i < contentItems.Count; i++)
        {
            if (i >= perkChoices.Count) break;

            PerkPanelController perkPanel = contentItems[i] as PerkPanelController;
            if (perkPanel)
            {
                perkPanel.SetData(perkChoices[i]);
                perkPanel.Deselect();
                perkPanel.Spawn(perkCardSpawnDelay * (i + 1));
                selectCardDelay += perkCardSpawnDelay;
            }
        }

        StartCoroutine(SelectCardIn(selectCardDelay));
    }

    public void PlayerChosedPerk(PerkPanelController _panel, Perk _perk)
    {
        choosedPerk = true;

        PlayerController player = GameManager.Instance.PlayerController;

        player.PerkPoints--;

        foreach (PerkPanelController panel in contentItems)
        {
            if (panel == _panel)
            {
                panel.Choosen();
            }
            else
            {
                panel.Vanish();
            }
        }

        List<Perk> unPerks = new List<Perk>(GameManager.Instance.DataList.UnPerks);

        // TODO: I have no idea why the unPerks don't get removed from the list
        foreach (Perk unPerk in player.UnPerks)
        {
            if (unPerks.Find(x => x == unPerk))
            {
                unPerks.Remove(unPerk);
            }
        }

        if (unPerks.Count > 0)
        {
            int randomIndex = Random.Range(0, unPerks.Count);
            unperkPanel.SetData(unPerks[randomIndex]);
            unperkPanel.Spawn(unPerkCardSpawnDelay);
        }
        else
        {
            StartCoroutine(ClosePerkMenu());
        }

        if (closeMenuOnPerkChose)
        {
            StartCoroutine(ClosePerkMenu());
        }
    }

    IEnumerator ClosePerkMenu()
    {
        yield return new WaitForSeconds(closeMenuDelay);

        playerUIController.Cancel();
    }

    IEnumerator SelectCardIn(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        UpdateSelection(Vector2.right);
    }

    public override void UpdateSelection(Vector2 _dir)
    {
        if (choosedPerk) return;

        base.UpdateSelection(_dir);
    }

    public override void InteractWithSelection()
    {
        if (choosedPerk) return;

        base.InteractWithSelection();
    }
}
