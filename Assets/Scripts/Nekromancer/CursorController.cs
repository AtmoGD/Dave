using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private PlayerController player = null;
    private InputController inputController = null;

    // private Nekromancer nekromancer = null;

    // private void OnEnable()
    // {
    //     if (!nekromancer)
    //         nekromancer = GetComponent<Nekromancer>();

    //     if (!cursor)
    //         cursor = Instantiate(cursorPrefab, transform.position, Quaternion.identity).transform;

    //     nekromancer.OnUpdateControlls += OnUpdateControlls;
    // }

    // private void OnDisable()
    // {
    //     nekromancer.OnUpdateControlls -= OnUpdateControlls;
    // }

    public void Init(PlayerController _playerController)
    {
        player = _playerController;
        inputController = player.InputController;
    }

    private void Update()
    {
        if (inputController)
            Move();
    }

    private void Move()
    {
        Vector3 newPos = Vector3.Lerp(transform.position, inputController.InputData.CursorWorldPosition, moveSpeed * Time.deltaTime);
        transform.position = newPos;
    }

    public void OnUpdateControlls(string _controllSheme)
    {
        switch (_controllSheme)
        {
            case "Keyboard":
                gameObject.SetActive(true);
                break;
            case "Controller":
                gameObject.SetActive(false);
                break;
        }
    }
}
