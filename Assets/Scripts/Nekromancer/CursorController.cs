using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float controllerSpeed = 10f;

    private PlayerController player = null;
    public InputController InputController { get; set; }

    private bool active = false;

    public void Init(PlayerController _playerController)
    {
        player = _playerController;
        InputController = player.InputController;

        InputController.OnControllShemeChanged += OnUpdateControlls;
    }

    private void Update()
    {
        if (InputController)
            Move();
    }

    private void Move()
    {
        Vector3 lerpPos;

        if (InputController.PlayerInput.currentControlScheme == "Keyboard")
        {
            StaticLib.GetWorldPosition(InputController.InputData.CursorPosition, out InputController.InputData.CursorWorldPosition);
            lerpPos = InputController.InputData.CursorWorldPosition;
        }
        else
            lerpPos = transform.position + (Vector3)InputController.InputData.MoveDir * controllerSpeed;

        Vector3 newPos = Vector3.Lerp(transform.position, lerpPos, moveSpeed * Time.deltaTime);
        transform.position = newPos;

        // This is a problem, because when i move the mouse, the controll sheme gets changed to keyboard
        // This causes the cursor to be very laggy

        // if (InputController.PlayerInput.currentControlScheme == "Controller")
        // {
        //     StaticLib.GetScreenPosition(newPos, out Vector2 mousePos);
        //     Mouse.current.WarpCursorPosition(mousePos);
        // }
    }

    public void SetCursorActive(bool _active)
    {
        active = _active;

        if (InputController.PlayerInput.currentControlScheme == "Keyboard")
            gameObject.SetActive(true);
        else
            gameObject.SetActive(active);
    }

    public void OnUpdateControlls(string _controllSheme)
    {
        switch (_controllSheme)
        {
            case "Keyboard":
                gameObject.SetActive(true);
                break;
            case "Controller":
                gameObject.SetActive(active);
                break;
        }
    }
}
