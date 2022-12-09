using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject buildMenu = null;
    [SerializeField] private GameObject minionMenu = null;

    bool buildMenuOpen = false;
    bool minionMenuOpen = false;

    bool AnyMenuOpen { get { return buildMenuOpen || minionMenuOpen; } }

    private void Start()
    {
        buildMenu.SetActive(false);
        minionMenu.SetActive(false);
    }

    public void OpenBuildingsMenu()
    {
        buildMenu.SetActive(true);
        buildMenuOpen = true;

        player.nekromancer.BlockNekromancer(AnyMenuOpen);
    }

    public void CloseBuildingsMenu()
    {
        buildMenu.SetActive(false);
        buildMenuOpen = false;

        player.nekromancer.BlockNekromancer(AnyMenuOpen);
    }

    public void OpenMinionsMenu()
    {
        minionMenu.SetActive(true);
        minionMenuOpen = true;

        player.nekromancer.BlockNekromancer(AnyMenuOpen);
    }

    public void CloseMinionsMenu()
    {
        minionMenu.SetActive(false);
        minionMenuOpen = false;

        // player.Cursor.SetCursorActive(false);

        player.nekromancer.BlockNekromancer(AnyMenuOpen);
    }

    public void CLoseAllMenus()
    {
        CloseBuildingsMenu();
        CloseMinionsMenu();

        player.nekromancer.BlockNekromancer(AnyMenuOpen);
    }

    public void PlaceObject(string _id)
    {

    }

    public void PlaceObjectData(Placeable _placeable)
    {

    }
}
