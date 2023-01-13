using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIController : MonoBehaviour
{
    [field: SerializeField] public PlayerController Player { get; private set; } = null;

    [SerializeField] private UIMenuController buildMenu = null;
    [SerializeField] private UIMenuController minionMenu = null;
    [SerializeField] private UIMenuController choosePerkMenu = null;
    [SerializeField] private UIMenuController pauseUI = null;
    [SerializeField] private UIMenuController endGameMenu = null;
    [SerializeField] private UIMenuController gameOverUI = null;
    [SerializeField] private GameObject GatheredRessourcesContent = null;
    [SerializeField] private GameObject GatheredRessourcePrefab = null;
    [SerializeField] private UIMenuController gameLostUI = null;
    [SerializeField] private float menuInputDelay = 0.2f;

    UIMenuController currentMenu = null;

    bool AnyMenuOpen { get { return currentMenu; } }

    float lastMenuInput = 0f;

    public void Init()
    {
        // Player = _player;

        buildMenu.SetIsActive(false);
        minionMenu.SetIsActive(false);
        // choosePerkMenu.SetIsActive(false);
        // pauseUI.SetIsActive(false);
    }

    public void OpenMenu(UIMenuController _menu)
    {
        _menu.SetIsActive(true);

        currentMenu = _menu;

        Player.Nekromancer.BlockNekromancer(AnyMenuOpen);

        LevelManager.Instance.StopTime();
    }

    public void CloseMenu()
    {
        currentMenu?.SetIsActive(false);

        currentMenu = null;

        Player.Nekromancer.BlockNekromancer(AnyMenuOpen);

        LevelManager.Instance.StartTime();
    }

    public void OpenBuildingsMenu()
    {
        OpenMenu(buildMenu);
    }

    public void OpenMinionsMenu()
    {
        OpenMenu(minionMenu);
    }

    public void OpenChoosePerkMenu()
    {
        ((PerkMenuController)choosePerkMenu).UpdatePerkCards();
        OpenMenu(choosePerkMenu);
    }

    public void OpenPauseMenu()
    {
        OpenMenu(pauseUI);
    }

    public void OpenEndGameMenu()
    {
        OpenMenu(endGameMenu);
    }

    public void NextItem(InputAction.CallbackContext _context)
    {
        if (!AnyMenuOpen || (Time.time - lastMenuInput) < menuInputDelay) return;

        lastMenuInput = Time.time;

        Vector2 dir = _context.ReadValue<Vector2>();

        if (dir.magnitude >= 0.5f)
        {
            currentMenu.UpdateSelection(dir);
        }
    }

    public void Interact(InputAction.CallbackContext _context)
    {
        if (_context.started)
            currentMenu.InteractWithSelection();
    }

    public void Cancel(InputAction.CallbackContext _context)
    {
        if (_context.started)
            Cancel();
    }

    public void OpenPauseMenu(InputAction.CallbackContext _context)
    {
        if (_context.started)
            OpenPauseMenu();
    }

    public void Cancel()
    {
        Player.Cancel();
    }

    public void EndGame()
    {
        LevelManager.Instance.EndGame();

        CloseMenu();

        UpdateGatheredRessourcesOnGameOverScreen();

        OpenMenu(gameOverUI);
    }

    public void UpdateGatheredRessourcesOnGameOverScreen()
    {
        foreach (Transform child in GatheredRessourcesContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (CollectedRessource ressource in LevelManager.Instance.GatheredRessources)
        {
            GameObject go = Instantiate(GatheredRessourcePrefab, GatheredRessourcesContent.transform);
            go.GetComponent<RessourceCard>()?.Init(ressource);
        }
    }
}
