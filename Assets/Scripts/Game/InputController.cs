using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputController : MonoBehaviour
{
    public Action<string> OnControllShemeChanged = null;
    public Action<InputData> OnCursorMove;
    public Action<InputData> OnMove;
    public Action<InputData> OnLook;
    public Action<InputData> OnInteractStart;
    public Action<InputData> OnInteract;
    public Action<InputData> OnInteractEnd;
    public Action<InputData> OnCancel;
    public Action<InputData> OnPlaceObjectStart;
    public Action<InputData> OnPlaceObject;
    public Action<InputData> OnPlaceObjectEnd;
    public Action<InputData> OnOpenBuildMenu;
    public Action<InputData> OnOpenMinionMenu;
    public Action<InputData> OnBaseSkillStart;
    public Action<InputData> OnBaseSkill;
    public Action<InputData> OnBaseSkillEnd;
    public Action<InputData> OnBaseChargeSkillStart;
    public Action<InputData> OnBaseChargeSkill;
    public Action<InputData> OnBaseChargeSkillEnd;
    public Action<InputData> OnFirstSkillStart;
    public Action<InputData> OnFirstSkill;
    public Action<InputData> OnFirstSkillEnd;
    public Action<InputData> OnSecondSkillStart;
    public Action<InputData> OnSecondSkill;
    public Action<InputData> OnSecondSkillEnd;
    public Action<InputData> OnFirstItemStart;
    public Action<InputData> OnFirstItem;
    public Action<InputData> OnFirstItemEnd;
    public Action<InputData> OnSecondItemStart;
    public Action<InputData> OnSecondItem;
    public Action<InputData> OnSecondItemEnd;
    public Action<InputData> OnThirdItemStart;
    public Action<InputData> OnThirdItem;
    public Action<InputData> OnThirdItemEnd;
    public Action<InputData> OnFourthItemStart;
    public Action<InputData> OnFourthItem;
    public Action<InputData> OnFourthItemEnd;

    [SerializeField] private InputData inputData = new InputData();
    public InputData InputData { get { return inputData; } }

    [SerializeField] private PlayerInput playerInput = null;
    public PlayerInput PlayerInput { get { return playerInput; } }

    public void ChangeActionMap(string _name)
    {
        playerInput.SwitchCurrentActionMap(_name);
    }

    public void OnControllsChanged(PlayerInput _input)
    {
        inputData.ControllSheme = _input.currentControlScheme;
        OnControllShemeChanged?.Invoke(_input.currentControlScheme);
    }

    public void OnMouseMove(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            inputData.CursorPosition = _context.ReadValue<Vector2>();
            StaticLib.GetWorldPosition(inputData.CursorPosition, out inputData.CursorWorldPosition);
            OnCursorMove?.Invoke(inputData);
        }
    }

    public void OnMoveInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            inputData.MoveDir = _context.ReadValue<Vector2>();
            OnMove?.Invoke(inputData);
        }
        if (_context.canceled)
        {
            inputData.MoveDir = Vector2.zero;
            OnMove?.Invoke(inputData);
        }
    }

    public void OnLookInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            inputData.LookDir = _context.ReadValue<Vector2>();
            OnLook?.Invoke(inputData);
        }
    }

    public void OnInteractInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputData.Interact = true;
            OnInteractStart?.Invoke(inputData);
        }
        if (_context.performed)
        {
            OnInteract?.Invoke(inputData);
        }
        if (_context.canceled)
        {
            inputData.Interact = false;
            OnInteractEnd?.Invoke(inputData);
        }
    }
    public void ResetInteract() => inputData.Interact = false;

    public void OnCancelInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            inputData.Cancel = true;
            OnCancel?.Invoke(inputData);
        }
        if (_context.canceled)
        {
            inputData.Cancel = false;
            OnCancel?.Invoke(inputData);
        }
    }
    public void ResetCancel() => inputData.Cancel = false;

    public void OnPlaceObjectInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputData.PlaceObject = true;
            OnPlaceObjectStart?.Invoke(inputData);
        }
        if (_context.performed)
        {
            OnPlaceObject?.Invoke(inputData);
        }
        if (_context.canceled)
        {
            inputData.PlaceObject = false;
            OnPlaceObjectEnd?.Invoke(inputData);
        }
    }
    public void ResetPlaceObject() => inputData.PlaceObject = false;

    public void OnOpenBuildMenuInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            inputData.OpenBuildingsMenu = true;
            OnOpenBuildMenu?.Invoke(inputData);
        }
        if (_context.canceled)
        {
            inputData.OpenBuildingsMenu = false;
        }
    }
    public void ResetOpenBuildMenu() => inputData.OpenBuildingsMenu = false;

    public void OnOpenMinionMenuInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            inputData.OpenMinionsMenu = true;
            OnOpenMinionMenu?.Invoke(inputData);
        }
        if (_context.canceled)
        {
            inputData.OpenMinionsMenu = false;
        }
    }
    public void ResetOpenMinionMenu() => inputData.OpenMinionsMenu = false;

    public void OnBaseSkillInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputData.BaseSkill = true;
            OnBaseSkillStart?.Invoke(inputData);
        }
        else if (_context.performed)
        {
            OnBaseSkill?.Invoke(inputData);
        }
        else if (_context.canceled)
        {
            inputData.BaseSkill = false;
            OnBaseSkillEnd?.Invoke(inputData);
        }
    }

    public void ResetFirstAttack() => inputData.BaseSkill = false;

    public void OnSecondAttackInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputData.BaseSkillCharge = true;
            OnBaseChargeSkillStart?.Invoke(inputData);
        }
        else if (_context.performed)
        {
            OnBaseChargeSkill?.Invoke(inputData);
        }
        else if (_context.canceled)
        {
            inputData.BaseSkillCharge = false;
            OnBaseChargeSkillEnd?.Invoke(inputData);
        }
    }

    public void ResetSecondAttack() => inputData.BaseSkillCharge = false;

    public void OnFirstSkillInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputData.FirstSkill = true;
            OnFirstSkillStart?.Invoke(inputData);
        }
        else if (_context.performed)
        {
            OnFirstSkill?.Invoke(inputData);
        }
        else if (_context.canceled)
        {
            inputData.FirstSkill = false;
            OnFirstSkillEnd?.Invoke(inputData);
        }
    }

    public void ResetFirstSkill() => inputData.FirstSkill = false;

    public void OnSecondSkillInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputData.SecondSkill = true;
            OnSecondSkillStart?.Invoke(inputData);
        }
        else if (_context.performed)
        {
            OnSecondSkill?.Invoke(inputData);
        }
        else if (_context.canceled)
        {
            inputData.SecondSkill = false;
            OnSecondSkillEnd?.Invoke(inputData);
        }
    }

    public void ResetSecondSkill() => inputData.SecondSkill = false;

    public void OnFirstItemInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputData.FirstItem = true;
            OnFirstItemStart?.Invoke(inputData);
        }
        else if (_context.performed)
        {
            OnFirstItem?.Invoke(inputData);
        }
        else if (_context.canceled)
        {
            inputData.FirstItem = false;
            OnFirstItemEnd?.Invoke(inputData);
        }
    }

    public void ResetFirstItem() => inputData.FirstItem = false;

    public void OnSecondItemInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputData.SecondItem = true;
            OnSecondItemStart?.Invoke(inputData);
        }
        else if (_context.performed)
        {
            OnSecondItem?.Invoke(inputData);
        }
        else if (_context.canceled)
        {
            inputData.SecondItem = false;
            OnSecondItemEnd?.Invoke(inputData);
        }
    }

    public void ResetSecondItem() => inputData.SecondItem = false;

    public void OnThirdItemInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputData.ThirdItem = true;
            OnThirdItemStart?.Invoke(inputData);
        }
        else if (_context.performed)
        {
            OnThirdItem?.Invoke(inputData);
        }
        else if (_context.canceled)
        {
            inputData.ThirdItem = false;
            OnThirdItemEnd?.Invoke(inputData);
        }
    }

    public void ResetThirdItem() => inputData.ThirdItem = false;

    public void OnFourthItemInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputData.FourthItem = true;
            OnFourthItemStart?.Invoke(inputData);
        }
        else if (_context.performed)
        {
            OnFourthItem?.Invoke(inputData);
        }
        else if (_context.canceled)
        {
            inputData.FourthItem = false;
            OnFourthItemEnd?.Invoke(inputData);
        }
    }

    public void ResetFourthItem() => inputData.FourthItem = false;
}
