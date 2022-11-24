using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerUIController playerUIController;
    [SerializeField] private Placeable data;
    [SerializeField] private TMP_Text placeableName;
    [SerializeField] private TMP_Text placeableDescription;
    [SerializeField] private TMP_Text placeableCost;
    [SerializeField] private Button placeButton;

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

    public void PlaceObject()
    {
        // playerController.PlaceObjectData(data);

        playerController.PlaceObject(data);
        playerUIController.CLoseAllMenus();
    }
}
