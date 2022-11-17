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

    private void Start()
    {
        buildMenu.SetActive(false);
        minionMenu.SetActive(false);
    }

    public void OpenBuildingsMenu()
    {
        buildMenu.SetActive(true);
    }

    public void CloseBuildingsMenu()
    {
        buildMenu.SetActive(false);
    }

    public void OpenMinionsMenu()
    {
        minionMenu.SetActive(true);
    }

    public void CloseMinionsMenu()
    {
        minionMenu.SetActive(false);
    }

    public void CLoseAllMenus()
    {
        CloseBuildingsMenu();
        CloseMinionsMenu();
    }
}
