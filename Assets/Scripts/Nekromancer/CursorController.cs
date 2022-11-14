using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class CursorController : MonoBehaviour
{
    public Action<Vector2> OnCursorMoved;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float controllerSpeed = 10f;
    [SerializeField] private float moveThreshold = 0.1f;
    [SerializeField] private LayerMask gridLayer = 12;

    private PlayerController player = null;
    public InputController InputController { get; set; }

    private bool active = false;

    private GridElement currentGridElement = null;

    public void Init(PlayerController _playerController)
    {
        player = _playerController;
        InputController = player.InputController;

        InputController.OnControllShemeChanged += OnUpdateControlls;
        // InputController.OnCursorMove += CheckForGridElement;
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


        if (Vector2.Distance(transform.position, lerpPos) > moveThreshold)
        {
            transform.position = Vector3.Lerp(transform.position, lerpPos, moveSpeed * Time.deltaTime);
            OnCursorMoved?.Invoke(transform.position);
        }
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
