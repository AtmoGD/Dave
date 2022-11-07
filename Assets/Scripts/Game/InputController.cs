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
    public Action<InputData> OnFirstAttackStart;
    public Action<InputData> OnFirstAttack;
    public Action<InputData> OnFirstAttackEnd;
    public Action<InputData> OnSecondAttackStart;
    public Action<InputData> OnSecondAttack;
    public Action<InputData> OnSecondAttackEnd;
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

    public void OnFirstAttackInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputData.FirstAttack = true;
            OnFirstAttackStart?.Invoke(inputData);
        }
        else if (_context.performed)
        {
            OnFirstAttack?.Invoke(inputData);
        }
        else if (_context.canceled)
        {
            inputData.FirstAttack = false;
            OnFirstAttackEnd?.Invoke(inputData);
        }
    }

    public void ResetFirstAttack() => inputData.FirstAttack = false;

    public void OnSecondAttackInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputData.SecondAttack = true;
            OnSecondAttackStart?.Invoke(inputData);
        }
        else if (_context.performed)
        {
            OnSecondAttack?.Invoke(inputData);
        }
        else if (_context.canceled)
        {
            inputData.SecondAttack = false;
            OnSecondAttackEnd?.Invoke(inputData);
        }
    }

    public void ResetSecondAttack() => inputData.SecondAttack = false;

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
