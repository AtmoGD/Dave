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
    [SerializeField] private float menuInputDelay = 0.2f;

    UIMenuController currentMenu = null;

    bool AnyMenuOpen { get { return currentMenu; } }

    float lastMenuInput = 0f;

    public void Init()
    {
        // Player = _player;

        buildMenu.SetIsActive(false);
        minionMenu.SetIsActive(false);
    }

    public void OpenMenu(UIMenuController _menu)
    {
        _menu.SetIsActive(true);

        currentMenu = _menu;

        Player.Nekromancer.BlockNekromancer(AnyMenuOpen);
    }

    public void CloseMenu()
    {
        currentMenu?.SetIsActive(false);

        currentMenu = null;

        Player.Nekromancer.BlockNekromancer(AnyMenuOpen);
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

    public void NextItem(InputAction.CallbackContext _context)
    {
        if (!AnyMenuOpen || (Time.time - lastMenuInput) < menuInputDelay) return;

        lastMenuInput = Time.time;

        Vector2 dir = _context.ReadValue<Vector2>();

        if (Mathf.Abs(dir.x) >= 0.5f)
        {
            dir.x = dir.x > 0 ? 1 : -1;

            currentMenu.UpdateSelection(dir);
        }
    }

    public void Interact(InputAction.CallbackContext _context)
    {
        if (!AnyMenuOpen || (Time.time - lastMenuInput) < menuInputDelay) return;

        lastMenuInput = Time.time;

        currentMenu.InteractWithSelection();
    }

    public void Cancel(InputAction.CallbackContext _context)
    {
        if (!AnyMenuOpen || (Time.time - lastMenuInput) < menuInputDelay) return;

        lastMenuInput = Time.time;

        Cancel();
    }

    public void Cancel()
    {
        Player.Cancel();
    }
}
