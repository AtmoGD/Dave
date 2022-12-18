using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelController : UIMenuItem
{
    [SerializeField] protected Placeable data;
    [SerializeField] protected TMP_Text placeableName;
    [SerializeField] protected TMP_Text placeableDescription;
    [SerializeField] protected TMP_Text placeableCost;
    [SerializeField] protected Button placeButton;

    protected PlayerController playerController;
    protected PlayerUIController playerUIController;

    private void Start()
    {
        if (!playerController) playerController = GetComponentInParent<PlayerController>();
        if (!playerUIController) playerUIController = GetComponentInParent<PlayerUIController>();

        if (data) UpdatePanel();
    }

    void UpdatePanel()
    {
        placeableName.text = data.name;
        placeableDescription.text = data.description;
        placeableCost.text = data.cost.ToString();
    }

    public virtual void PlaceObject()
    {
        playerController.PlaceObject(data);
        playerUIController.CLoseAllMenus();
    }

    public override void Interact()
    {
        base.Interact();

        PlaceObject();
    }
}
