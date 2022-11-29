using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject buildMenu = null;
    [SerializeField] private GameObject buildContent = null;
    [SerializeField] private GameObject buildPanelPrefab = null;
    [SerializeField] private GameObject minionMenu = null;
    [SerializeField] private GameObject minionContent = null;
    [SerializeField] private GameObject minionPanelPrefab = null;

    bool buildMenuOpen = false;
    bool minionMenuOpen = false;

    private void Start()
    {
        buildMenu.SetActive(false);
        minionMenu.SetActive(false);
    }

    public void OpenBuildingsMenu()
    {
        buildMenu.SetActive(true);
        buildMenuOpen = true;
    }

    public void CloseBuildingsMenu()
    {
        buildMenu.SetActive(false);
        buildMenuOpen = false;
    }

    public void OpenMinionsMenu()
    {
        minionMenu.SetActive(true);
        minionMenuOpen = true;
    }

    public void CloseMinionsMenu()
    {
        minionMenu.SetActive(false);
        minionMenuOpen = false;
    }

    public void CLoseAllMenus()
    {
        CloseBuildingsMenu();
        CloseMinionsMenu();
    }

    public void PlaceObject(string _id)
    {

    }

    public void PlaceObjectData(Placeable _placeable)
    {

    }
}
