using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] public Nekromancer nekromancer = null;
    [SerializeField] private Nekromancer nekromancerPrefab = null;
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private CursorController cursor = null;


    // [Title("Dynamic References", "These are set dynamically during initialization")]
    public InputController InputController { get; private set; }

    public void Init(InputController _inputController)
    {
        InputController = _inputController;

        if (!nekromancer)
            nekromancer = Instantiate(nekromancerPrefab, spawnPoint.position, Quaternion.identity);

        nekromancer.Init(this);
        cursor.Init(this);
    }
}
