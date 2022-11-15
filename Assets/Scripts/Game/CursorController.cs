using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using System;
using Cinemachine;


public class CursorController : MonoBehaviour
{
    public Action<Vector2> OnCursorMoved;

    [SerializeField] private CinemachineVirtualCamera cursorCamera = null;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float cameraSpeed = 1f;
    [SerializeField] private float padding = 35f;
    [SerializeField] private float moveThreshold = 0.1f;

    private Camera cam;
    private RectTransform canvasRect;
    private bool previousMouseState;
    private Mouse virtualMouse;

    private string previousControllSheme = "";
    private const string gamepadSheme = "Controller";
    private const string mouseSheme = "Keyboard";
    private bool shouldBeActive = false;
    private Vector2 cameraMoveDirection = Vector2.zero;

    private void OnEnable()
    {
        cam = Camera.main;
        canvasRect = canvas.GetComponent<RectTransform>();
        Cursor.visible = false;

        if (virtualMouse == null)
        {
            virtualMouse = InputSystem.AddDevice("VirtualMouse") as Mouse;
        }
        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (cursor != null)
        {
            Vector2 mousePosition = cursor.anchoredPosition;
            InputState.Change(virtualMouse.position, mousePosition);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
    }

    private void OnDisable()
    {
        if (virtualMouse != null && virtualMouse.added)
            InputSystem.RemoveDevice(virtualMouse);

        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    private void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null || !canvas.gameObject.activeSelf)
        {
            return;
        }

        if (cameraMoveDirection != Vector2.zero && shouldBeActive)
        {
            Vector2 dir = cameraMoveDirection * cameraSpeed * Time.deltaTime;
            cursorCamera.transform.position += new Vector3(dir.x, dir.y, 0f);
        }

        if (playerInput.currentControlScheme == gamepadSheme)
        {
            Vector2 delta = Gamepad.current.leftStick.ReadValue();
            delta *= speed * Time.deltaTime;

            Vector2 mousePosition = virtualMouse.position.ReadValue();
            Vector2 newPosition = mousePosition + delta;

            newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
            newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

            InputState.Change(virtualMouse.position, newPosition);
            InputState.Change(virtualMouse.delta, delta);

            bool southPressed = Gamepad.current.buttonSouth.isPressed;

            if (previousMouseState != southPressed)
            {
                virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, southPressed);
                InputState.Change(virtualMouse, mouseState);
                previousMouseState = southPressed;
            }

            AnchorCursor(newPosition);
        }
        else if (playerInput.currentControlScheme == mouseSheme)
        {
            AnchorCursor(Mouse.current.position.ReadValue());
        }

    }

    private void AnchorCursor(Vector2 _position)
    {
        Vector2 anchoredPosition;

        // We don't need the camera if we're using Screen Space - Overlay
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, _position, null, out anchoredPosition);
        else
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, _position, cam, out anchoredPosition);

        if (Vector2.Distance(cursor.anchoredPosition, anchoredPosition) > moveThreshold)
        {
            cursor.anchoredPosition = anchoredPosition;
            OnCursorMoved?.Invoke(cursor.position);
        }
    }

    public void OnControlsChanged(PlayerInput _playerInput)
    {
        if (virtualMouse == null) return;

        if (_playerInput.currentControlScheme == mouseSheme && previousControllSheme != mouseSheme)
        {
            previousControllSheme = mouseSheme;
            Mouse.current.WarpCursorPosition(virtualMouse.position.ReadValue());
        }
        else if (_playerInput.currentControlScheme == gamepadSheme && previousControllSheme != gamepadSheme)
        {
            previousControllSheme = gamepadSheme;
            AnchorCursor(Mouse.current.position.ReadValue());
            InputState.Change(virtualMouse.position, Mouse.current.position.ReadValue());
        }

        switch (_playerInput.currentControlScheme)
        {
            case mouseSheme:
                canvas.gameObject.SetActive(true);
                break;
            case gamepadSheme:
                canvas.gameObject.SetActive(shouldBeActive);
                break;
        }
    }

    public void SetCursorActive(bool _active)
    {
        shouldBeActive = _active;

        if (playerInput.currentControlScheme == mouseSheme)
            canvas.gameObject.SetActive(true);
        else
            canvas.gameObject.SetActive(shouldBeActive);
    }

    public void MoveUpEnter()
    {
        cameraMoveDirection.y = 1f;
    }

    public void MoveUpExit()
    {
        cameraMoveDirection.y = 0f;
    }

    public void MoveDownEnter()
    {
        cameraMoveDirection.y = -1f;
    }

    public void MoveDownExit()
    {
        cameraMoveDirection.y = 0f;
    }

    public void MoveLeftEnter()
    {
        cameraMoveDirection.x = -1f;
    }

    public void MoveLeftExit()
    {
        cameraMoveDirection.x = 0f;
    }

    public void MoveRightEnter()
    {
        cameraMoveDirection.x = 1f;
    }

    public void MoveRightExit()
    {
        cameraMoveDirection.x = 0f;
    }
}