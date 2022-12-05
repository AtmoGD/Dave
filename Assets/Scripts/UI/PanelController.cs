using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected PlayerUIController playerUIController;
    [SerializeField] protected Placeable data;
    [SerializeField] protected TMP_Text placeableName;
    [SerializeField] protected TMP_Text placeableDescription;
    [SerializeField] protected TMP_Text placeableCost;
    [SerializeField] protected Button placeButton;

    private void Start()
    {
        if (!playerController) playerController = GetComponentInParent<PlayerController>();
        if (!playerUIController) playerUIController = GetComponentInParent<PlayerUIController>();

        if (data) UpdatePanel();

        placeButton.onClick.AddListener(PlaceObject);
    }

    private void OnDestroy()
    {
        placeButton.onClick.RemoveListener(PlaceObject);
    }

    void UpdatePanel()
    {
        placeableName.text = data.name;
        placeableDescription.text = data.description;
        placeableCost.text = data.cost.ToString();
    }

    public virtual void PlaceObject()
    {
        // playerController.PlaceObjectData(data);

        playerController.PlaceObject(data);
        playerUIController.CLoseAllMenus();
    }
}
