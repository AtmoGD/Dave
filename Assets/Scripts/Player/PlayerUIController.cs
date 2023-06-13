using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
public class PlayerUIController : MonoBehaviour
{
    [field: SerializeField] public PlayerController Player { get; private set; } = null;

    // [SerializeField] private FMODUnity.StudioEventEmitter titleScreenMusic = null;

    [SerializeField] private UIMenuController buildMenu = null;
    [SerializeField] private UIMenuController minionMenu = null;
    [SerializeField] private UIMenuController choosePerkMenu = null;
    [SerializeField] private UIMenuController pauseUI = null;
    [SerializeField] private UIMenuController endGameMenu = null;
    [SerializeField] private UIMenuController gameOverUI = null;
    [SerializeField] private UIMenuController chooseLevelUI = null;
    [SerializeField] private UIMenuController ritualUI = null;
    [SerializeField] public UIMenuController titlescreenUI = null;
    [SerializeField] private UIMenuController chooseDataPathUI = null;
    [SerializeField] private UIMenuController creditsUI = null;
    [SerializeField] private UIMenuController optionsUI = null;
    [SerializeField] private GameObject GatheredRessourcesContent = null;
    [SerializeField] private GameObject GatheredRessourcePrefab = null;
    [SerializeField] private UIMenuController gameLostUI = null;
    [SerializeField] private float menuInputDelay = 0.2f;

    UIMenuController currentMenu = null;

    bool AnyMenuOpen { get { return currentMenu; } }

    float lastMenuInput = 0f;

    public void Init()
    {
        buildMenu.SetIsActive(false);
        minionMenu.SetIsActive(false);
    }

    // # if UNITY_WEBGL
    //     private void Update()
    //     {
    //         if (currentMenu == titlescreenUI && !titleScreenMusic.IsPlaying())
    //             titleScreenMusic.Play();
    //     }
    // # endif

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

    public void OpenGameLostMenu()
    {
        OpenMenu(gameLostUI);
    }

    public void OpenChooseLevelMenu()
    {
        OpenMenu(chooseLevelUI);
    }

    public void OpenRitualMenu()
    {
        OpenMenu(ritualUI);
    }

    public void OpenTitleScreen()
    {
        OpenMenu(titlescreenUI);
        // print("OpenTitleScreen");
        // titleScreenMusic.Play();
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


        foreach (CollectedRessource ressource in SortRessources(LevelManager.Instance.GatheredRessources))
        {
            GameObject go = Instantiate(GatheredRessourcePrefab, GatheredRessourcesContent.transform);
            go.GetComponent<RessourceCard>()?.Init(ressource);
        }
    }

    public List<CollectedRessource> SortRessources(List<CollectedRessource> _ressources)
    {
        List<CollectedRessource> sortedRessources = new List<CollectedRessource>();

        foreach (CollectedRessource ressource in _ressources)
        {
            CollectedRessource newRessource = sortedRessources.Find(x => x.ressource == ressource.ressource);
            if (newRessource != null)
            {
                newRessource.amount += ressource.amount;
            }
            else
            {
                newRessource = new CollectedRessource();
                newRessource.ressource = ressource.ressource;
                newRessource.amount = ressource.amount;
                sortedRessources.Add(newRessource);
            }
        }

        return sortedRessources;
    }
}
