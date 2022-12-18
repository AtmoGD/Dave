using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIController : MonoBehaviour
{
    public PlayerController Player { get; private set; } = null;

    [SerializeField] private UIMenuController buildMenu = null;
    [SerializeField] private UIMenuController minionMenu = null;
    [SerializeField] private float menuInputDelay = 0.2f;

    UIMenuController currentMenu = null;

    bool AnyMenuOpen { get { return currentMenu; } }

    float lastMenuInput = 0f;

    public void Init(PlayerController _player)
    {
        Player = _player;

        buildMenu.SetIsActive(false);
        minionMenu.SetIsActive(false);
    }

    public void OpenBuildingsMenu()
    {
        buildMenu.SetIsActive(true);

        currentMenu = buildMenu;

        Player.Nekromancer.BlockNekromancer(AnyMenuOpen);
    }

    public void CloseBuildingsMenu()
    {
        buildMenu.SetIsActive(false);

        currentMenu = null;

        Player.Nekromancer.BlockNekromancer(AnyMenuOpen);
    }

    public void OpenMinionsMenu()
    {
        minionMenu.SetIsActive(true);

        currentMenu = minionMenu;

        Player.Nekromancer.BlockNekromancer(AnyMenuOpen);
    }

    public void CloseMinionsMenu()
    {
        minionMenu.SetIsActive(false);

        currentMenu = null;

        Player.Nekromancer.BlockNekromancer(AnyMenuOpen);
    }

    public void CLoseAllMenus()
    {
        CloseBuildingsMenu();
        CloseMinionsMenu();

        Player.Nekromancer.BlockNekromancer(AnyMenuOpen);
    }

    public void NextItem(InputAction.CallbackContext _context)
    {
        if (!AnyMenuOpen || (Time.time - lastMenuInput) < menuInputDelay) return;

        lastMenuInput = Time.time;

        Vector2 dir = _context.ReadValue<Vector2>();

        if (Mathf.Abs(dir.x) >= 0.5f)
        {
            int val = dir.x > 0 ? 1 : -1;

            currentMenu.UpdateSelection(val);
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

        Player.Cancel();
    }
}
