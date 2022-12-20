using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkPanelController : UIMenuItem
{
    [SerializeField] protected Animator perkCardAnimator;
    [SerializeField] protected PerkMenuController perkMenuController;
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

    public void Spawn(float _delay = 0f)
    {
        StartCoroutine(SetTriggerAfterDelay("Spawn", _delay));
    }

    public void Choosen(float _delay = 0f)
    {
        Deselect();
        StartCoroutine(SetTriggerAfterDelay("Choosen", _delay));
    }

    public void Vanish(float delay = 0f)
    {
        StartCoroutine(SetTriggerAfterDelay("Vanish", delay));
    }

    public void InstantVanish()
    {
        perkCardAnimator.SetTrigger("InstantVanish");
    }

    IEnumerator SetTriggerAfterDelay(string _trigger, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        perkCardAnimator.SetTrigger(_trigger);
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
        data.ActivatePerk(perkMenuController.PlayerController);
        perkMenuController.PlayerChosedPerk(this, data);
    }
}
