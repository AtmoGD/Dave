using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkPanelController : UIMenuItem
{
    [SerializeField] protected Image perkIcon;
    [SerializeField] protected TMP_Text perkName;
    [SerializeField] protected TMP_Text perkDescription;

    protected Perk data;

    private void Start()
    {
        if (data) UpdatePerkCard();
    }

    public void SetData(Perk _data)
    {
        data = _data;
        UpdatePerkCard();
    }

    public void UpdatePerkCard()
    {
        perkIcon.sprite = data.perkIcon;
        perkName.text = data.perkName;
        perkDescription.text = data.perkDescription;
    }

    public override void Interact()
    {
        base.Interact();

        ActivatePerk();
    }

    public void ActivatePerk()
    {
        data.ActivatePerk(GetComponentInParent<PlayerController>());
    }
}
