using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private Animator loadingScreen = null;
    [SerializeField] private UIMenuController pauseUI = null;
    [SerializeField] private float menuInputDelay = 0.2f;

    private UIMenuController currentMenu = null;
    float lastMenuInput = 0f;

    bool AnyMenuOpen { get { return currentMenu; } }

    private const string baseActionMap = "BaseMap";
    private const string uiActionMap = "UI";

    public void Pause(bool _pause)
    {
        pauseUI.SetIsActive(_pause);

        if (_pause)
        {
            LevelManager.Instance.StopTime();
            playerInput.SwitchCurrentActionMap(uiActionMap);
            currentMenu = pauseUI;
        }
        else
        {
            LevelManager.Instance.StartTime();
            playerInput.SwitchCurrentActionMap(baseActionMap);
            currentMenu = null;
        }
    }
    public void NextItem(InputAction.CallbackContext _context)
    {
        if (!AnyMenuOpen || (Time.time - lastMenuInput) < menuInputDelay) return;

        lastMenuInput = Time.time;

        Vector2 dir = _context.ReadValue<Vector2>();

        if (dir.magnitude >= 0.5f)
        {
            // dir.x = dir.x > 0 ? 1 : -1;

            currentMenu.UpdateSelection(dir);
        }
    }

    public void Interact(InputAction.CallbackContext _context)
    {
        print("Interact");
        if (_context.started)
            currentMenu.InteractWithSelection();
    }

    public void Cancel(InputAction.CallbackContext _context)
    {
        if (_context.started)
            Cancel();
    }

    public void Cancel()
    {
        Pause(false);
    }

    public void StartLoadGame()
    {
        loadingScreen.SetTrigger("StartLoading");
    }

    public void LoadNewGame()
    {
        GameManager.Instance.ReloadGame();
    }
}
